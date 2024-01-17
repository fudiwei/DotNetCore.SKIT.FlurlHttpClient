using System;
using System.Collections.Generic;
using System.Net.Http;
using Flurl.Http;

namespace SKIT.FlurlHttpClient
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

    internal static class HttpInterceptorContextFlurlCallExtensions
    {
        private const string OPTION_KEY = "__INTERNAL__SKIT.FlurlHttpClient.HttpInterceptorContext";

        public static HttpInterceptorContext GetHttpInterceptorContext(this FlurlCall call)
        {
            HttpInterceptorContext? context;

#if NET5_0_OR_GREATER
            if (!call.HttpRequestMessage.Options.TryGetValue(new HttpRequestOptionsKey<HttpInterceptorContext>(OPTION_KEY), out context) || context is null)
            {
                context = new HttpInterceptorContext(call);
                SetHttpInterceptorContext(call, context);
            }
#else
            if (!call.HttpRequestMessage.Properties.TryGetValue(OPTION_KEY, out object? obj) || obj as HttpInterceptorContext is null)
            {
                context = new HttpInterceptorContext(call);
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
            if (context == null) throw new ArgumentNullException(nameof(context));

#if NET5_0_OR_GREATER
            call.HttpRequestMessage.Options.Set(new HttpRequestOptionsKey<HttpInterceptorContext>(OPTION_KEY), context);
#else
            call.HttpRequestMessage.Properties[OPTION_KEY] = context;
#endif
        }
    }
}
