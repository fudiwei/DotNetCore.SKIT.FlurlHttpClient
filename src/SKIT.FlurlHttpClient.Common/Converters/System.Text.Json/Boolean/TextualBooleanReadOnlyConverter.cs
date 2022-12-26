using System.Text.Json.Serialization;

namespace System.Text.Json.Converters.Common
{
    public sealed partial class TextualBooleanReadOnlyConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(bool) == typeToConvert ||
                   typeof(bool?) == typeToConvert;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeof(bool) == typeToConvert)
                return new InternalTextualBooleanReadOnlyConverter();
            else if (typeof(bool?) == typeToConvert)
                return new InternalTextualNullableBooleanReadOnlyConverter();

            throw new NotSupportedException();
        }
    }

    partial class TextualBooleanReadOnlyConverter
    {
        private sealed class InternalTextualNullableBooleanReadOnlyConverter : JsonConverter<bool?>
        {
            private readonly JsonConverterFactory _converterFactory = new TextualBooleanConverter();

            public override bool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                JsonConverter<bool?> converter = (JsonConverter<bool?>)_converterFactory.CreateConverter(typeof(bool?), options)!;
                return converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, bool? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteBooleanValue(value.Value);
            }
        }

        private sealed class InternalTextualBooleanReadOnlyConverter : JsonConverter<bool>
        {
            private readonly JsonConverter<bool?> _converter = new InternalTextualNullableBooleanReadOnlyConverter();

            public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                bool? result = _converter.Read(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
            {
                _converter.Write(writer, value, options);
            }
        }
    }
}
