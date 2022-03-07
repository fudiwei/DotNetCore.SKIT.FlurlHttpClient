using System;

namespace Newtonsoft.Json.Converters
{
    public class NumericalNullableBooleanConverter : JsonConverter<bool?>
    {
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
                return serializer.Deserialize<bool>(reader);
            }
            else if (reader.TokenType == JsonToken.Integer)
            {
                int value = serializer.Deserialize<int>(reader);
                return Convert.ToBoolean(value);
            }
            else if (reader.TokenType == JsonToken.String)
            {
                string? value = serializer.Deserialize<string>(reader);
                if (string.IsNullOrEmpty(value))
                    return existingValue;

                if (int.TryParse(value, out int i))
                    return Convert.ToBoolean(i);

                throw new JsonSerializationException($"Could not parse String '{value}' to Integer.");
            }

            throw new JsonSerializationException($"Unexpected token type '{reader.TokenType}' when deserializing. Path '{reader.Path}'.");
        }

        public override void WriteJson(JsonWriter writer, bool? value, JsonSerializer serializer)
        {
            if (value.HasValue)
                writer.WriteValue(value.Value ? 1 : 0);
            else
                writer.WriteNull();
        }
    }
}
