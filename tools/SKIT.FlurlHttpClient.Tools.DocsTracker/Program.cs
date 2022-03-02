using System;
using System.Threading.Tasks;

namespace SKIT.FlurlHttpClient.Tools.DocsTracker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string WORD_DIR = "./Logs";
            Task.WaitAll(
                new Trackers.WeixinMiniProgramBackendTracker(new DocsTrackerOptions() { OutputPath = $"{WORD_DIR}/Weixin.MiniProgram.Backend" }).RunAsync(),
                new Trackers.WeixinMiniProgramPlatformTracker(new DocsTrackerOptions() { OutputPath = $"{WORD_DIR}/Weixin.MiniProgram.Platform" }).RunAsync(),
                new Trackers.WeixinPayCommonTracker(new DocsTrackerOptions() { OutputPath = $"{WORD_DIR}/Weixin.Pay.Common" }).RunAsync(),
                new Trackers.WeixinPayPartnerTracker(new DocsTrackerOptions() { OutputPath = $"{WORD_DIR}/Weixin.Pay.Partner" }).RunAsync()
            );
        }
    }
}