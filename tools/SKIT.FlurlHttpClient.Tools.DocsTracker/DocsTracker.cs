using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;

namespace SKIT.FlurlHttpClient.Tools.DocsTracker
{
    public abstract class DocsTracker
    {
        private readonly string _outputPath;

        public DocsTracker(DocsTrackerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            _outputPath = Path.Combine(options.OutputPath, DateTimeOffset.Now.ToString("yyyyMMdd"));
        }

        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            IList<Exception> errors = new List<Exception>();

            Uri httpUri = GetDocumentationUri();
            using HttpClient httpClient = new HttpClient() { BaseAddress = new Uri($"{httpUri.Scheme}://{httpUri.Host}") };

            using HttpRequestMessage catalogHttpRequest = new HttpRequestMessage(HttpMethod.Get, httpUri);
            using HttpResponseMessage catalogHttpResponse = await httpClient.SendAsync(catalogHttpRequest, cancellationToken);
            catalogHttpResponse.EnsureSuccessStatusCode();

            HtmlDocument catalogHtmlDocument = new HtmlDocument();
            catalogHtmlDocument.Load(catalogHttpResponse.Content.ReadAsStream(), Encoding.UTF8);

            Models.Catalog catalogModel = ParseDocumentationCatalog(catalogHtmlDocument);
            await Helpers.FileHelper.WriteTextAsync(_outputPath, "catalog.html", catalogModel.InnerHtml, cancellationToken);
            
            await Parallel.ForEachAsync(catalogModel.Sections, async (sectionModel, cancellationToken) =>
            {
                try
                {
                    using HttpRequestMessage contentHttpRequest = new HttpRequestMessage(HttpMethod.Get, HttpUtility.HtmlDecode(sectionModel.Uri));
                    using HttpResponseMessage contentHttpResponse = await httpClient.SendAsync(contentHttpRequest, cancellationToken);
                    contentHttpResponse.EnsureSuccessStatusCode();

                    HtmlDocument sectionHtmlDocument = new HtmlDocument();
                    sectionHtmlDocument.Load(contentHttpResponse.Content.ReadAsStream(), Encoding.UTF8);

                    Models.Content contentModel = ParseDocumentationContent(sectionModel, sectionHtmlDocument);
                    await Helpers.FileHelper.WriteTextAsync(_outputPath, $"content-{sectionModel.Title}.html", contentModel.InnerHtml, cancellationToken);
                }
                catch (Exception ex)
                {
                    errors.Add(ex);
                }
            });

            if (errors.Any())
            {
                throw new AggregateException(errors);
            }
        }

        protected abstract Uri GetDocumentationUri();

        protected abstract Models.Catalog ParseDocumentationCatalog(HtmlDocument htmlDocument);

        protected abstract Models.Content ParseDocumentationContent(Models.Catalog.Types.Section section, HtmlDocument htmlDocument);
    }
}
