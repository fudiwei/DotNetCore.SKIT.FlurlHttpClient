using System.Text.Json.Serialization;

namespace System.Text.Json.Converters.Common
{
    public abstract class TextualStringArrayWithSplitConverterBase : JsonConverter<string[]?>
    {
        protected abstract string Separator { get; }

        public override string[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                string? value = reader.GetString();
                if (value == null)
                    return null;
                if (value == string.Empty)
                    return Array.Empty<string>();

#if NET5_0_OR_GREATER
                return value.Split(Separator);
#else
                return value.Split(new string[] { Separator }, StringSplitOptions.None);
#endif
            }

            throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
        }

        public override void Write(Utf8JsonWriter writer, string[]? value, JsonSerializerOptions options)
        {
            if (value is null)
                writer.WriteNullValue();
            else
                writer.WriteStringValue(string.Join(Separator, value));
        }
    }
}
