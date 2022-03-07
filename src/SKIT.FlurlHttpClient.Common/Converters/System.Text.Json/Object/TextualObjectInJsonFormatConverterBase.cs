using System.Text.Json.Serialization;

namespace System.Text.Json.Converters
{
    public abstract class TextualObjectInJsonFormatConverterBase<T> : JsonConverter<T>
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return default!;
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                string? value = reader.GetString();
                if (value == null)
                    return default!;

                return JsonSerializer.Deserialize<T>(value)!;
            }

            throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            if (value != null)
                writer.WriteStringValue(JsonSerializer.Serialize(value, typeof(T), options));
            else
                writer.WriteNullValue();
        }
    }
}
