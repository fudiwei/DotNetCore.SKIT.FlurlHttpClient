using System;
using HtmlAgilityPack;

namespace SKIT.FlurlHttpClient.Tools.DocsTracker.Trackers
{
    public class WeixinWorkHardwareTracker : WeixinWorkEnterpriseTracker
    {
        public WeixinWorkHardwareTracker(DocsTrackerOptions options)
            : base(options)
        {
        }

        protected override Uri GetDocumentationEntrypointUri()
        {
            return new Uri("https://developer.work.weixin.qq.com/document/path/90647");
        }

        protected override Models.Catalog ParseDocumentationCatalog(HtmlDocument htmlDocument)
        {
            return base.ParseDocumentationCatalog(htmlDocument);
        }

        protected override Models.Content ParseDocumentationContent(Models.Catalog.Types.Section section, HtmlDocument htmlDocument)
        {
            return base.ParseDocumentationContent(section, htmlDocument);
        }
    }
}
