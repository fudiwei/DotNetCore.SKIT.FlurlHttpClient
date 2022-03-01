using System;

namespace SKIT.FlurlHttpClient.Tools.DocsTracker
{
    public class WeixinDeveloperMiniProgramTracker : DocsTracker
    {
        public WeixinDeveloperMiniProgramTracker(DocsTrackerOptions options)
            : base(options)
        {
        }

        protected override Uri GetDocumentationCatalogUri()
        {
            return new Uri("https://developers.weixin.qq.com/miniprogram/dev/api-backend/");
        }
    }
}
