using System;

namespace Newtonsoft.Json.Converters.Common
{
    /// <summary>
    /// 一个 JSON 转换器，可针对指定适配类型做如下形式的对象转换。
    /// <code>
    ///   .NET → DateTimeOffset Foo { get; } = new DateTimeOffset(2000, 1, 1, 23, 59, 59);
    ///   JSON → { "Foo": 946742399000 }
    /// </code>
    /// 
    /// 适配类型：
    /// <code>  <see cref="DateTimeOffset"/> <see cref="DateTimeOffset"/>?</code>
    /// </summary>
    public sealed partial class UnixMillisecondsDateTimeOffsetConverter : JsonConverter
    {
        private readonly JsonConverter<DateTimeOffset?> _converter = new InternalUnixMillisecondsDateTimeOffsetConverter();

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
            return typeof(DateTimeOffset) == objectType ||
                   typeof(DateTimeOffset?) == objectType;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            DateTimeOffset? result = _converter.ReadJson(reader, objectType, (DateTimeOffset?)existingValue, (DateTimeOffset?)existingValue != null, serializer);
            if (objectType == typeof(DateTimeOffset))
                return result.GetValueOrDefault();
            return result;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            _converter.WriteJson(writer, (DateTimeOffset?)value, serializer);
        }
    }

    partial class UnixMillisecondsDateTimeOffsetConverter
    {
        private sealed class InternalUnixMillisecondsDateTimeOffsetConverter : JsonConverter<DateTimeOffset?>
        {
            public override bool CanRead
            {
                get { return true; }
            }

            public override bool CanWrite
            {
                get { return true; }
            }

            public override DateTimeOffset? ReadJson(JsonReader reader, Type objectType, DateTimeOffset? existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null)
                {
                    return existingValue;
                }
                else if (reader.TokenType == JsonToken.Integer)
                {
                    long value = serializer.Deserialize<long>(reader);
                    return DateTimeOffset.FromUnixTimeMilliseconds(value);
                }
                else if (reader.TokenType == JsonToken.String)
                {
                    string? value = serializer.Deserialize<string>(reader);
                    if (string.IsNullOrEmpty(value))
                        return existingValue;

                    if (long.TryParse(value, out long n))
                        return DateTimeOffset.FromUnixTimeMilliseconds(n);

                    throw new JsonSerializationException($"Could not parse String '{value}' to Int64.");
                }

                throw new JsonSerializationException($"Unexpected token type '{reader.TokenType}' when deserializing. Path '{reader.Path}'.");
            }

            public override void WriteJson(JsonWriter writer, DateTimeOffset? value, JsonSerializer serializer)
            {
                if (value is null)
                    writer.WriteNull();
                else
                    writer.WriteValue(value.Value.ToUnixTimeMilliseconds());
            }
        }
    }
}
