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
    using SKIT.FlurlHttpClient.Constants;
    using SKIT.FlurlHttpClient.Exceptions;

    /// <summary>
    /// SKIT.FlurlHttpClient 通用客户端基类。
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
        /// <inheritdoc />
        /// </summary>
        public IFormUrlEncodedSerializer FormUrlEncodedSerializer
        {
            get { return ((InternalWrappedFormUrlEncodedSerializer)FlurlClient.Settings.UrlEncodedSerializer)!.Serializer; }
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
            FlurlClient.WithSettings(flurlSettings =>
            {
                IJsonSerializer jsonSerializer = new SystemTextJsonSerializer();
                IFormUrlEncodedSerializer formUrlEncodedSerializer = new JsonifiedFormUrlEncodedSerializer(jsonSerializer);
                flurlSettings.JsonSerializer = new InternalWrappedJsonSerializer(jsonSerializer);
                flurlSettings.UrlEncodedSerializer = new InternalWrappedFormUrlEncodedSerializer(formUrlEncodedSerializer);
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
                            if (ex is CommonException)
                                throw;

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
                            if (ex is CommonException)
                                throw;

                            throw new CommonInterceptorCallException(flurlCall, ex.Message, ex);
                        }
                    }

                    context.Items.Clear();
                };
            });
        }

        /// <inheritdoc/>
        public void Configure(Action<CommonClientSettings> configure)
        {
            if (configure == null) throw new ArgumentNullException(nameof(configure));

            FlurlClient.WithSettings(flurlSettings =>
            {
                CommonClientSettings settings = new CommonClientSettings(flurlSettings);
                configure.Invoke(settings);

                flurlSettings.Timeout = settings.Timeout;
                flurlSettings.HttpVersion = settings.HttpVersion.ToString();
                flurlSettings.JsonSerializer = new InternalWrappedJsonSerializer(settings.JsonSerializer);
                flurlSettings.UrlEncodedSerializer = new InternalWrappedFormUrlEncodedSerializer(settings.FormUrlEncodedSerializer);
                flurlSettings.HttpClientFactory = settings.FlurlHttpClientFactory;
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
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (FlurlHttpException ex)
            {
                if (ex is FlurlParsingException)
                    throw new CommonSerializationException(ex.Message, ex);
                if (ex is FlurlHttpTimeoutException)
                    throw new CommonTimeoutException(ex.Message, ex);

                throw new CommonHttpException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                if (ex is CommonException)
                    throw;

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
        protected virtual async Task<IFlurlResponse> SendFlurlRequestAsJsonAsync(IFlurlRequest flurlRequest, object? data = null, CancellationToken cancellationToken = default)
        {
            if (flurlRequest == null) throw new ArgumentNullException(nameof(flurlRequest));
            if (_disposed) throw new ObjectDisposedException(nameof(FlurlClient));

            HttpContent? httpContent = null;
            if (data != null)
            {
                try
                {
                    string content = JsonSerializer.Serialize(data, data.GetType());
                    httpContent = new StringContent(content, encoding: null, mediaType: MimeTypes.Json);
                }
                catch (Exception ex)
                {
                    throw new CommonSerializationException(ex.Message, ex);
                }
            }

            try
            {
                return await SendFlurlRequestAsync(flurlRequest, httpContent, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (FlurlHttpException ex)
            {
                if (ex is FlurlParsingException)
                    throw new CommonSerializationException(ex.Message, ex);
                if (ex is FlurlHttpTimeoutException)
                    throw new CommonTimeoutException(ex.Message, ex);

                throw new CommonHttpException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                if (ex is CommonException)
                    throw;

                throw new CommonException(ex.Message, ex);
            }
            finally
            {
                httpContent?.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flurlRequest"></param>
        /// <param name="data"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected virtual async Task<IFlurlResponse> SendFlurlRequestAsFormUrlEncodedAsync(IFlurlRequest flurlRequest, object? data = null, CancellationToken cancellationToken = default)
        {
            if (flurlRequest == null) throw new ArgumentNullException(nameof(flurlRequest));
            if (_disposed) throw new ObjectDisposedException(nameof(FlurlClient));

            HttpContent? httpContent = null;
            if (data != null)
            {
                try
                {
                    string content = FormUrlEncodedSerializer.Serialize(data, data.GetType());
                    httpContent = new StringContent(content, encoding: null, mediaType: MimeTypes.FormUrlEncoded);
                }
                catch (Exception ex)
                {
                    throw new CommonSerializationException(ex.Message, ex);
                }
            }

            try
            {
                return await SendFlurlRequestAsync(flurlRequest, httpContent, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (FlurlHttpException ex)
            {
                if (ex is FlurlParsingException)
                    throw new CommonSerializationException(ex.Message, ex);
                if (ex is FlurlHttpTimeoutException)
                    throw new CommonTimeoutException(ex.Message, ex);

                throw new CommonHttpException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                if (ex is CommonException)
                    throw;

                throw new CommonException(ex.Message, ex);
            }
            finally
            {
                httpContent?.Dispose();
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
            byte tb1 = default(byte),
                 tb2 = default(byte);
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
                    string? contentType = flurlResponse.Headers.GetAll(HttpHeaders.ContentType).FirstOrDefault();
                    string? charset = MediaTypeHeaderValue.TryParse(contentType, out MediaTypeHeaderValue? mediaType) ? mediaType.CharSet : null;
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

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
