using System;

namespace Newtonsoft.Json.Converters
{
    public class TextualNullableDoubleReadOnlyConverter : JsonConverter<double?>
    {
        private readonly JsonConverter<double?> _converter = new TextualNullableDoubleConverter();

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override double? ReadJson(JsonReader reader, Type objectType, double? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return _converter.ReadJson(reader, objectType, existingValue, hasExistingValue, serializer);
        }

        public override void WriteJson(JsonWriter writer, double? value, JsonSerializer serializer)
        {
        }
    }
}
