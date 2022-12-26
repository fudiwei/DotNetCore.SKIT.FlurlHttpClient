using System.Text.Json.Serialization;

namespace System.Text.Json.Converters.Common
{
    public sealed partial class TextualBooleanConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(bool) == typeToConvert ||
                   typeof(bool?) == typeToConvert;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeof(bool) == typeToConvert)
                return new InternalTextualBooleanConverter();
            else if (typeof(bool?) == typeToConvert)
                return new InternalTextualNullableBooleanConverter();

            throw new NotSupportedException();
        }
    }

    partial class TextualBooleanConverter
    {
        private sealed class InternalTextualNullableBooleanConverter : JsonConverter<bool?>
        {
            private const string TRUE_VALUE = "true";
            private const string FALSE_VALUE = "false";

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
                else if (reader.TokenType == JsonTokenType.String)
                {
                    string? value = reader.GetString();
                    if (string.IsNullOrEmpty(value))
                        return null;

                    if (TRUE_VALUE.Equals(value, StringComparison.OrdinalIgnoreCase))
                        return true;
                    else if (FALSE_VALUE.Equals(value, StringComparison.OrdinalIgnoreCase))
                        return false;

                    throw new JsonException($"Could not parse String '{value}' to Boolean.");
                }

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
            }

            public override void Write(Utf8JsonWriter writer, bool? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteStringValue(value.Value ? TRUE_VALUE : FALSE_VALUE);
            }
        }

        private sealed class InternalTextualBooleanConverter : JsonConverter<bool>
        {
            private readonly JsonConverter<bool?> _converter = new InternalTextualNullableBooleanConverter();

            public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                bool? result = _converter.Read(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
            {
                _converter.Write(writer, value, options);
            }
        }
    }
}
