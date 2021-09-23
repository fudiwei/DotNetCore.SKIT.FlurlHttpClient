using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Newtonsoft.Json.Converters
{
    public class NumericalStringConverter : JsonConverter<string?>
    {
        public override bool CanRead 
        { 
            get { return true; } 
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override string? ReadJson(JsonReader reader, Type objectType, string? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return existingValue;
            }
            else if (reader.TokenType == JsonToken.Integer)
            {
                long value = serializer.Deserialize<long>(reader);
                return value.ToString();
            }
            else if (reader.TokenType == JsonToken.Float)
            {
                double value = serializer.Deserialize<double>(reader);
                return value.ToString();
            }
            else if (reader.TokenType == JsonToken.String)
            {
                return serializer.Deserialize<string>(reader);
            }

            throw new JsonReaderException();
        }

        public override void WriteJson(JsonWriter writer, string? value, JsonSerializer serializer)
        {
            if (value != null)
            {
                if (long.TryParse(value, out long valueAsInt64))
                    writer.WriteValue(valueAsInt64);
                else if (ulong.TryParse(value, out ulong valueAsUInt64))
                    writer.WriteValue(valueAsUInt64);
                else if (double.TryParse(value, out double valueAsDouble))
                    writer.WriteValue(valueAsDouble);
                else
                    writer.WriteValue(value);
            }
            else
            {
                writer.WriteNull();
            }
        }
    }
}
