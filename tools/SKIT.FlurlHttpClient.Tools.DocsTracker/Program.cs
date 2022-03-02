using System;
using System.Threading.Tasks;

namespace SKIT.FlurlHttpClient.Tools.DocsTracker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //new Trackers.WeixinMiniProgramBackendTracker(new DocsTrackerOptions() { OutputPath = "./Logs/Weixin.MiniProgram.Backend/" }).RunAsync().Wait();
            //new Trackers.WeixinMiniProgramPlatformTracker(new DocsTrackerOptions() { OutputPath = "./Logs/Weixin.MiniProgram.Platform/" }).RunAsync().Wait();
            //new Trackers.WeixinPayCommonTracker(new DocsTrackerOptions() { OutputPath = "./Logs/Weixin.Pay.Common/" }).RunAsync().Wait();
            new Trackers.WeixinPayPartnerTracker(new DocsTrackerOptions() { OutputPath = "./Logs/Weixin.Pay.Partner/" }).RunAsync().Wait();
        }
    }
}