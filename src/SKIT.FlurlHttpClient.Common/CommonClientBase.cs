using System;
using System.Collections.ObjectModel;
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
    /// <summary>
    /// SKIT.FlurlHttpClient 客户端基类。
    /// </summary>
    public abstract class CommonClientBase : ICommonClient
    {
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public FlurlHttpCallInterceptorCollection Interceptors { get; }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public ISerializer JsonSerializer
        {
            get { return FlurlClient.Settings?.JsonSerializer ?? FlurlHttp.GlobalSettings.JsonSerializer; }
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
            Interceptors = new FlurlHttpCallInterceptorCollection();
            FlurlClient = new FlurlClient();
            FlurlClient.Configure(flurlSettings =>
            {
                flurlSettings.JsonSerializer = new FlurlSystemTextJsonSerializer();
                flurlSettings.BeforeCallAsync = async (flurlCall) =>
                {
                    for (int i = 0, len = Interceptors.Count; i < len; i++)
                    {
                        await Interceptors[i].BeforeCallAsync(flurlCall);
                    }
                };
                flurlSettings.AfterCallAsync = async (flurlCall) =>
                {
                    for (int i = Interceptors.Count - 1; i >= 0; i--)
                    {
                        await Interceptors[i].AfterCallAsync(flurlCall);
                    }
                };
            });
        }

        /// <inheritdoc/>
        public void Configure(Action<CommonClientSettings> configure)
        {
            if (configure == null) throw new ArgumentNullException(nameof(configure));

            FlurlClient.Configure(flurlClientSettings =>
            {
                CommonClientSettings settings = new CommonClientSettings(flurlClientSettings);
                configure.Invoke(settings);

                flurlClientSettings.Timeout = settings.ConnectionRequestTimeout;
                flurlClientSettings.ConnectionLeaseTimeout = settings.ConnectionLeaseTimeout;
                flurlClientSettings.JsonSerializer = settings.JsonSerializer;
                flurlClientSettings.UrlEncodedSerializer = settings.UrlEncodedSerializer;
                flurlClientSettings.HttpClientFactory = settings.FlurlHttpClientFactory;
            });
        }

        private IFlurlRequest WrapRequest(IFlurlRequest flurlRequest)
        {
            return flurlRequest
                .WithClient(FlurlClient)
                .AllowAnyHttpStatus();
        }

        /// <summary>
        /// 异步发起请求。
        /// </summary>
        /// <param name="flurlRequest"></param>
        /// <param name="httpContent"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected virtual async Task<IFlurlResponse> SendRequestAsync(IFlurlRequest flurlRequest, HttpContent? httpContent = null, CancellationToken cancellationToken = default)
        {
            if (flurlRequest == null) throw new ArgumentNullException(nameof(flurlRequest));

            return await WrapRequest(flurlRequest).SendAsync(flurlRequest.Verb, httpContent, cancellationToken);
        }

        /// <summary>
        /// 异步发起请求。
        /// <para>指定请求标头 `Content-Type` 为 `application/json`。</para>
        /// </summary>
        /// <param name="flurlRequest"></param>
        /// <param name="data"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected virtual async Task<IFlurlResponse> SendRequestWithJsonAsync(IFlurlRequest flurlRequest, object? data = null, CancellationToken cancellationToken = default)
        {
            if (flurlRequest == null) throw new ArgumentNullException(nameof(flurlRequest));

            if (data != null)
            {
                if (!flurlRequest.Headers.GetAll(Contants.HttpHeaders.ContentType).Any())
                {
                    flurlRequest.WithHeader(Contants.HttpHeaders.ContentType, "application/json");
                }
            }

            return await WrapRequest(flurlRequest).SendJsonAsync(flurlRequest.Verb, data: data, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="flurlResponse"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected async Task<TResponse> WrapResponseAsync<TResponse>(IFlurlResponse flurlResponse, CancellationToken cancellationToken = default)
            where TResponse : ICommonResponse, new()
        {
            Task<byte[]> taskReadBytes = flurlResponse.GetBytesAsync();
            Task taskCancellationToken = Task.Run(async () => { while (!cancellationToken.IsCancellationRequested && !taskReadBytes.IsCompleted) await Task.Yield(); });
            
            await Task.WhenAny(taskReadBytes, taskCancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            TResponse result = new TResponse();
            result.RawBytes = await taskReadBytes;
            result.RawStatus = flurlResponse.StatusCode;
            result.RawHeaders = new ReadOnlyDictionary<string, string>(
                flurlResponse.Headers
                    .GroupBy(e => e.Name)
                    .ToDictionary(
                        k => k.Key,
                        v => string.Join(", ", v.Select(e => e.Value))
                    )
            );
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="flurlResponse"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected async Task<TResponse> WrapResponseWithJsonAsync<TResponse>(IFlurlResponse flurlResponse, CancellationToken cancellationToken = default)
            where TResponse : ICommonResponse, new()
        {
            TResponse tmp = await WrapResponseAsync<TResponse>(flurlResponse, cancellationToken);
            byte tmpb1 = tmp.RawBytes.SkipWhile(b => b <= 32).FirstOrDefault(),
                 tmpb2 = tmp.RawBytes.Reverse().SkipWhile(b => b <= 32).FirstOrDefault();
            bool jsonable = (tmpb1 == 91 && tmpb2 == 93) || (tmpb1 == 123 && tmpb2 == 125); // "[...]" or "{...}"

            TResponse result;
            if (jsonable)
            {
                string? contentType = flurlResponse.Headers.GetAll(Contants.HttpHeaders.ContentType).FirstOrDefault();
                string? charset = MediaTypeHeaderValue.TryParse(contentType, out var mediaType) ? mediaType.CharSet : null;
                string json = (string.IsNullOrEmpty(charset) ? Encoding.UTF8 : Encoding.GetEncoding(charset)).GetString(tmp.RawBytes);

                result = JsonSerializer.Deserialize<TResponse>(json);
                result.RawStatus = tmp.RawStatus;
                result.RawHeaders = tmp.RawHeaders;
                result.RawBytes = tmp.RawBytes;
            }
            else
            {
                result = tmp;
            }

            return result;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public virtual void Dispose()
        {
            FlurlClient?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
