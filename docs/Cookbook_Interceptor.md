### 高级技巧：使用拦截器

---

拦截器是一种可以监视或重写请求调用的强大机制，在某些场景下非常有用。

你可以在构造得到相应 SDK 的 API 客户端对象时注册拦截器：

```csharp
// 可在构造器中注册
builder.UseInterceptor(new MyInterceptor());
builder.Build();

// 或直接注册到单个客户端
client.Interceptors.Add(new MyInterceptor());
```

拦截器的工作方式类似于洋葱模型。对于请求拦截器而言，将按照添加时的顺序依次执行；对于响应拦截器而言，将按照添加时的顺序逆序依次执行。

例如，当你按顺序注册了 I1、I2、I3 三个拦截器，对于发出的请求，将按照 I1、I2、I3 的顺序触发它们；对于接收的响应，将按照 I3、I2、I1 的顺序触发它们。

下面给出一个用于记录 HTTP 日志的拦截器简单示例：

```csharp
using Microsoft.Extensions.Logging;
using SKIT.FlurlHttpClient;

public class HttpLoggingInterceptor : HttpInterceptor
{
    private readonly ILogger _logger;

    public HttpLoggingInterceptor(ILogger logger)
    {
        _logger = logger;
    }

    public override Task BeforeCallAsync(HttpInterceptorContext context, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Sending request to {context.FlurlCall.Request.Url} on {DateTimeOffset.Now}.");
        return Task.CompletedTask;
    }

    public override Task AfterCallAsync(HttpInterceptorContext context, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Received response from {context.FlurlCall.Request.Url} in {context.FlurlCall.Duration}.");
        return Task.CompletedTask;
    }
}
```

示例代码中的 `FlurlCall` 对象，是 `Flurl.Http` 的内置类型，有关该类型的更进一步的说明，请自行阅读相关文档。
