using System;
using System.Threading.Tasks;

namespace SKIT.FlurlHttpClient.Tools.DocsTracker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //new Trackers.WeixinMiniProgramBackendTracker(new DocsTrackerOptions() { OutputPath = "./Logs/Weixin.MiniProgramBackend/" }).RunAsync().Wait();
            new Trackers.WeixinMiniProgramPlatformTracker(new DocsTrackerOptions() { OutputPath = "./Logs/Weixin.MiniProgramPlatform/" }).RunAsync().Wait();
        }
    }
}