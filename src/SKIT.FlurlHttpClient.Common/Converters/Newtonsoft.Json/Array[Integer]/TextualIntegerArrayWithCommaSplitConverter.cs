using System;

namespace Newtonsoft.Json.Converters
{
    public class TextualIntegerArrayWithCommaSplitConverter : JsonConverter<int[]?>
    {
        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override int[]? ReadJson(JsonReader reader, Type objectType, int[]? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            else if (reader.TokenType == JsonToken.String)
            {
                string? value = serializer.Deserialize<string>(reader);
                if (value == null)
                    return null;
                if (string.IsNullOrEmpty(value))
                    return Array.Empty<int>();

                string[] strArr = value.Split(',');
                int[] intArr = new int[strArr.Length];
                for (int i = 0; i < strArr.Length; i++)
                {
                    if (!int.TryParse(strArr[i], out int j))
                        throw new JsonSerializationException($"Could not parse String '{strArr[i]}' to Integer.");

                    intArr[i] = j;
                }
                return intArr;
            }

            throw new JsonSerializationException($"Unexpected token type '{reader.TokenType}' when deserializing. Path '{reader.Path}'.");
        }

        public override void WriteJson(JsonWriter writer, int[]? value, JsonSerializer serializer)
        {
            if (value != null)
                writer.WriteValue(string.Join(",", value));
            else
                writer.WriteNull();
        }
    }
}
