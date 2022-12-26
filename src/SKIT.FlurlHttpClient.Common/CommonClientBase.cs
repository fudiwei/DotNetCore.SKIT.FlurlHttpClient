using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;

namespace SKIT.FlurlHttpClient
{
    using SKIT.FlurlHttpClient.Configuration;
    using SKIT.FlurlHttpClient.Configuration.Internal;
    using SKIT.FlurlHttpClient.Exceptions;

    /// <summary>
    /// SKIT.FlurlHttpClient 客户端基类。
    /// </summary>
    public abstract class CommonClientBase : ICommonClient
    {
        private bool _disposed;

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public HttpInterceptorCollection Interceptors { get; }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public IJsonSerializer JsonSerializer
        {
            get { return ((InternalWrappedJsonSerializer)FlurlClient.Settings.JsonSerializer)!.Serializer; }
        }

        /// <summary>
        /// 获取当前客户端使用的 <see cref="IFlurlClient"/> 对象。
        /// </summary>
        protected IFlurlClient FlurlClient { get; }

        /// <summary>
        ///
        /// </summary>
        protected CommonClientBase()
        {
            Interceptors = new HttpInterceptorCollection();
            FlurlClient = new FlurlClient();
            FlurlClient.Configure(flurlSettings =>
            {
                flurlSettings.JsonSerializer = new InternalWrappedJsonSerializer(new SystemTextJsonSerializer());
                flurlSettings.BeforeCallAsync = async (flurlCall) =>
                {
                    using CancellationTokenSource cts = new CancellationTokenSource();
                    if (flurlSettings.Timeout.HasValue)
                        cts.CancelAfter(flurlSettings.Timeout.Value);

                    HttpInterceptorContext context = flurlCall.GetHttpInterceptorContext();
                    for (int i = 0, len = Interceptors.Count; i < len; i++)
                    {
                        cts.Token.ThrowIfCancellationRequested();

                        try
                        {
                            await Interceptors[i].BeforeCallAsync(context, cancellationToken: cts.Token);
                        }
                        catch (OperationCanceledException)
                        {
                            throw;
                        }
                        catch (Exception ex)
                        {
                            throw new CommonInterceptorCallException(flurlCall, ex.Message, ex);
                        }
                    }
                };
                flurlSettings.AfterCallAsync = async (flurlCall) =>
                {
                    using CancellationTokenSource cts = new CancellationTokenSource();
                    if (flurlSettings.Timeout.HasValue)
                        cts.CancelAfter(flurlSettings.Timeout.Value);

                    HttpInterceptorContext context = flurlCall.GetHttpInterceptorContext();
                    for (int i = Interceptors.Count - 1; i >= 0; i--)
                    {
                        cts.Token.ThrowIfCancellationRequested();

                        try
                        {
                            await Interceptors[i].AfterCallAsync(context, cancellationToken: cts.Token);
                        }
                        catch (OperationCanceledException)
                        {
                            throw;
                        }
                        catch (Exception ex)
                        {
                            throw new CommonInterceptorCallException(flurlCall, ex.Message, ex);
                        }
                    }

                    context.Items.Clear();
                };
            });
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="configure"></param>
        public void Configure(Action<CommonClientSettings> configure)
        {
            if (configure == null) throw new ArgumentNullException(nameof(configure));

            FlurlClient.Configure(flurlClientSettings =>
            {
                CommonClientSettings settings = new CommonClientSettings(flurlClientSettings);
                configure.Invoke(settings);

                flurlClientSettings.Timeout = settings.Timeout;
                flurlClientSettings.JsonSerializer = new InternalWrappedJsonSerializer(settings.JsonSerializer);
                flurlClientSettings.HttpClientFactory = settings.FlurlHttpClientFactory;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual IFlurlRequest CreateFlurlRequest(CommonRequestBase request, HttpMethod method, params object[] urlSegments)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            IFlurlRequest flurlRequest = FlurlClient.Request(urlSegments).WithVerb(method);

            if (request.Timeout != null)
            {
                flurlRequest.WithTimeout(request.Timeout.Value);
            }

            return flurlRequest;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flurlRequest"></param>
        /// <param name="httpContent"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="CommonRequestTimeoutException"></exception>
        /// <exception cref="CommonException"></exception>
        protected virtual async Task<IFlurlResponse> SendFlurlRequestAsync(IFlurlRequest flurlRequest, HttpContent? httpContent = null, CancellationToken cancellationToken = default)
        {
            if (flurlRequest == null) throw new ArgumentNullException(nameof(flurlRequest));
            if (_disposed) throw new ObjectDisposedException(nameof(FlurlClient));

            try
            {
                return await flurlRequest
                    .AllowAnyHttpStatus()
                    .SendAsync(flurlRequest.Verb, httpContent, cancellationToken: cancellationToken);
            }
            catch (FlurlHttpTimeoutException ex)
            {
                throw new CommonTimeoutException(ex.Message, ex);
            }
            catch (FlurlParsingException ex)
            {
                throw new CommonSerializationException(ex.Message, ex);
            }
            catch (FlurlHttpException ex)
            {
                throw new CommonException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flurlRequest"></param>
        /// <param name="data"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="CommonRequestTimeoutException"></exception>
        /// <exception cref="CommonRequestSerializationException"></exception>
        /// <exception cref="CommonException"></exception>
        protected virtual async Task<IFlurlResponse> SendFlurlRequestAsJsonAsync(IFlurlRequest flurlRequest, object? data = null, CancellationToken cancellationToken = default)
        {
            if (flurlRequest == null) throw new ArgumentNullException(nameof(flurlRequest));
            if (_disposed) throw new ObjectDisposedException(nameof(FlurlClient));

            if (data != null)
            {
                if (!flurlRequest.Headers.Contains(Constants.HttpHeaders.ContentType))
                {
                    flurlRequest.WithHeader(Constants.HttpHeaders.ContentType, "application/json");
                }
            }

            try
            {
                return await flurlRequest
                    .AllowAnyHttpStatus()
                    .SendJsonAsync(flurlRequest.Verb, data, cancellationToken: cancellationToken);
            }
            catch (FlurlHttpTimeoutException ex)
            {
                throw new CommonTimeoutException(ex.Message, ex);
            }
            catch (FlurlParsingException ex)
            {
                throw new CommonSerializationException(ex.Message, ex);
            }
            catch (FlurlHttpException ex)
            {
                throw new CommonException(ex.Message, ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="flurlResponse"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected async Task<TResponse> WrapFlurlResponseAsync<TResponse>(IFlurlResponse flurlResponse, CancellationToken cancellationToken = default)
            where TResponse : CommonResponseBase, new()
        {
            if (flurlResponse == null) throw new ArgumentNullException(nameof(flurlResponse));

            Task<byte[]> task = flurlResponse.GetBytesAsync();
            Task taskWithCt = await Task.WhenAny(task, Task.Delay(Timeout.Infinite, cancellationToken));
            if (taskWithCt == task)
            {
                TResponse result = new TResponse();
                result.RawStatus = flurlResponse.StatusCode;
                result.RawHeaders = new HttpHeaderCollection(flurlResponse.Headers);
                result.RawBytes = await task;
                return result;
            }
            else
            {
                cancellationToken.ThrowIfCancellationRequested();
                throw new OperationCanceledException("Infinite delay task completed.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="flurlResponse"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected async Task<TResponse> WrapFlurlResponseAsJsonAsync<TResponse>(IFlurlResponse flurlResponse, CancellationToken cancellationToken = default)
            where TResponse : CommonResponseBase, new()
        {
            if (flurlResponse == null) throw new ArgumentNullException(nameof(flurlResponse));

            TResponse tmp = await WrapFlurlResponseAsync<TResponse>(flurlResponse, cancellationToken);
            byte tb1 = byte.MinValue,
                 tb2 = byte.MinValue;
            for (long i = 0; i < tmp.RawBytes.LongLength; i++)
            {
                tb1 = tmp.RawBytes[i];
                if (tb1 > 32)
                    break;
            }
            for (long i = tmp.RawBytes.LongLength - 1; i >= 0; i--)
            {
                tb2 = tmp.RawBytes[i];
                if (tb2 > 32)
                    break;
            }

            TResponse result;
            if ((tb1 == 91 && tb2 == 93) || (tb1 == 123 && tb2 == 125)) // "[...]" or "{...}"
            {
                try
                {
                    string? contentType = flurlResponse.Headers.GetAll(Constants.HttpHeaders.ContentType).FirstOrDefault();
                    string? charset = MediaTypeHeaderValue.TryParse(contentType, out var mediaType) ? mediaType.CharSet : null;
                    string json = (string.IsNullOrEmpty(charset) ? Encoding.UTF8 : Encoding.GetEncoding(charset)).GetString(tmp.RawBytes);

                    result = JsonSerializer.Deserialize<TResponse>(json);
                    result.RawStatus = tmp.RawStatus;
                    result.RawHeaders = tmp.RawHeaders;
                    result.RawBytes = tmp.RawBytes;
                }
                catch (Exception ex)
                {
                    throw new CommonSerializationException(ex.Message, ex);
                }
            }
            else
            {
                result = tmp;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    FlurlClient.Dispose();
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
