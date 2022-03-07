namespace SKIT.FlurlHttpClient.Tools.DocsTracker.Models
{
    public class Content
    {
        public string Title { get; }

        public string Uri { get; }

        public string InnerHtml { get; }

        public Content(string title, string uri, string innerHtml)
        {
            Title = title;
            Uri = uri;
            InnerHtml = innerHtml;
        }
    }
}
