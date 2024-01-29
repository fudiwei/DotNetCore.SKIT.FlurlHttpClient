using System;

namespace Flurl.Http
{
    public static class FlurlRequestUrlExtensions
    {
        public static IFlurlRequest WithUrl(this IFlurlRequest request, string url)
        {
            return WithUrl(request, Url.Parse(url));
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
            if (request is null) throw new ArgumentNullException(nameof(request));
            if (configure is null) throw new ArgumentNullException(nameof(configure));

            request.Url = configure(request.Url);
            return request;
        }
    }
}
