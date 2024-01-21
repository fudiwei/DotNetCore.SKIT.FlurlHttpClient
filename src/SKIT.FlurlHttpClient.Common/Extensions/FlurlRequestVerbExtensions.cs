using System;
using System.Net.Http;

namespace Flurl.Http
{
    public static class FlurlRequestVerbExtensions
    {
        public static IFlurlRequest WithVerb(this string url, string method)
        {
            return WithVerb(url, new HttpMethod(method));
        }

        public static IFlurlRequest WithVerb(this string url, HttpMethod method)
        {
            return WithVerb(Url.Parse(url), method);
        }

        public static IFlurlRequest WithVerb(this Url url, string method)
        {
            return WithVerb(url, new HttpMethod(method));
        }

        public static IFlurlRequest WithVerb(this Url url, HttpMethod method)
        {
            return WithVerb(new FlurlRequest(url), method);
        }

        public static IFlurlRequest WithVerb(this IFlurlRequest request, string method)
        {
            return WithVerb(request, new HttpMethod(method));
        }

        public static IFlurlRequest WithVerb(this IFlurlRequest request, HttpMethod method)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            request.Verb = method;
            return request;
        }
    }
}
