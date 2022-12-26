using System.Text.Json.Serialization;

namespace System.Text.Json.Converters.Common
{
    public sealed partial class UnixTimestampDateTimeOffsetConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(DateTimeOffset) == typeToConvert ||
                   typeof(DateTimeOffset?) == typeToConvert;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeof(DateTimeOffset) == typeToConvert)
                return new InternalUnixTimestampDateTimeOffsetConverter();
            else if (typeof(DateTimeOffset?) == typeToConvert)
                return new InternalUnixTimestampNullableDateTimeOffsetConverter();

            throw new NotSupportedException();
        }
    }

    partial class UnixTimestampDateTimeOffsetConverter
    {
        private sealed class InternalUnixTimestampNullableDateTimeOffsetConverter : JsonConverter<DateTimeOffset?>
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
                    return DateTimeOffset.FromUnixTimeSeconds(value);
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if ((options.NumberHandling & JsonNumberHandling.AllowReadingFromString) > 0)
                    {
                        string? value = reader.GetString();
                        if (string.IsNullOrEmpty(value))
                            return null;

                        if (long.TryParse(value, out long n))
                            return DateTimeOffset.FromUnixTimeSeconds(n);

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
                    writer.WriteNumberValue(value.Value.ToUnixTimeSeconds());
            }
        }

        private sealed class InternalUnixTimestampDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
        {
            private readonly JsonConverter<DateTimeOffset?> _converter = new InternalUnixTimestampNullableDateTimeOffsetConverter();

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
