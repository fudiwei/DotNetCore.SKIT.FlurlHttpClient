using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace System.Text.Json.Converters
{
    public class UnixTimestampNullableDateTimeOffsetConverter : JsonConverter<DateTimeOffset?>
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

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteNumberValue(value.Value.ToUnixTimeSeconds());
            else
                writer.WriteNullValue();
        }
    }
}
