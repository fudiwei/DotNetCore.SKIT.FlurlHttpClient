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
                new Trackers.WeixinMediaPlatformTracker(new DocsTrackerOptions() { OutputPath = $"{WORD_DIR}/Weixin.MediaPlatform" }).RunAsync(),
                new Trackers.WeixinMiniProgramBackendTracker(new DocsTrackerOptions() { OutputPath = $"{WORD_DIR}/Weixin.MiniProgram.Backend" }).RunAsync(),
                new Trackers.WeixinMiniProgramPlatformTracker(new DocsTrackerOptions() { OutputPath = $"{WORD_DIR}/Weixin.MiniProgram.Platform" }).RunAsync(),
                new Trackers.WeixinMiniGameBackendTracker(new DocsTrackerOptions() { OutputPath = $"{WORD_DIR}/Weixin.MiniGame.Platform" }).RunAsync(),
                new Trackers.WeixinOpenPlatformTracker(new DocsTrackerOptions() { OutputPath = $"{WORD_DIR}/Weixin.OpenPlatform" }).RunAsync(),
                new Trackers.WeixinPayCommonTracker(new DocsTrackerOptions() { OutputPath = $"{WORD_DIR}/Weixin.Pay.Common" }).RunAsync(),
                new Trackers.WeixinPayPartnerTracker(new DocsTrackerOptions() { OutputPath = $"{WORD_DIR}/Weixin.Pay.Partner" }).RunAsync(),
                new Trackers.WeixinWorkEnterpriseTracker(new DocsTrackerOptions() { OutputPath = $"{WORD_DIR}/Weixin.Work.Enterprise" }).RunAsync(),
                new Trackers.WeixinWorkPartnerTracker(new DocsTrackerOptions() { OutputPath = $"{WORD_DIR}/Weixin.Work.Partner" }).RunAsync(),
                new Trackers.WeixinWorkHardwareTracker(new DocsTrackerOptions() { OutputPath = $"{WORD_DIR}/Weixin.Work.Hardware" }).RunAsync()
            );
        }
    }
}