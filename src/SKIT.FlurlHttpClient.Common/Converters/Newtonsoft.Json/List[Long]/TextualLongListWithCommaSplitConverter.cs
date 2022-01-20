using System;
using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json.Converters
{
    public class TextualLongListWithCommaSplitConverter : JsonConverter
    {
        private readonly JsonConverter<long[]?> _converter = new TextualLongArrayWithCommaSplitConverter();

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsGenericType && 
                   objectType.GetGenericTypeDefinition() == typeof(List<>) &&
                   objectType.GetElementType() == typeof(long);
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
            long[]? array = _converter.ReadJson(reader, objectType, default, serializer) as long[];
            return array?.ToList();
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            _converter.WriteJson(writer, value, serializer);
        }
    }
}
