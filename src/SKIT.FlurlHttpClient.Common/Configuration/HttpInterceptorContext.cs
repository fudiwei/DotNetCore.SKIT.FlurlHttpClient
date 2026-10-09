namespace SKIT.FlurlHttpClient
{
    /// <summary>
    /// SKIT.FlurlHttpClient HTTP 拦截器上下文。
    /// </summary>
    public sealed class HttpInterceptorContext
    {
        private readonly ICommonClient _originalClient;

        /// <summary>
        /// 获取此调用的对象。
        /// </summary>
        public FlurlCall FlurlCall { get; }

        /// <summary>
        /// 获取此调用的请求对象。
        /// </summary>
        public IFlurlRequest FlurlRequest { get { return FlurlCall.Request; } }

        /// <summary>
        /// 获取此调用的响应对象。
        /// </summary>
        public IFlurlResponse? FlurlResponse { get { return FlurlCall.Response; } }

        /// <summary>
        /// 获取可用于在此调用范围内共享数据的键值集合。
        /// </summary>
        public IDictionary<object, object?> Items { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="context"></param>
        internal HttpInterceptorContext(ICommonClient client, HttpInterceptorContext context)
            : this(client, context.FlurlCall, context.Items)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="flurlCall"></param>
        internal HttpInterceptorContext(ICommonClient client, FlurlCall flurlCall)
            : this(client, flurlCall, new Dictionary<object, object?>(capacity: 4))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="flurlCall"></param>
        /// <param name="items"></param>
        internal HttpInterceptorContext(ICommonClient client, FlurlCall flurlCall, IDictionary<object, object?> items)
        {
            _originalClient = client ?? throw new ArgumentNullException(nameof(client));
            FlurlCall = flurlCall ?? throw new ArgumentNullException(nameof(flurlCall));
            Items = items ?? throw new ArgumentNullException(nameof(items));
        }

        /// <summary>
        /// 获取拦截器上下文所属的 <see cref="ICommonClient"/> 对象。
        /// </summary>
        /// <returns></returns>
        public ICommonClient GetOriginalClient()
        {
            return _originalClient;
        }

        /// <summary>
        /// 获取拦截器上下文所属的 <see cref="ICommonRequest"/> 对象。
        /// </summary>
        /// <returns></returns>
        public ICommonRequest GetOriginalRequest()
        {
            if (FlurlRequest is InternalWrappedFlurlRequest wrappedFlurlRequest)
            {
                return wrappedFlurlRequest.OriginalRequest;
            }

            throw new InvalidOperationException("The FlurlRequest object is not wrapped. May be you are not using SKIT.FlurlHttpClient.");
        }
    }

    internal static class HttpInterceptorContextFlurlCallExtensions
    {
        private const string OPTION_KEY = "__INTERNAL__SKIT.FlurlHttpClient.HttpInterceptorContext";

        public static HttpInterceptorContext GetHttpInterceptorContextWithClient(this FlurlCall call, ICommonClient client)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));

            HttpInterceptorContext? context;
#if NET5_0_OR_GREATER
            if (!call.HttpRequestMessage.Options.TryGetValue(new HttpRequestOptionsKey<HttpInterceptorContext>(OPTION_KEY), out context) || context is null)
            {
                context = new HttpInterceptorContext(client, call);
                SetHttpInterceptorContext(call, context);
            }
#else
            if (!call.HttpRequestMessage.Properties.TryGetValue(OPTION_KEY, out object? obj) || obj as HttpInterceptorContext is null)
            {
                context = new HttpInterceptorContext(client, call);
                SetHttpInterceptorContext(call, context);
            }
            else
            {
                context = (HttpInterceptorContext)obj;
            }
#endif

            return context!;
        }

        public static void SetHttpInterceptorContext(this FlurlCall call, HttpInterceptorContext context)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));

#if NET5_0_OR_GREATER
            call.HttpRequestMessage.Options.Set(new HttpRequestOptionsKey<HttpInterceptorContext>(OPTION_KEY), context);
#else
            call.HttpRequestMessage.Properties[OPTION_KEY] = context;
#endif
        }
    }
}
