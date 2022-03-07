using System.Text.Json.Serialization;

namespace System.Text.Json.Converters
{
    public class TextualDoubleArrayWithCommaSplitConverter : JsonConverter<double[]?>
    {
        public override double[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
                if (string.IsNullOrEmpty(value))
                    return Array.Empty<double>();

                string[] strArr = value.Split(',');
                double[] intArr = new double[strArr.Length];
                for (int i = 0; i < strArr.Length; i++)
                {
                    if (!double.TryParse(strArr[i], out double j))
                        throw new JsonException($"Could not parse String '{strArr[i]}' to Double.");

                    intArr[i] = j;
                }
                return intArr;
            }

            throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
        }

        public override void Write(Utf8JsonWriter writer, double[]? value, JsonSerializerOptions options)
        {
            if (value != null)
                writer.WriteStringValue(string.Join(",", value));
            else
                writer.WriteNullValue();
        }
    }
}
