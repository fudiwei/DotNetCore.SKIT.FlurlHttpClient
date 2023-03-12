using System.Text.Json.Serialization;

namespace System.Text.Json.Converters.Common.Internal
{
    internal sealed class TextualNullableInt64Converter : JsonConverter<long?>
    {
        public override long? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt64();
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                string? value = reader.GetString();
                if (string.IsNullOrEmpty(value))
                    return null;

                if (long.TryParse(value, out long result))
                    return result;

                throw new JsonException($"Could not parse String '{value}' to Int64.");
            }

            throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");

            throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
        }

        public override void Write(Utf8JsonWriter writer, long? value, JsonSerializerOptions options)
        {
            if (value is null)
                writer.WriteNullValue();
            else
                writer.WriteStringValue(value.Value.ToString());
        }

        public override long? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string propName = reader.GetString()!;
            if (string.IsNullOrEmpty(propName))
                return null;

            if (long.TryParse(propName, out long result))
                return result;

            throw new JsonException($"Could not parse String '{propName}' to Int64.");
        }

        public override void WriteAsPropertyName(Utf8JsonWriter writer, long? value, JsonSerializerOptions options)
        {
            if (value is null)
                writer.WritePropertyName(string.Empty);
            else
                writer.WritePropertyName(value.Value.ToString());
        }
    }

    internal sealed class TextualInt64Converter : JsonConverter<long>
    {
        private readonly JsonConverter<long?> _converter = new TextualNullableInt64Converter();

        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            long? result = _converter.Read(ref reader, typeToConvert, options);
            return result.GetValueOrDefault();
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            _converter.Write(writer, value, options);
        }

        public override long ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            long? result = _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            return result.GetValueOrDefault();
        }

        public override void WriteAsPropertyName(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            _converter.WriteAsPropertyName(writer, value, options);
        }
    }
}
