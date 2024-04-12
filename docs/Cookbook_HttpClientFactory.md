### 最佳实践：与 `IHttpClientFactory` 集成

---

> 请先自行阅读：
>
> [《Microsoft Docs - 使用 IHttpClientFactory 实现复原 HTTP 请求》](https://docs.microsoft.com/zh-cn/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests)
>
> [《Microsoft Docs - 在 ASP.NET Core 中使用 IHttpClientFactory 发出 HTTP 请求》](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/http-requests#httpclient-and-lifetime-management)
>
> [《Microsoft Docs - .NET Core 2.1 的新增功能：套接字改进》](https://docs.microsoft.com/zh-CN/dotnet/core/whats-new/dotnet-core-2-1#sockets-improvements)

我们强烈推荐开发者搭配 `System.Net.Http.IHttpClientFactory` 来使用相应 SDK 的 API 客户端，以获得更好的性能提升和开发体验。

下面给出一个基于 DI/IoC 实现的简单示例：

```csharp
/* 以微信 Wechat.Api 模块为例，其他 SDK 模块集成方式完全一致，只需替换为相应的构造器 */
using System.Net.Http;
using Microsoft.Extensions.Options;
using SKIT.FlurlHttpClient;
using SKIT.FlurlHttpClient.Wechat.Api;

public class WechatApiClientFactory
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOptions<WechatApiClientOptions> _wechatApiClientOptions;

    public WechatApiClientFactory(
        IHttpClientFactory httpClientFactory,
        IOptions<WechatApiClientOptions> wechatApiClientOptions)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _wechatApiClientOptions = wechatApiClientOptions ?? throw new ArgumentNullException(nameof(wechatApiClientOptions));
    }

    public WechatApiClient CreateClient()
    {
        WechatApiClient client = WechatApiClientBuilder.Create(_wechatApiClientOptions.Value)
            .UseHttpClient(_httpClientFactory.CreateClient(), disposeClient: false) // 设置 HttpClient 不随客户端一同销毁
            .Build();
        return client;
    }
}
```
