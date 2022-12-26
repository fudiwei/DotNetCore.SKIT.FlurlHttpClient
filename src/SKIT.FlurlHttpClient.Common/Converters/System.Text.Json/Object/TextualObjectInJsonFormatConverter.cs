using System.Text.Json.Serialization;

namespace System.Text.Json.Converters.Common
{
    public partial class TextualObjectInJsonFormatConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return true;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new InternalTextualObjectInJsonFormatConverter(typeToConvert);
        }
    }

    partial class TextualObjectInJsonFormatConverter
    {
        private sealed class InternalTextualObjectInJsonFormatConverter : JsonConverter<object?>
        {
            private readonly Type _convertType;

            public InternalTextualObjectInJsonFormatConverter(Type convertType)
            {
                _convertType = convertType;
            }

            public override bool CanConvert(Type typeToConvert)
            {
                return _convertType == typeToConvert ||
                       _convertType.IsAssignableFrom(typeToConvert) ||
                       typeToConvert.IsSubclassOf(_convertType);
            }

            public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return default;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    string? value = reader.GetString();
                    if (string.IsNullOrEmpty(value))
                        return default;

                    return JsonSerializer.Deserialize(value!, typeToConvert)!;
                }

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
            }

            public override void Write(Utf8JsonWriter writer, object? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteStringValue(JsonSerializer.Serialize(value, value.GetType(), options));
            }
        }
    }
}
