using System;
using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json.Converters
{
    public class TextualDoubleListWithCommaSplitConverter : JsonConverter
    {
        private readonly JsonConverter<double[]?> _converter = new TextualDoubleArrayWithCommaSplitConverter();

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsGenericType && 
                   objectType.GetGenericTypeDefinition() == typeof(List<>) &&
                   objectType.GetElementType() == typeof(double);
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            double[]? array = _converter.ReadJson(reader, objectType, default, serializer) as double[];
            return array?.ToList();
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            _converter.WriteJson(writer, value, serializer);
        }
    }
}
