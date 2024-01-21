namespace System.Text.Json.Serialization.Common.Internal
{
    internal sealed class TextualNullableDecimalConverter : JsonConverter<decimal?>
    {
        public override decimal? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetDecimal();
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                string? value = reader.GetString();
                if (string.IsNullOrEmpty(value))
                    return null;

                if (decimal.TryParse(value, out decimal result))
                    return result;

                throw new JsonException($"Could not parse String '{value}' to Decimal.");
            }

            throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");

            throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
        }

        public override void Write(Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options)
        {
            if (value is null)
                writer.WriteNullValue();
            else
                writer.WriteStringValue(value.Value.ToString());
        }

        public override decimal? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string propName = reader.GetString()!;
            if (string.IsNullOrEmpty(propName))
                return null;

            if (decimal.TryParse(propName, out decimal result))
                return result;

            throw new JsonException($"Could not parse String '{propName}' to Decimal.");
        }

        public override void WriteAsPropertyName(Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options)
        {
            if (value is null)
                writer.WritePropertyName(string.Empty);
            else
                writer.WritePropertyName(value.Value.ToString());
        }
    }

    internal sealed class TextualDecimalConverter : JsonConverter<decimal>
    {
        private static readonly JsonConverter<decimal?> _converter = new TextualNullableDecimalConverter();

        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            decimal? result = _converter.Read(ref reader, typeToConvert, options);
            return result.GetValueOrDefault();
        }

        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
        {
            _converter.Write(writer, value, options);
        }

        public override decimal ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            decimal? result = _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            return result.GetValueOrDefault();
        }

        public override void WriteAsPropertyName(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
        {
            _converter.WriteAsPropertyName(writer, value, options);
        }
    }
}
