using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace SKIT.FlurlHttpClient.Tools.DocsTracker
{
    public abstract class DocsTracker
    {
        private readonly string _outputPath;

        public DocsTracker(DocsTrackerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            _outputPath = options.OutputPath;
        }

        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            using HttpClient httpClient = new HttpClient();
            using HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, GetDocumentationUri());
            using HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken);
            httpResponse.EnsureSuccessStatusCode();

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.Load(httpResponse.Content.ReadAsStream(), Encoding.UTF8);
        }

        protected abstract Uri GetDocumentationUri();

        protected abstract 
    }
}
