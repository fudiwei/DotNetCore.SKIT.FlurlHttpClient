using System;

namespace Newtonsoft.Json.Converters.Common
{
    /// <summary>
    /// 一个 JSON 转换器，可针对指定适配类型做如下形式的对象转换。
    /// <code>
    ///   .NET → object Foo { get; } = new { Bar = "baz" };
    ///   JSON → { "Foo": "{\"Bar\":\"baz\"}" }
    /// </code>
    /// 
    /// 适配类型：
    /// <code>  任意类型。</code>
    /// </summary>
    public sealed class StringifiedObjectInJsonFormatConverter : JsonConverter
    {
        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return existingValue;
            }
            else if (reader.TokenType == JsonToken.String)
            {
                string? value = serializer.Deserialize<string>(reader);
                if (string.IsNullOrEmpty(value))
                    return existingValue;

                return JsonConvert.DeserializeObject(value!, objectType);
            }

            throw new JsonSerializationException($"Unexpected token type '{reader.TokenType}' when deserializing. Path '{reader.Path}'.");
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is null)
                writer.WriteNull();
            else
                writer.WriteValue(JsonConvert.SerializeObject(value, value.GetType(), serializer.ExtractSerializerSettings()));
        }
    }
}
