using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace System.Text.Json.Converters.Common
{
    public abstract partial class TextualStringListWithSplitConverterBase : JsonConverterFactory
    {
        protected abstract string Separator { get; }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsGenericType &&
                   typeof(IList<>).IsAssignableFrom(typeToConvert.GetGenericTypeDefinition()) &&
                   typeof(string) == typeToConvert.GetGenericArguments()[0];
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new InternalTextualStringListWithSplitConverter(Separator);
        }
    }

    partial class TextualStringListWithSplitConverterBase
    {
        private sealed class InternalTextualStringArrayWithSplitConverter : TextualStringArrayWithSplitConverterBase
        {
            protected override string Separator { get; }

            public InternalTextualStringArrayWithSplitConverter(string separator)
            {
                Separator = separator;
            }
        }

        private sealed class InternalTextualStringListWithSplitConverter : JsonConverter<IList<string>?>
        {
            private readonly JsonConverter<string[]?> _converter;

            public InternalTextualStringListWithSplitConverter(string separator)
            {
                _converter = new InternalTextualStringArrayWithSplitConverter(separator);
            }

            public override IList<string>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeof(string[]), options)?.ToList();
            }

            public override void Write(Utf8JsonWriter writer, IList<string>? value, JsonSerializerOptions options)
            {
                _converter.Write(writer, value?.ToArray(), options);
            }

            public override IList<string>? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, IList<string>? value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value?.ToArray(), options);
            }
        }
    }
}
