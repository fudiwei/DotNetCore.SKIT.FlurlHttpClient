using System;

namespace Newtonsoft.Json.Converters
{
    public class TextualNullableLongReadOnlyConverter : JsonConverter<long?>
    {
        private readonly JsonConverter<long?> _converter = new TextualNullableLongConverter();

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override long? ReadJson(JsonReader reader, Type objectType, long? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return _converter.ReadJson(reader, objectType, existingValue, hasExistingValue, serializer);
        }

        public override void WriteJson(JsonWriter writer, long? value, JsonSerializer serializer)
        {
            if (value.HasValue)
                writer.WriteValue(value);
            else
                writer.WriteNull();
        }
    }
}
