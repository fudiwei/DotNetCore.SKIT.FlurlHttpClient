### FAQ：如何使用拦截器？

---

拦截器是一种可以监视或重写请求调用的强大机制。下面给出一个用于记录传出请求和传入响应的拦截器简单示例：

```csharp
using SKIT.FlurlHttpClient;

public class LoggingInterceptor : HttpInterceptor
{
    private readonly ILogger _logger;

    public LoggingInterceptor(ILogger logger)
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
        logger.LogInformation($"Received response in {context.FlurlCall.Duration.Value.TotalMilliseconds}ms.");
        return Task.CompletedTask;
    }
}
```

示例代码中的 `FlurlCall` 对象，是 `Flurl.Http` 的内置类型，有关该类型的更进一步的说明，请自行阅读相关文档。

你可以在构造得到相应 SDK 的 API 客户端对象时，将拦截器注入到该客户端中：

```csharp
HttpInterceptor interceptor = new LoggingInterceptor(loggerFactory.CreateLogger());

// 利用构造器注入
builder.UseInterceptor(interceptor);
builder.Build();

// 或直接注入
client.Interceptors.Add(interceptor);
```

拦截器的工作方式类似于洋葱模型。对于请求拦截器而言，将按照添加时的顺序依次执行；对于响应拦截器而言，将按照添加时的顺序逆序依次执行。

拦截器在某些场景下非常有用。
