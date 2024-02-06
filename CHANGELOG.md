# CHANGELOG

-   Release 3.0.0

    -   **新增**：全新的可配置项、拦截器、异常等的实现。

    -   **新增**：全新的自定义 JSON 序列化器。

    -   **变更**：升级 .NET 基础库至 .NET 8.0。

    -   **变更**：升级依赖 `Flurl` 至 v4.0.0。

    -   **变更**：升级依赖 `Flurl.Http` 至 v4.0.2。

    -   **变更**：.NET Framework 目标框架由 .NET Framework 4.6.1 调整至 .NET Framework 4.6.2。

-   Release 2.6.0

    -   **新增**：`ICommonResponse.RawHeaders` 调整为不区分标头名的大小写。

    -   **变更**：升级依赖 `Flurl` 至 v3.0.6。

    -   **变更**：升级依赖 `Flurl.Http` 至 v3.2.4。

    -   **变更**：升级依赖 `Newtonsoft.Json` 至 v13.0.2。

    -   **变更**：移除已被废弃的部分类型。

-   Release 2.5.0

    -   **新增**：新增 `IFlurlHttpRequest.WithUrl` 的链式扩展方法。

-   Release 2.4.2

    -   **修复**：修复命名空间拼写错误，将 `Contants` 修正为 `Constants`。

-   Release 2.4.1

    -   **修复**：修复 `UnixTimestampDateTimeOffsetConverter`、`UnixMillisecondsDateTimeOffsetConverter` 不能正确处理字符串形式的数值的问题。

-   Release 2.4.0

    -   **新增**：使 `UnixTimestampDateTimeOffsetConverter`、`UnixMillisecondsDateTimeOffsetConverter` 支持字符串形式的数值。

-   Release 2.3.3

    -   **修复**：修复 `CommonClient.WrapResponseAsync` 方法内存泄露问题。

-   Release 2.3.2

    -   **修复**：修复 `CommonClient.WrapResponseAsync` 方法传入 `CancellationToken.None` 抛出异常的问题。

-   Release 2.3.1

    -   **修复**：修复 `DynamicObjectConverter` 的序列化不遵守 `JsonConverterAttribute` 特性的问题。

-   Release 2.3.0

    -   **变更**：升级依赖 `Flurl` 至 v3.0.4。

    -   **变更**：升级依赖 `Flurl.Http` 至 v3.2.2。

    -   **变更**：升级依赖 `Newtonsoft.Json` 至 v13.0.1。

    -   **变更**：重命名类型 `DynamicObjectReadOnlyConverter` 为 `DynamicObjectConverter`，并实现其 Write 方法。

-   Release 2.2.0

    -   **新增**：补充若干自定义 JSON 转换器。

-   Release 2.1.1

    -   **修复**：修复 `ICommonResponse` 接口属性可访问性定义问题。

-   Release 2.1.0

    -   **新增**：新增 `CommonClient.WrapResponseAsync` 方法。

    -   **新增**：补充若干自定义 JSON 转换器。

    -   **变更**：只在判断响应字节数组可反序列化为 JSON 后才执行反序列化方法。

-   Release 2.0.0

    -   **新增**：适配 .NET 6.0。

    -   **新增**：补充若干自定义 JSON 转换器。

    -   **变更**：重命名 `CommonClientSettings` 类型的属性 `Timeout` 为 `ConnectionRequestTimeout`。

    -   **修复**：修复部分自定义 JSON 转换器在处理空字符串值时抛出异常的问题。

-   Release 1.0.0

    -   首次发布。
