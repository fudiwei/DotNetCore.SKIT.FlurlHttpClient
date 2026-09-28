# CHANGELOG

- v3.1.1 (2025-01-24)
    - **变更**：升级依赖 `System.Text.Json` 至 v8.0.5。

- v3.1.0 (2024-07-13)
    - **新增**：新增 MIME 类型映射工具方法。
    - **变更**：停止对 .NET Core 2.0 目标框架的支持，最低支持至 .NET Core 3.1。

- v3.0.0 (2024-02-07)
    - **新增**：全新的可配置项、拦截器、异常等的实现。
    - **新增**：全新的自定义 JSON 序列化器。
    - **变更**：升级 .NET 基础库至 .NET 8.0。
    - **变更**：升级依赖 `Flurl` 至 v4.0.0。
    - **变更**：升级依赖 `Flurl.Http` 至 v4.0.2。
    - **变更**：停止对 .NET Framework 4.6.1 目标框架的支持，最低支持至 .NET Framework 4.6.2。

- v2.6.0 (2022-12-05)
    - **新增**：`ICommonResponse.RawHeaders` 调整为不区分标头名的大小写。
    - **变更**：升级依赖 `Flurl` 至 v3.0.6。
    - **变更**：升级依赖 `Flurl.Http` 至 v3.2.4。
    - **变更**：升级依赖 `Newtonsoft.Json` 至 v13.0.2。
    - **变更**：移除已被废弃的部分类型。

- v2.5.0 (2022-03-14)
    - **新增**：新增 `IFlurlHttpRequest.WithUrl` 的链式扩展方法。

- v2.4.2 (2022-03-11)
    - **修复**：修复命名空间拼写错误，将 `Contants` 修正为 `Constants`。

- v2.4.1 (2022-03-07, Deprecated)
    - **修复**：修复 `UnixTimestampDateTimeOffsetConverter`、`UnixMillisecondsDateTimeOffsetConverter` 不能正确处理字符串形式的数值的问题。

- v2.4.0 (2022-03-07, Deprecated)
    - **新增**：使 `UnixTimestampDateTimeOffsetConverter`、`UnixMillisecondsDateTimeOffsetConverter` 支持字符串形式的数值。

- v2.3.3 (2022-02-25)
    - **修复**：修复 `CommonClient.WrapResponseAsync` 方法内存泄露问题。

- v2.3.2 (2022-02-24, Deprecated)
    - **修复**：修复 `CommonClient.WrapResponseAsync` 方法传入 `CancellationToken.None` 抛出异常的问题。

- v2.3.1 (2022-02-23, Deprecated)
    - **修复**：修复 `DynamicObjectConverter` 的序列化不遵守 `JsonConverterAttribute` 特性的问题。

- v2.3.0 (2022-02-23, Deprecated)
    - **变更**：升级依赖 `Flurl` 至 v3.0.4。
    - **变更**：升级依赖 `Flurl.Http` 至 v3.2.2。
    - **变更**：升级依赖 `Newtonsoft.Json` 至 v13.0.1。
    - **变更**：重命名类型 `DynamicObjectReadOnlyConverter` 为 `DynamicObjectConverter`，并实现其 Write 方法。

- v2.2.0 (2022-01-20)
    - **新增**：补充若干自定义 JSON 转换器。

- v2.1.1 (2022-01-04)
    - **修复**：修复 `ICommonResponse` 接口属性可访问性定义问题。

- v2.1.0 (2022-01-04, Deprecated)
    - **新增**：新增 `CommonClient.WrapResponseAsync` 方法。
    - **新增**：补充若干自定义 JSON 转换器。
    - **变更**：只在判断响应字节数组可反序列化为 JSON 后才执行反序列化方法。

- v2.0.0 (2021-11-09)
    - **新增**：适配 .NET 6.0。
    - **新增**：补充若干自定义 JSON 转换器。
    - **变更**：重命名 `CommonClientSettings` 类型的属性 `Timeout` 为 `ConnectionRequestTimeout`。
    - **修复**：修复部分自定义 JSON 转换器在处理空字符串值时抛出异常的问题。

- v1.0.0 (2021-08-20, Deprecated)
    - **新增**：补充若干自定义 JSON 转换器。

- v0.1.0 (2021-08-20, Deprecated)
    - 首次发布。
