using System;

namespace Newtonsoft.Json.Converters
{
    public class UnixMillisecondsNullableDateTimeOffsetConverter : JsonConverter<DateTimeOffset?>
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

                if (long.TryParse(value, out long l))
                    DateTimeOffset.FromUnixTimeMilliseconds(l);

                throw new JsonSerializationException($"Could not parse String '{value}' to Long.");
            }

            throw new JsonSerializationException($"Unexpected token type '{reader.TokenType}' when deserializing. Path '{reader.Path}'.");
        }

        public override void WriteJson(JsonWriter writer, DateTimeOffset? value, JsonSerializer serializer)
        {
            if (value.HasValue)
                writer.WriteValue(value.Value.ToUnixTimeMilliseconds());
            else
                writer.WriteNull();
        }
    }
}
