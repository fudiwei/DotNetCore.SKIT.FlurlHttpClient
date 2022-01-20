using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace System.Text.Json.Converters
{
    public class TextualStringListWithCommaSplitConverter : JsonConverterFactory
    {
        private sealed class InnerTextualStringListWithCommaSplitConverter : JsonConverter<IList<string>?>
        {
            private readonly JsonConverter<string[]?> _converter = new TextualStringArrayWithCommaSplitConverter();

            public override IList<string>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string[]? array = _converter.Read(ref reader, typeToConvert, options);
                IList<string>? list = array?.ToList();
                return list;
            }

            public override void Write(Utf8JsonWriter writer, IList<string>? value, JsonSerializerOptions options)
            {
                _converter.Write(writer, value?.ToArray(), options);
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsGenericType &&
                   objectType.GetGenericTypeDefinition() == typeof(List<>) &&
                   objectType.GetGenericArguments()[0] == typeof(string);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new InnerTextualStringListWithCommaSplitConverter();
        }
    }
}
