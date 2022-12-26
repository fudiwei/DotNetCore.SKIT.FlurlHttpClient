using System.Text.Json.Serialization;

namespace System.Text.Json.Converters.Common
{
    public class NumericalStringConverter : JsonConverter<string?>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetInt64(out long valueAsInt64))
                    return valueAsInt64.ToString();
                else if (reader.TryGetUInt64(out ulong valueAsUInt64))
                    return valueAsUInt64.ToString();
                else if (reader.TryGetDecimal(out decimal valueAsDecimal))
                    return valueAsDecimal.ToString();
                else if (reader.TryGetDouble(out double valueAsDouble))
                    return valueAsDouble.ToString();
                else
                    return reader.GetDecimal().ToString();
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                return reader.GetString();
            }

            throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
        }

        public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
            }
            else
            {
                if (string.IsNullOrEmpty(value))
                    writer.WriteStringValue(value);
                else if (long.TryParse(value, out long valueAsInt64))
                    writer.WriteNumberValue(valueAsInt64);
                else if (ulong.TryParse(value, out ulong valueAsUInt64))
                    writer.WriteNumberValue(valueAsUInt64);
                else if (decimal.TryParse(value, out decimal valueAsDecimal))
                    writer.WriteNumberValue(valueAsDecimal);
                else if (double.TryParse(value, out double valueAsDouble))
                    writer.WriteNumberValue(valueAsDouble);
                else
                    throw new JsonException($"Could not parse String '{value}' to Number.");
            }
        }
    }
}
