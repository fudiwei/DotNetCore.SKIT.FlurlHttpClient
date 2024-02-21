### FAQ：如何与 `IHttpClientFactory` 集成？

---

> 请先自行阅读：
>
> [《Microsoft Docs - 使用 IHttpClientFactory 实现复原 HTTP 请求》](https://docs.microsoft.com/zh-cn/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests)
>
> [《Microsoft Docs - 在 ASP.NET Core 中使用 IHttpClientFactory 发出 HTTP 请求》](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/http-requests#httpclient-and-lifetime-management)
>
> [《Microsoft Docs - .NET Core 2.1 的新增功能：套接字改进》](https://docs.microsoft.com/zh-CN/dotnet/core/whats-new/dotnet-core-2-1#sockets-improvements)

如果你想手动管理 `HttpClient`，那么可以参考下面基于 DI/IoC 的代码实现：

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
            .UseHttpClient(_httpClientFactory.Create(), disposeClient: false) // 设置 HttpClient 不随客户端一同销毁
            .Build();
        return client;
    }
}
```

需要强调的是，虽然 `SKIT.FlurlHttpClient.Wechat.Api.WechatApiClient` 实现了 `System.IDisposable` 接口，但你不应该在 DI/IoC 中手动释放它，而是应该交给 DI/IoC 容器自动管理。否则，请务必配合 `using` 语句或显式地执行 `Dispose()` 方法，以免产生内存泄漏。
