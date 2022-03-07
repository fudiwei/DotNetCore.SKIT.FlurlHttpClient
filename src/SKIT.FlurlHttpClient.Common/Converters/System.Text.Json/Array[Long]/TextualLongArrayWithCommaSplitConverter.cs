using System.Text.Json.Serialization;

namespace System.Text.Json.Converters
{
    public class TextualLongArrayWithCommaSplitConverter : JsonConverter<long[]?>
    {
        public override long[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
                    return Array.Empty<long>();

                string[] strArr = value.Split(',');
                long[] intArr = new long[strArr.Length];
                for (int i = 0; i < strArr.Length; i++)
                {
                    if (!long.TryParse(strArr[i], out long j))
                        throw new JsonException($"Could not parse String '{strArr[i]}' to Long.");

                    intArr[i] = j;
                }
                return intArr;
            }

            throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
        }

        public override void Write(Utf8JsonWriter writer, long[]? value, JsonSerializerOptions options)
        {
            if (value != null)
                writer.WriteStringValue(string.Join(",", value));
            else
                writer.WriteNullValue();
        }
    }
}
