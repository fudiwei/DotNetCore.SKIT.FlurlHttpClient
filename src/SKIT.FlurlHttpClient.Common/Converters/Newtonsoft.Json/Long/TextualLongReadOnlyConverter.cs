using System;

namespace Newtonsoft.Json.Converters
{
    public class TextualLongReadOnlyConverter : JsonConverter<long>
    {
        private readonly JsonConverter<long> _converter = new TextualLongConverter();

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override long ReadJson(JsonReader reader, Type objectType, long existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return _converter.ReadJson(reader, objectType, existingValue, hasExistingValue, serializer);
        }

        public override void WriteJson(JsonWriter writer, long value, JsonSerializer serializer)
        {
        }
    }
}
