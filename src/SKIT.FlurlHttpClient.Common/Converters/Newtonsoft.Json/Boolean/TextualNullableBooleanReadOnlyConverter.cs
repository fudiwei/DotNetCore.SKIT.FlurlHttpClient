using System;

namespace Newtonsoft.Json.Converters
{
    public class TextualNullableBooleanReadOnlyConverter : JsonConverter<bool?>
    {
        private readonly JsonConverter<bool?> _converter = new TextualNullableBooleanConverter();

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override bool? ReadJson(JsonReader reader, Type objectType, bool? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return _converter.ReadJson(reader, objectType, existingValue, hasExistingValue, serializer);
        }

        public override void WriteJson(JsonWriter writer, bool? value, JsonSerializer serializer)
        {
        }
    }
}
