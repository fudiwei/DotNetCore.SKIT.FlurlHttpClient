namespace System.Text.Json.Serialization.Common.Internal
{
    internal sealed class TextualNullableSByteConverter : JsonConverter<sbyte?>
    {
        public override sbyte? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetSByte();
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                string? value = reader.GetString();
                if (string.IsNullOrEmpty(value))
                    return null;

                if (sbyte.TryParse(value, out sbyte result))
                    return result;

                throw new JsonException($"Could not parse String '{value}' to SByte.");
            }

            throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");

            throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
        }

        public override void Write(Utf8JsonWriter writer, sbyte? value, JsonSerializerOptions options)
        {
            if (value is null)
                writer.WriteNullValue();
            else
                writer.WriteStringValue(value.Value.ToString());
        }

        public override sbyte? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string propName = reader.GetString()!;
            if (string.IsNullOrEmpty(propName))
                return null;

            if (sbyte.TryParse(propName, out sbyte result))
                return result;

            throw new JsonException($"Could not parse String '{propName}' to SByte.");
        }

        public override void WriteAsPropertyName(Utf8JsonWriter writer, sbyte? value, JsonSerializerOptions options)
        {
            if (value is null)
                writer.WritePropertyName(string.Empty);
            else
                writer.WritePropertyName(value.Value.ToString());
        }
    }

    internal sealed class TextualSByteConverter : JsonConverter<sbyte>
    {
        private readonly JsonConverter<sbyte?> _converter = new TextualNullableSByteConverter();

        public override sbyte Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            sbyte? result = _converter.Read(ref reader, typeToConvert, options);
            return result.GetValueOrDefault();
        }

        public override void Write(Utf8JsonWriter writer, sbyte value, JsonSerializerOptions options)
        {
            _converter.Write(writer, value, options);
        }

        public override sbyte ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            sbyte? result = _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            return result.GetValueOrDefault();
        }

        public override void WriteAsPropertyName(Utf8JsonWriter writer, sbyte value, JsonSerializerOptions options)
        {
            _converter.WriteAsPropertyName(writer, value, options);
        }
    }
}
