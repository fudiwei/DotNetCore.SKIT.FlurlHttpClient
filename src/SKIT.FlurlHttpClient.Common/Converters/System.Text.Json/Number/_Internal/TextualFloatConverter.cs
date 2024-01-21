namespace System.Text.Json.Serialization.Common.Internal
{
    internal sealed class TextualNullableFloatConverter : JsonConverter<float?>
    {
        public override float? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetSingle();
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                string? value = reader.GetString();
                if (string.IsNullOrEmpty(value))
                    return null;

                if (float.TryParse(value, out float result))
                    return result;

                throw new JsonException($"Could not parse String '{value}' to Float.");
            }

            throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");

            throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
        }

        public override void Write(Utf8JsonWriter writer, float? value, JsonSerializerOptions options)
        {
            if (value is null)
                writer.WriteNullValue();
            else
                writer.WriteStringValue(value.Value.ToString());
        }

        public override float? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string propName = reader.GetString()!;
            if (string.IsNullOrEmpty(propName))
                return null;

            if (float.TryParse(propName, out float result))
                return result;

            throw new JsonException($"Could not parse String '{propName}' to Float.");
        }

        public override void WriteAsPropertyName(Utf8JsonWriter writer, float? value, JsonSerializerOptions options)
        {
            if (value is null)
                writer.WritePropertyName(string.Empty);
            else
                writer.WritePropertyName(value.Value.ToString());
        }
    }

    internal sealed class TextualFloatConverter : JsonConverter<float>
    {
        private readonly JsonConverter<float?> _converter = new TextualNullableFloatConverter();

        public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            float? result = _converter.Read(ref reader, typeToConvert, options);
            return result.GetValueOrDefault();
        }

        public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
        {
            _converter.Write(writer, value, options);
        }

        public override float ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            float? result = _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            return result.GetValueOrDefault();
        }

        public override void WriteAsPropertyName(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
        {
            _converter.WriteAsPropertyName(writer, value, options);
        }
    }
}
