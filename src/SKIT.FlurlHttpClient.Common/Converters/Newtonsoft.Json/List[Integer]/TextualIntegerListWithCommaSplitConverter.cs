using System;
using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json.Converters
{
    public class TextualIntegerListWithCommaSplitConverter : JsonConverter
    {
        private readonly JsonConverter<int[]?> _converter = new TextualIntegerArrayWithCommaSplitConverter();

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsGenericType &&
                   objectType.GetGenericTypeDefinition() == typeof(List<>) &&
                   objectType.GetElementType() == typeof(int);
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
            int[]? array = _converter.ReadJson(reader, objectType, default, serializer) as int[];
            return array?.ToList();
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            _converter.WriteJson(writer, value, serializer);
        }
    }
}
