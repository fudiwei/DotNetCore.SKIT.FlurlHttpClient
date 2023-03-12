using System;
using System.Collections.Generic;
using System.Linq;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SKIT.FlurlHttpClient.Configuration
{
    /// <summary>
    /// <para>用于序列化 "application/x-www-form-urlencoded" 内容的序列化器。</para>
    /// <para>基于 SKIT.FlurlHttpClient.Configuration.<see cref="IJsonSerializer"/> 实现。</para>
    /// </summary>
    public class JsonifiedFormUrlEncodedSerializer : IFormUrlEncodedSerializer
    {
        private const string TRUE_VALUE = "true";
        private const string FALSE_VALUE = "false";

        private readonly DefaultUrlEncodedSerializer _flurlUrlEncodedSerializer = new DefaultUrlEncodedSerializer();

        protected IJsonSerializer JsonSerializer { get; }

        public JsonifiedFormUrlEncodedSerializer(IJsonSerializer jsonSerializer)
        {
            JsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
        }

        public virtual string Serialize(object? obj, Type type)
        {
            if (obj is null)
                return string.Empty;

            return _flurlUrlEncodedSerializer.Serialize(FlattenObject(obj, type));
        }

        protected IDictionary<string, object?> FlattenObject(object? obj, Type type)
        {
            // 判断是否需要平面化展开为简单字典结构
            bool flattenable = type.IsGenericType
                ? typeof(IDictionary<,>).IsAssignableFrom(type.GetGenericTypeDefinition())
                    ? Type.GetTypeCode(type.GetGenericArguments()[1]) == TypeCode.Object
                    : true
                : Type.GetTypeCode(type) == TypeCode.Object;

            // JSON 序列化
            string tmpJson = JsonSerializer.Serialize(obj, type);

            // JSON 反序列化
            IDictionary<string, object?> tmpDict = flattenable ?
                JObject.Parse(tmpJson)
                    .Descendants()
                    .Where(p => !p.Any())
                    .Aggregate(new Dictionary<string, object?>(), static (properties, jToken) =>
                    {
                        object? value;
                        string? valueAsString = jToken.Value<object>()?.ToString()?.Trim();
                        if ("[]".Equals(valueAsString))
                        {
                            value = Enumerable.Empty<object>();
                        }
                        else if ("{}".Equals(valueAsString))
                        {
                            value = new object();
                        }
                        else
                        {
                            value = (jToken as JValue)?.Value;

                            // 特殊处理：布尔值要转换为小写的字符串形式
                            if (Boolean.Equals(true, value))
                                value = TRUE_VALUE;
                            else if (Boolean.Equals(false, value))
                                value = FALSE_VALUE;
                        }

                        properties.Add(jToken.Path, value);
                        return properties;
                    }) :
                JsonConvert.DeserializeObject<Dictionary<string, object?>>(tmpJson)!;

            return tmpDict;
        }
    }
}
