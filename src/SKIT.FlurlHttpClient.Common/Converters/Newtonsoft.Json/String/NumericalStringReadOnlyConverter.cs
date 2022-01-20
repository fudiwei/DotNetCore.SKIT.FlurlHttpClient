using System;

namespace Newtonsoft.Json.Converters
{
    public class NumericalStringReadOnlyConverter : JsonConverter<string?>
    {
        private readonly JsonConverter<string?> _converter = new NumericalStringConverter();

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override string? ReadJson(JsonReader reader, Type objectType, string? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return _converter.ReadJson(reader, objectType, existingValue, hasExistingValue, serializer);
        }

        public override void WriteJson(JsonWriter writer, string? value, JsonSerializer serializer)
        {
        }
    }
}
