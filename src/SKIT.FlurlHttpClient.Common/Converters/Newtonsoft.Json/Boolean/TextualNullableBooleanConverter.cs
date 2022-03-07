using System;

namespace Newtonsoft.Json.Converters
{
    public class TextualNullableBooleanConverter : JsonConverter<bool?>
    {
        private const string TRUE_TEXT = "true";
        private const string FALSE_TEXT = "false";

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override bool? ReadJson(JsonReader reader, Type objectType, bool? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return existingValue;
            }
            else if (reader.TokenType == JsonToken.Boolean)
            {
                return serializer.Deserialize<bool?>(reader);
            }
            else if (reader.TokenType == JsonToken.String)
            {
                string? value = serializer.Deserialize<string>(reader);
                if (string.IsNullOrEmpty(value))
                    return existingValue;

                if (TRUE_TEXT.Equals(value, StringComparison.OrdinalIgnoreCase))
                    return true;
                else if (FALSE_TEXT.Equals(value, StringComparison.OrdinalIgnoreCase))
                    return false;

                throw new JsonSerializationException($"Could not parse String '{value}' to Boolean.");
            }

            throw new JsonSerializationException($"Unexpected token type '{reader.TokenType}' when deserializing. Path '{reader.Path}'.");
        }

        public override void WriteJson(JsonWriter writer, bool? value, JsonSerializer serializer)
        {
            if (value.HasValue)
                writer.WriteValue(value.Value ? TRUE_TEXT : FALSE_TEXT);
            else
                writer.WriteNull();
        }
    }
}
