using System.Text.Json.Serialization;

namespace System.Text.Json.Converters
{
    public class NumericalNullableBooleanConverter : JsonConverter<bool?>
    {
        public override bool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }
            else if (reader.TokenType == JsonTokenType.True)
            {
                return true;
            }
            else if (reader.TokenType == JsonTokenType.False)
            {
                return false;
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                int value = reader.GetInt32();
                return Convert.ToBoolean(value);
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                string? value = reader.GetString();
                if (string.IsNullOrEmpty(value))
                    return null;

                if (int.TryParse(value, out int i))
                    return Convert.ToBoolean(i);

                throw new JsonException($"Could not parse String '{value}' to Integer.");
            }

            throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
        }

        public override void Write(Utf8JsonWriter writer, bool? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteNumberValue(value.Value ? 1 : 0);
            else
                writer.WriteNullValue();
        }
    }
}
