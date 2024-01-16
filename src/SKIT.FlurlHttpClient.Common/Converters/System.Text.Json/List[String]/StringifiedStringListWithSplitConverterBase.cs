using System.Collections.Generic;
using System.Linq;

namespace System.Text.Json.Serialization.Common
{
    public abstract partial class StringifiedStringListWithSplitConverterBase : JsonConverterFactory
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
            return new InternalStringifiedStringListWithSplitConverter(Separator);
        }
    }

    partial class StringifiedStringListWithSplitConverterBase
    {
        private sealed class InternalStringifiedStringArrayWithSplitConverter : StringifiedStringArrayWithSplitConverterBase
        {
            protected override string Separator { get; }

            public InternalStringifiedStringArrayWithSplitConverter(string separator)
            {
                Separator = separator;
            }
        }

        private sealed class InternalStringifiedStringListWithSplitConverter : JsonConverter<IList<string>?>
        {
            private readonly JsonConverter<string[]?> _converter;

            public InternalStringifiedStringListWithSplitConverter(string separator)
            {
                _converter = new InternalStringifiedStringArrayWithSplitConverter(separator);
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
                _converter.WriteAsPropertyName(writer, value?.ToArray()!, options);
            }
        }
    }
}
