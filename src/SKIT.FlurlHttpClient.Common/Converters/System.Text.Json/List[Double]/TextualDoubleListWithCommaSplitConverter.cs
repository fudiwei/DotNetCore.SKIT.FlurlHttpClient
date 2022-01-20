using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace System.Text.Json.Converters
{
    public class TextualDoubleListWithCommaSplitConverter : JsonConverterFactory
    {
        private sealed class InnerTextualDoubleListWithCommaSplitConverter : JsonConverter<IList<double>?>
        {
            private readonly JsonConverter<double[]?> _converter = new TextualDoubleArrayWithCommaSplitConverter();

            public override IList<double>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                double[]? array = _converter.Read(ref reader, typeToConvert, options);
                IList<double>? list = array?.ToList();
                return list;
            }

            public override void Write(Utf8JsonWriter writer, IList<double>? value, JsonSerializerOptions options)
            {
                _converter.Write(writer, value?.ToArray(), options);
            }
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsGenericType &&
                   typeof(IList<>).IsAssignableFrom(typeToConvert.GetGenericTypeDefinition()) &&
                   typeof(double) == typeToConvert.GetGenericArguments()[0];
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new InnerTextualDoubleListWithCommaSplitConverter();
        }
    }
}
