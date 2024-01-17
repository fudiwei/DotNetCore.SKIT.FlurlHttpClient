using System;
using System.Net.Http;
using Flurl.Http;

namespace SKIT.FlurlHttpClient
{
    using SKIT.FlurlHttpClient.Configuration;

    internal static class FlurlCallInterceptorExtensions
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
