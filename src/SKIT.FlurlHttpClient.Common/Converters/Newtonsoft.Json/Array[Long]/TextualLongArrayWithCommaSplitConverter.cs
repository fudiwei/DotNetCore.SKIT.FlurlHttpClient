using System;

namespace Newtonsoft.Json.Converters
{
    public class TextualLongArrayWithCommaSplitConverter : JsonConverter<long[]?>
    {
        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override long[]? ReadJson(JsonReader reader, Type objectType, long[]? existingValue, bool hasExistingValue, JsonSerializer serializer)
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
                    return Array.Empty<long>();

                string[] strArr = value.Split(',');
                long[] lngArr = new long[strArr.Length];
                for (int i = 0; i < strArr.Length; i++)
                {
                    if (!long.TryParse(strArr[i], out long j))
                        throw new JsonSerializationException("Unexpected token when parsing string to long.");

                    lngArr[i] = j;
                }
                return lngArr;
            }

            throw new JsonSerializationException();
        }

        public override void WriteJson(JsonWriter writer, long[]? value, JsonSerializer serializer)
        {
            if (value != null)
                writer.WriteValue(string.Join(",", value));
            else
                writer.WriteNull();
        }
    }
}
