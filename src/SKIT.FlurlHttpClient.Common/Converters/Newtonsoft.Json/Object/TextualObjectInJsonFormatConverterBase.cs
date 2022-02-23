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

#pragma warning disable CS8765
        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
#pragma warning restore CS8765
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
                
#pragma warning disable CS8603
                return JsonConvert.DeserializeObject<T>(value);
#pragma warning restore CS8603
            }

            throw new JsonSerializationException();
        }

#pragma warning disable CS8765
        public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
#pragma warning restore CS8765
        {
            if (value != null)
                writer.WriteValue(JsonConvert.SerializeObject(value, typeof(T), serializer.ExtractSerializerSettings()));
            else
                writer.WriteNull();
        }
    }
}
