### FAQ：如何指定 JSON 序列化器？

---

> 请先自行阅读：
>
> [《Microsoft Docs - .NET 中的 JSON 序列化和反序列化（封送和拆收）》](https://docs.microsoft.com/zh-cn/dotnet/standard/serialization/system-text-json-overview)

默认情况下，本库使用 `System.Text.Json` 作为 JSON 序列化器。

如果你希望调整一些序列化器的配置项，那么你可以在构造得到相应 SDK 的 API 客户端对象后：

```csharp
using System.Text.Json;
using SKIT.FlurlHttpClient;

client.Configure(config =>
{
    JsonSerializerOptions jsonSerializerOptions = SystemTextJsonSerializer.GetDefaultSerializerOptions();
    jsonSerializerOptions.WriteIndented = true;
    config.JsonSerializer = new SystemTextJsonSerializer(jsonSerializerOptions);
});
```

如果你更习惯于 `Newtonsoft.Json`，那么你可以像这样指定：

```csharp
using Newtonsoft.Json;
using SKIT.FlurlHttpClient;

client.Configure(config =>
{
    JsonSerializerSettings jsonSerializerSettings = NewtonsoftJsonSerializer.GetDefaultSerializerSettings();
    jsonSerializerSettings.Formatting = Formatting.Indented;
    config.JsonSerializer = new NewtonsoftJsonSerializer(jsonSerializerSettings);
});
```

需要注意的是，虽然你也可在代码中指定成其他实现 `SKIT.FlurlHttpClient.IJsonSerializer` 的 JSON 序列化器，但因本库的接口模型定义与实际发送的 JSON 数据并非完全一致，使用其他实现会导致意外的执行结果，所以请务必只使用本库内置的这两种 JSON 序列化器。
