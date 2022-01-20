using System;

namespace Newtonsoft.Json.Converters
{
    public class TextualNullableIntegerReadOnlyConverter : JsonConverter<int?>
    {
        private readonly JsonConverter<int?> _converter = new TextualNullableIntegerConverter();

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override int? ReadJson(JsonReader reader, Type objectType, int? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return _converter.ReadJson(reader, objectType, existingValue, hasExistingValue, serializer);
        }

        public override void WriteJson(JsonWriter writer, int? value, JsonSerializer serializer)
        {
        }
    }
}
