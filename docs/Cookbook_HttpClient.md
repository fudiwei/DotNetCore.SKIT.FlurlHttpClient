### 高级技巧：配置 HttpClient

---

默认情况下，本库会在每个 API 客户端内部维护一个 `System.Net.Http.HttpClient` 对象。

如果你想手动配置 `HttpClient`（比如设置代理服务器），请在构造得到相应 SDK 的 API 客户端对象时：

```csharp
using System.Net;
using System.Net.Http;
using SKIT.FlurlHttpClient;

var httpClientHandler = new HttpClientHandler()
{
    Proxy = new WebProxy("http://my-proxy-server"),
    UseProxy = true
};
var httpClient = new HttpClient(httpClientHandler);

builder.UseHttpClient(httpClient);
builder.Build();
```
