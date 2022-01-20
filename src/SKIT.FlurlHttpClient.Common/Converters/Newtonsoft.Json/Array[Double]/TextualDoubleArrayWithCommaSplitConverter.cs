using System;

namespace Newtonsoft.Json.Converters
{
    public class TextualDoubleArrayWithCommaSplitConverter : JsonConverter<double[]?>
    {
        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override double[]? ReadJson(JsonReader reader, Type objectType, double[]? existingValue, bool hasExistingValue, JsonSerializer serializer)
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
                    return Array.Empty<double>();

                string[] strArr = value.Split(',');
                double[] dblArr = new double[strArr.Length];
                for (int i = 0; i < strArr.Length; i ++)
                {
                    if (!double.TryParse(strArr[i], out double j))
                        throw new JsonSerializationException("Unexpected token when parsing string to double.");

                    dblArr[i] = j;
                }
                return dblArr;
            }

            throw new JsonSerializationException();
        }

        public override void WriteJson(JsonWriter writer, double[]? value, JsonSerializer serializer)
        {
            if (value != null)
                writer.WriteValue(string.Join(",", value));
            else
                writer.WriteNull();
        }
    }
}
