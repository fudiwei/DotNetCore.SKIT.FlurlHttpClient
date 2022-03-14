using System;

namespace Flurl.Http
{
    public static class FlurlHttpRequestUrlExtensions
    {
        public static IFlurlRequest WithUrl(this IFlurlRequest request, string baseUrl)
        {
            return WithUrl(request, new Url(baseUrl));
        }

        public static IFlurlRequest WithUrl(this IFlurlRequest request, Uri uri)
        {
            return WithUrl(request, new Url(uri));
        }

        public static IFlurlRequest WithUrl(this IFlurlRequest request, Url url)
        {
            return WithUrl(request, (_) => url);
        }

        public static IFlurlRequest WithUrl(this IFlurlRequest request, Func<Url, Url> configure)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (configure == null) throw new ArgumentNullException(nameof(configure));

            request.Url = configure(request.Url);
            return request;
        }
    }
}
