# CHANGELOG

-   Release 2.4.0

    -   **修复**：使 `UnixTimestampDateTimeOffsetConverter` / `UnixMillisecondsDateTimeOffsetConverter` 支持字符串形式的数值。

-   Release 2.3.3

    -   **修复**：修复 `CommonClient.WrapResponseAsync` 方法内存泄露问题。

-   Release 2.3.2

    -   **修复**：修复 `CommonClient.WrapResponseAsync` 方法传入 `CancellationToken.None` 抛出异常的问题。

-   Release 2.3.1

    -   **修复**：修复 `DynamicObjectConverter` 的序列化不遵守 `JsonConverterAttribute` 特性的问题。

-   Release 2.3.0

    -   **变更**：升级依赖库。

    -   **变更**：重命名 `DynamicObjectReadOnlyConverter` 为 `DynamicObjectConverter`，并实现其 Write 方法。

-   Release 2.2.0

    -   **新增**：补充若干自定义 JSON 转换器。

-   Release 2.1.1

    -   **修复**：修复 `ICommonResponse` 接口属性定义问题。

-   Release 2.1.0

    -   **新增**：新增 `CommonClient.WrapResponseAsync` 方法。

    -   **新增**：补充若干自定义 JSON 转换器。

    -   **变更**：只在判断响应字节数组可反序列化为 JSON 后才执行反序列化方法。

-   Release 2.0.0

    -   **新增**：适配 .NET 6.0。

    -   **新增**：补充若干自定义 JSON 转换器。

    -   **变更**：重命名 `CommonClientSettings` 的属性 `Timeout` 为 `ConnectionRequestTimeout`。

    -   **修复**：修复部分自定义 JSON 转换器在处理空字符串值时抛出异常的问题。

-   Release 1.0.0

    -   首次发布。
