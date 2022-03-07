using System;
using HtmlAgilityPack;

namespace SKIT.FlurlHttpClient.Tools.DocsTracker.Trackers
{
    public class WeixinMiniProgramPlatformTracker : WeixinMediaPlatformTracker
    {
        public WeixinMiniProgramPlatformTracker(DocsTrackerOptions options)
            : base(options)
        {
        }

        protected override Uri GetDocumentationEntrypointUri()
        {
            return new Uri("https://developers.weixin.qq.com/miniprogram/dev/platform-capabilities/");
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
