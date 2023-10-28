namespace System.Text.Json.Serialization.Common
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
    public sealed partial class UnixMillisecondsDateTimeOffsetConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(DateTimeOffset) == typeToConvert ||
                   typeof(DateTimeOffset?) == typeToConvert;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeof(DateTimeOffset) == typeToConvert)
                return new InternalUnixMillisecondsDateTimeOffsetConverter();
            if (typeof(DateTimeOffset?) == typeToConvert)
                return new InternalUnixMillisecondsNullableDateTimeOffsetConverter();

            throw new NotSupportedException();
        }
    }

    partial class UnixMillisecondsDateTimeOffsetConverter
    {
        private sealed class InternalUnixMillisecondsNullableDateTimeOffsetConverter : JsonConverter<DateTimeOffset?>
        {
            public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return null;
                }
                else if (reader.TokenType == JsonTokenType.Number)
                {
                    long value = reader.GetInt64();
                    return DateTimeOffset.FromUnixTimeMilliseconds(value);
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if ((options.NumberHandling & JsonNumberHandling.AllowReadingFromString) > 0)
                    {
                        string? value = reader.GetString();
                        if (string.IsNullOrEmpty(value))
                            return null;

                        if (long.TryParse(value, out long n))
                            return DateTimeOffset.FromUnixTimeMilliseconds(n);

                        throw new JsonException($"Could not parse String '{value}' to Int64.");
                    }
                }

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
            }

            public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteNumberValue(value.Value.ToUnixTimeMilliseconds());
            }
        }

        private sealed class InternalUnixMillisecondsDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
        {
            private readonly JsonConverter<DateTimeOffset?> _converter = new InternalUnixMillisecondsNullableDateTimeOffsetConverter();

            public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                DateTimeOffset? result = _converter.Read(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
            {
                _converter.Write(writer, value, options);
            }
        }
    }
}
