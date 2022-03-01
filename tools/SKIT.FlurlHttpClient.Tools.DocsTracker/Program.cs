using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SKIT.FlurlHttpClient.Tools.DocsTracker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            CancellationToken cancellationToken = default;
            using HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://developers.weixin.qq.com/miniprogram/dev/api-backend/");

            string html = await httpClient.GetStringAsync("", cancellationToken);

            Console.WriteLine("Hello World");
        }
    }
}