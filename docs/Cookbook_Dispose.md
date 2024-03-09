### 最佳实践：避免内存泄漏

---

> 请先自行阅读：
>
> [《Microsoft Docs - 清理非托管资源》](https://learn.microsoft.com/zh-cn/dotnet/standard/garbage-collection/unmanaged)

各 SDK 模块 API 客户端均继承自公共模块的 `SKIT.FlurlHttpClient.CommonClientBase` 基类，该基类内部使用了一个非托管资源的 `System.Net.Http.HttpClient` 对象。因此，为了确保资源正确释放，每个 API 客户端同时实现了 `System.IDisposable` 接口。

在以 .NET Core / .NET 5.0+ 或更高版本为目标框架的项目中，我们强烈推荐开发者搭配 `System.Net.Http.IHttpClientFactory` 来使用这些客户端。此时，你无需手动销毁客户端。具体做法请参阅本文档下[《最佳实践：与 `IHttpClientFactory` 集成》](./Cookbook_HttpClientFactory.md)这一章节的内容。

在以 .NET Framework 为目标框架的项目中，我们建议开发者使用单例模式来使用这些客户端。

否则，你应该在使用完客户端对象后，搭配 `using` 语法或显式地执行其 `Dispose()` 方法以释放非托管资源，以免产生内存泄漏。
