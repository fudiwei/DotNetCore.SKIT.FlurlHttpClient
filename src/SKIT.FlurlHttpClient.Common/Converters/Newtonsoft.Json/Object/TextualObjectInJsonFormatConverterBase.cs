using System;

namespace Newtonsoft.Json.Converters
{
    public abstract class TextualObjectInJsonFormatConverterBase<T> : JsonConverter<T>
    {
        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return existingValue;
            }
            else if (reader.TokenType == JsonToken.String)
            {
                string? value = serializer.Deserialize<string>(reader);
                if (value == null)
                    return existingValue;

                return JsonConvert.DeserializeObject<T>(value);
            }

            throw new JsonSerializationException();
        }

        public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            if (value != null)
                writer.WriteValue(JsonConvert.SerializeObject(value, typeof(T), serializer.ExtractSerializerSettings()));
            else
                writer.WriteNull();
        }
    }
}
