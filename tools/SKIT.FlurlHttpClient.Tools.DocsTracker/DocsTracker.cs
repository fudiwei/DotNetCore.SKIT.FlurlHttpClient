using System;
using System.IO;
using System.Linq;
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

            _outputPath = Path.Combine(options.OutputPath, DateTimeOffset.Now.ToString("yyyyMMdd"));
        }

        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            Uri httpUri = GetDocumentationUri();
            using HttpClient httpClient = new HttpClient() { BaseAddress = new Uri($"{httpUri.Scheme}://{httpUri.Host}") };

            using HttpRequestMessage catalogHttpRequest = new HttpRequestMessage(HttpMethod.Get, httpUri);
            using HttpResponseMessage catalogHttpResponse = await httpClient.SendAsync(catalogHttpRequest, cancellationToken);
            catalogHttpResponse.EnsureSuccessStatusCode();

            HtmlDocument catalogHtmlDocument = new HtmlDocument();
            catalogHtmlDocument.Load(catalogHttpResponse.Content.ReadAsStream(), Encoding.UTF8);

            Models.Catalog catalogModel = ParseDocumentationCatalog(catalogHtmlDocument);
            await SaveAllTextAsync("catalog.html", catalogModel.InnerHtml, cancellationToken);

            await Parallel.ForEachAsync(catalogModel.Sections, async (sectionModel, cancellationToken) =>
            {
                using HttpRequestMessage contentHttpRequest = new HttpRequestMessage(HttpMethod.Get, sectionModel.Uri);
                using HttpResponseMessage contentHttpResponse = await httpClient.SendAsync(contentHttpRequest, cancellationToken);
                contentHttpResponse.EnsureSuccessStatusCode();

                Models.Content contentModel = ParseDocumentationContent(sectionModel, catalogHtmlDocument);
                await SaveAllTextAsync($"content-{sectionModel.Title}.html", contentModel.InnerHtml, cancellationToken);
            });
        }

        protected abstract Uri GetDocumentationUri();

        protected abstract Models.Catalog ParseDocumentationCatalog(HtmlDocument htmlDocument);

        protected abstract Models.Content ParseDocumentationContent(Models.Catalog.Types.Section section, HtmlDocument htmlDocument);

        private async Task SaveAllTextAsync(string fileName, string text, CancellationToken cancellationToken)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
                fileName = fileName.Replace(c.ToString(), "");

            string filePath = Path.Combine(_outputPath, fileName);
            string fileDir = Path.GetDirectoryName(filePath)!;

            try
            {
                if (!Directory.Exists(fileDir))
                    Directory.CreateDirectory(fileDir);
            }
            catch (NotSupportedException) { }

            await File.WriteAllTextAsync(path: filePath, contents: text, cancellationToken);
        }
    }
}
