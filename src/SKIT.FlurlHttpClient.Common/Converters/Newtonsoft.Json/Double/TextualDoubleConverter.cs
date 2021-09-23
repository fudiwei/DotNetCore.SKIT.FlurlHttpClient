using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Newtonsoft.Json.Converters
{
    public class TextualDoubleConverter : JsonConverter<double>
    {
        private readonly JsonConverter<double?> _converter = new TextualNullableDoubleConverter();

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override double ReadJson(JsonReader reader, Type objectType, double existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return _converter.ReadJson(reader, objectType, existingValue, hasExistingValue, serializer) ?? default;
        }

        public override void WriteJson(JsonWriter writer, double value, JsonSerializer serializer)
        {
            _converter.WriteJson(writer, value, serializer);
        }
    }
}
