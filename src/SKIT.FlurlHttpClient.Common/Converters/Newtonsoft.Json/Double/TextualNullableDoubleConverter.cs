using System;

namespace Newtonsoft.Json.Converters
{
    public class TextualNullableDoubleConverter : JsonConverter<double?>
    {
        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override double? ReadJson(JsonReader reader, Type objectType, double? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return existingValue;
            }
            else if (reader.TokenType == JsonToken.Integer)
            {
                long value = serializer.Deserialize<long>(reader);
                return value;
            }
            else if (reader.TokenType == JsonToken.Float)
            {
                double value = serializer.Deserialize<double>(reader);
                return value;
            }
            else if (reader.TokenType == JsonToken.String)
            {
                string? str = serializer.Deserialize<string>(reader);
                if (string.IsNullOrEmpty(str))
                    return existingValue;

                if (double.TryParse(str, out double value))
                    return Convert.ToInt64(value);
            }

            throw new JsonSerializationException();
        }

        public override void WriteJson(JsonWriter writer, double? value, JsonSerializer serializer)
        {
            if (value.HasValue)
                writer.WriteValue(value.Value);
            else
                writer.WriteNull();
        }
    }
}
