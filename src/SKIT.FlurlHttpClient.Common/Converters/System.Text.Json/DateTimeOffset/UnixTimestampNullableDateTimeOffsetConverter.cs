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
            else if (reader.TokenType == JsonTokenType.String)
            {
                if ((options.NumberHandling & JsonNumberHandling.AllowReadingFromString) > 0)
                {
                    string? value = reader.GetString();
                    if (string.IsNullOrEmpty(value))
                        return null;

                    if (long.TryParse(value, out long l))
                        DateTimeOffset.FromUnixTimeSeconds(l);

                    throw new JsonException($"Could not parse String '{value}' to Long.");
                }
            }

            throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
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
