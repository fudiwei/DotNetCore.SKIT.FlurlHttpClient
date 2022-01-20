using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace System.Text.Json.Converters
{
    public class TextualIntegerListWithCommaSplitConverter : JsonConverterFactory
    {
        private sealed class InnerTextualIntegerListWithCommaSplitConverter : JsonConverter<IList<int>?>
        {
            private readonly JsonConverter<int[]?> _converter = new TextualIntegerArrayWithCommaSplitConverter();

            public override IList<int>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                int[]? array = _converter.Read(ref reader, typeToConvert, options);
                IList<int>? list = array?.ToList();
                return list;
            }

            public override void Write(Utf8JsonWriter writer, IList<int>? value, JsonSerializerOptions options)
            {
                _converter.Write(writer, value?.ToArray(), options);
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsGenericType &&
                   objectType.GetGenericTypeDefinition() == typeof(List<>) &&
                   objectType.GetGenericArguments()[0] == typeof(int);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new InnerTextualIntegerListWithCommaSplitConverter();
        }
    }
}
