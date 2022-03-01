using System.Threading.Tasks;

namespace SKIT.FlurlHttpClient.Tools.DocsTracker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            new Trackers.WeixinMiniProgramTracker(new DocsTrackerOptions() { OutputPath = "./Logs/Weixin/MiniProgram/" }).RunAsync().Wait();
        }
    }
}