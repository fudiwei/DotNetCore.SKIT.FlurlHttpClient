using System.Text.Json.Serialization;

namespace System.Text.Json.Converters.Common.Internal
{
    internal sealed class TextualNullableInt16Converter : JsonConverter<short?>
    {
        public override short? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt16();
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                string? value = reader.GetString();
                if (string.IsNullOrEmpty(value))
                    return null;

                if (short.TryParse(value, out short result))
                    return result;

                throw new JsonException($"Could not parse String '{value}' to Int16.");
            }

            throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");

            throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
        }

        public override void Write(Utf8JsonWriter writer, short? value, JsonSerializerOptions options)
        {
            if (value is null)
                writer.WriteNullValue();
            else
                writer.WriteStringValue(value.Value.ToString());
        }

        public override short? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string propName = reader.GetString()!;
            if (string.IsNullOrEmpty(propName))
                return null;

            if (short.TryParse(propName, out short result))
                return result;

            throw new JsonException($"Could not parse String '{propName}' to Int16.");
        }

        public override void WriteAsPropertyName(Utf8JsonWriter writer, short? value, JsonSerializerOptions options)
        {
            if (value is null)
                writer.WritePropertyName(string.Empty);
            else
                writer.WritePropertyName(value.Value.ToString());
        }
    }

    internal sealed class TextualInt16Converter : JsonConverter<short>
    {
        private readonly JsonConverter<short?> _converter = new TextualNullableInt16Converter();

        public override short Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            short? result = _converter.Read(ref reader, typeToConvert, options);
            return result.GetValueOrDefault();
        }

        public override void Write(Utf8JsonWriter writer, short value, JsonSerializerOptions options)
        {
            _converter.Write(writer, value, options);
        }

        public override short ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            short? result = _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            return result.GetValueOrDefault();
        }

        public override void WriteAsPropertyName(Utf8JsonWriter writer, short value, JsonSerializerOptions options)
        {
            _converter.WriteAsPropertyName(writer, value, options);
        }
    }
}
