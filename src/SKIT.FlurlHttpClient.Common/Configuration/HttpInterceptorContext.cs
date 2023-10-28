using System.Collections.Generic;
using Flurl.Http;

namespace SKIT.FlurlHttpClient.Configuration
{
    /// <summary>
    /// SKIT.FlurlHttpClient HTTP 拦截器上下文。
    /// </summary>
    public sealed class HttpInterceptorContext
    {
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

        internal HttpInterceptorContext(HttpInterceptorContext context)
            : this(context.FlurlCall, context.Items)
        {
        }

        internal HttpInterceptorContext(FlurlCall flurlCall)
            : this(flurlCall, new Dictionary<object, object?>())
        {
        }

        internal HttpInterceptorContext(FlurlCall flurlCall, IDictionary<object, object?> items)
        {
            FlurlCall = flurlCall;
            Items = items;
        }
    }
}
