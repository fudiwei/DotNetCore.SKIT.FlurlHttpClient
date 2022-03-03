using System;
using System.Threading.Tasks;
using Utility.CommandLine;

namespace SKIT.FlurlHttpClient.Tools.DocsTracker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Arguments arguments = Arguments.Parse();
            string ouputDir = (arguments["output-dir"]?.ToString()?.TrimEnd('/', '\\')) ?? "./Logs";

            Task.WaitAll(
                new Trackers.WeixinMediaPlatformTracker(new DocsTrackerOptions() { OutputPath = $"{ouputDir}/Weixin.MediaPlatform" }).RunAsync(),
                new Trackers.WeixinMiniProgramBackendTracker(new DocsTrackerOptions() { OutputPath = $"{ouputDir}/Weixin.MiniProgram.Backend" }).RunAsync(),
                new Trackers.WeixinMiniProgramPlatformTracker(new DocsTrackerOptions() { OutputPath = $"{ouputDir}/Weixin.MiniProgram.Platform" }).RunAsync(),
                new Trackers.WeixinMiniGameBackendTracker(new DocsTrackerOptions() { OutputPath = $"{ouputDir}/Weixin.MiniGame.Platform" }).RunAsync(),
                new Trackers.WeixinOpenPlatformTracker(new DocsTrackerOptions() { OutputPath = $"{ouputDir}/Weixin.OpenPlatform" }).RunAsync(),
                new Trackers.WeixinPayCommonTracker(new DocsTrackerOptions() { OutputPath = $"{ouputDir}/Weixin.Pay.Common" }).RunAsync(),
                new Trackers.WeixinPayPartnerTracker(new DocsTrackerOptions() { OutputPath = $"{ouputDir}/Weixin.Pay.Partner" }).RunAsync(),
                new Trackers.WeixinWorkEnterpriseTracker(new DocsTrackerOptions() { OutputPath = $"{ouputDir}/Weixin.Work.Enterprise" }).RunAsync(),
                new Trackers.WeixinWorkPartnerTracker(new DocsTrackerOptions() { OutputPath = $"{ouputDir}/Weixin.Work.Partner" }).RunAsync(),
                new Trackers.WeixinWorkHardwareTracker(new DocsTrackerOptions() { OutputPath = $"{ouputDir}/Weixin.Work.Hardware" }).RunAsync()
            );
        }
    }
}