using System;

namespace SKIT.FlurlHttpClient.Tools.DocsTracker.Trackers
{
    public class WeixinMiniProgramTracker : DocsTracker
    {
        public WeixinMiniProgramTracker(DocsTrackerOptions options)
            : base(options)
        {
        }

        protected override Uri GetDocumentationUri()
        {
            return new Uri("https://developers.weixin.qq.com/miniprogram/dev/api-backend/");
        }
    }
}
