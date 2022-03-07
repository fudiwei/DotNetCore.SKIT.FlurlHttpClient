using System.Globalization;
using System.Text.Json.Serialization;

namespace System.Text.Json.Converters
{
    public class RFC3339NullableDateTimeOffsetConverter : JsonConverter<DateTimeOffset?>
    {
        internal const string DATETIME_FORMAT = Newtonsoft.Json.Converters.RFC3339NullableDateTimeOffsetConverter.DATETIME_FORMAT;

        public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                string? value = reader.GetString();
                if (string.IsNullOrEmpty(value))
                    return null;

                if (DateTimeOffset.TryParseExact(value, DATETIME_FORMAT, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out DateTimeOffset d))
                    return d;

                if (DateTimeOffset.TryParse(value, out d))
                    return d;

                throw new JsonException($"Could not parse String '{value}' to DateTimeOffset.");
            }

            throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteStringValue(value.Value.ToString(DATETIME_FORMAT, DateTimeFormatInfo.InvariantInfo));
            else
                writer.WriteNullValue();
        }
    }
}
