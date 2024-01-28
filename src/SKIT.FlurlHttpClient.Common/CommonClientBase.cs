using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace SKIT.FlurlHttpClient
{
    using SKIT.FlurlHttpClient.Internal;

    /// <summary>
    /// SKIT.FlurlHttpClient 通用客户端基类。
    /// </summary>
    public abstract class CommonClientBase : ICommonClient
    {
        private volatile bool _disposed;
        private readonly bool _disposeClient;

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
        /// <param name="httpClient"></param>
        /// <param name="disposeClient"></param>
        protected CommonClientBase(HttpClient? httpClient = null, bool disposeClient = true)
        {
            _disposeClient = disposeClient;

            Interceptors = new HttpInterceptorCollection();
            FlurlClient = httpClient is null ? new FlurlClient() : new FlurlClient(httpClient);
            FlurlClient.WithSettings(flurlSettings =>
            {
                IJsonSerializer jsonSerializer = new SystemTextJsonSerializer();
                IFormUrlEncodedSerializer formUrlEncodedSerializer = new JsonifiedFormUrlEncodedSerializer(jsonSerializer);
                flurlSettings.JsonSerializer = new InternalWrappedJsonSerializer(jsonSerializer);
                flurlSettings.UrlEncodedSerializer = new InternalWrappedFormUrlEncodedSerializer(formUrlEncodedSerializer);

                FlurlClient.BeforeCall(async flurlCall =>
                {
                    using CancellationTokenSource cts = new CancellationTokenSource();
                    if (flurlSettings.Timeout.HasValue)
                        cts.CancelAfter(flurlSettings.Timeout.Value);
                    if (flurlCall.Request.Settings.Timeout.HasValue)
                        cts.CancelAfter(flurlCall.Request.Settings.Timeout.Value);

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
                });

                FlurlClient.AfterCall(async flurlCall =>
                {
                    using CancellationTokenSource cts = new CancellationTokenSource();
                    if (flurlSettings.Timeout.HasValue)
                        cts.CancelAfter(flurlSettings.Timeout.Value);
                    if (flurlCall.Request.Settings.Timeout.HasValue)
                        cts.CancelAfter(flurlCall.Request.Settings.Timeout.Value);

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
                });
            });
        }

        /// <inheritdoc/>
        public void Configure(Action<CommonClientSettings> configure)
        {
            if (configure is null) throw new ArgumentNullException(nameof(configure));
            if (_disposed) throw new ObjectDisposedException(nameof(FlurlClient));

            FlurlClient.WithSettings(flurlSettings =>
            {
                CommonClientSettings settings = new CommonClientSettings(flurlSettings);
                configure.Invoke(settings);

                flurlSettings.Timeout = settings.Timeout;
                flurlSettings.HttpVersion = settings.HttpVersion.ToString();
                flurlSettings.JsonSerializer = new InternalWrappedJsonSerializer(settings.JsonSerializer);
                flurlSettings.UrlEncodedSerializer = new InternalWrappedFormUrlEncodedSerializer(settings.FormUrlEncodedSerializer);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual IFlurlRequest CreateFlurlRequest(CommonRequestBase request, HttpMethod httpMethod, params object[] urlSegments)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));
            if (_disposed) throw new ObjectDisposedException(nameof(FlurlClient));

            IFlurlRequest flurlRequest = FlurlClient.Request(urlSegments).WithVerb(httpMethod);

            if (request._InternalTimeout is not null)
            {
                flurlRequest.WithTimeout(request._InternalTimeout.Value);
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
            if (flurlRequest is null) throw new ArgumentNullException(nameof(flurlRequest));
            if (_disposed) throw new ObjectDisposedException(nameof(FlurlClient));

            try
            {
                flurlRequest.Client = FlurlClient;

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
            if (flurlRequest is null) throw new ArgumentNullException(nameof(flurlRequest));
            if (_disposed) throw new ObjectDisposedException(nameof(FlurlClient));

            HttpContent? httpContent = null;
            if (data is not null)
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
            if (flurlRequest is null) throw new ArgumentNullException(nameof(flurlRequest));
            if (_disposed) throw new ObjectDisposedException(nameof(FlurlClient));

            HttpContent? httpContent = null;
            if (data is not null)
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
            if (flurlResponse is null) throw new ArgumentNullException(nameof(flurlResponse));

            TResponse result = new TResponse();
            result._InternalRawStatus = flurlResponse.StatusCode;
            result._InternalRawHeaders = new HttpHeaderCollection(flurlResponse.Headers);
            result._InternalRawBytes = await _AsyncEx.RunTaskWithCancellationTokenAsync(flurlResponse.GetBytesAsync(), cancellationToken);
            return result;
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
            if (flurlResponse is null) throw new ArgumentNullException(nameof(flurlResponse));

            TResponse result;

            TResponse tmp = await WrapFlurlResponseAsync<TResponse>(flurlResponse, cancellationToken);
            if (_StringSyntaxAssert.MaybeJson(tmp._InternalRawBytes))
            {
                try
                {
                    string? contentType = flurlResponse.Headers.GetAll(HttpHeaders.ContentType).FirstOrDefault();
                    string? charset = MediaTypeHeaderValue.TryParse(contentType, out MediaTypeHeaderValue? mediaType) ? mediaType.CharSet : null;
                    string json = (string.IsNullOrEmpty(charset) ? Encoding.UTF8 : Encoding.GetEncoding(charset)).GetString(tmp._InternalRawBytes);

                    result = JsonSerializer.Deserialize<TResponse>(json);
                    result._InternalRawStatus = tmp._InternalRawStatus;
                    result._InternalRawHeaders = tmp._InternalRawHeaders;
                    result._InternalRawBytes = tmp._InternalRawBytes;
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
                    if (_disposeClient)
                    {
                        FlurlClient.Dispose();
                    }
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
