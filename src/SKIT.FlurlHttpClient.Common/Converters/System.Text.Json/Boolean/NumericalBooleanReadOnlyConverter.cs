using System.Text.Json.Serialization;

namespace System.Text.Json.Converters.Common
{
    public sealed partial class NumericalBooleanReadOnlyConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(bool) == typeToConvert ||
                   typeof(bool?) == typeToConvert;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeof(bool) == typeToConvert)
                return new InternalNumericalBooleanReadOnlyConverter();
            else if (typeof(bool?) == typeToConvert)
                return new InternalNumericalNullableBooleanReadOnlyConverter();

            throw new NotSupportedException();
        }
    }

    partial class NumericalBooleanReadOnlyConverter
    {
        private sealed class InternalNumericalNullableBooleanReadOnlyConverter : JsonConverter<bool?>
        {
            private readonly JsonConverterFactory _converterFactory = new NumericalBooleanConverter();

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

        private sealed class InternalNumericalBooleanReadOnlyConverter : JsonConverter<bool>
        {
            private readonly JsonConverter<bool?> _converter = new InternalNumericalNullableBooleanReadOnlyConverter();

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
