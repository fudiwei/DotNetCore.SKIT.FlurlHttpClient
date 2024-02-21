### FAQ：如何销毁客户端（避免内存泄漏）？

---

> 请先自行阅读：
>
> [《Microsoft Docs - 清理非托管资源》](https://learn.microsoft.com/zh-cn/dotnet/standard/garbage-collection/unmanaged)

各 SDK 模块 API 客户端均继承自公共模块的 `SKIT.FlurlHttpClient.CommonClientBase` 基类，它使用了一个非托管资源的 `System.Net.Http.HttpClient` 对象。因此，`SKIT.FlurlHttpClient.CommonClientBase` 实现了 `System.IDisposable` 接口。

在以 .NET Core / .NET 5.0+ 或更高版本为目标框架的项目中，我们强烈推荐开发者搭配 `System.Net.Http.IHttpClientFactory` 来使用这些客户端。此时，你无需手动销毁客户端。（请参阅本文档[《如何与 `IHttpClientFactory` 集成？》](./FAQ_IHttpClientFactory.md)有关章节）

在以 .NET Framework 为目标框架的项目中，我们建议开发者使用单例模式来使用这些客户端。

否则，你应该在使用完客户端后，使用 `using` 语句或显式地执行其 `Dispose()` 方法以释放非托管资源，以免产生内存泄漏。
