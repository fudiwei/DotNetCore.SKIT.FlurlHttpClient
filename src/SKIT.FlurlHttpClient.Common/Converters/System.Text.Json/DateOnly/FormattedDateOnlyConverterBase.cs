#if NET5_0_OR_GREATER
using System.Globalization;
using System.Text.Json.Serialization;

namespace System.Text.Json.Converters.Common
{
    public abstract partial class FormattedDateOnlyConverterBase : JsonConverterFactory
    {
        protected abstract string FormatString { get; }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(DateOnly) == typeToConvert ||
                   typeof(DateOnly?) == typeToConvert;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeof(DateOnly) == typeToConvert)
                return new InternalFormattedDateOnlyConverter(FormatString);
            else if (typeof(DateOnly?) == typeToConvert)
                return new InternalFormattedNullableDateOnlyConverter(FormatString);

            throw new NotSupportedException();
        }
    }

    partial class FormattedDateOnlyConverterBase
    {
        private sealed class InternalFormattedNullableDateOnlyConverter : JsonConverter<DateOnly?>
        {
            private readonly string _dateFormat;

            public InternalFormattedNullableDateOnlyConverter(string dateFormat)
            {
                _dateFormat = dateFormat;
            }

            public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return null;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    string? value = reader.GetString();
                    if (string.IsNullOrEmpty(value))
                        return null;

                    DateOnly result;
                    if (DateOnly.TryParseExact(value, _dateFormat, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out result))
                        return result;
                    if (DateOnly.TryParse(value, out result))
                        return result;

                    throw new JsonException($"Could not parse String '{value}' to DateOnly.");
                }

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
            }

            public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteStringValue(value.Value.ToString(_dateFormat, DateTimeFormatInfo.InvariantInfo));
            }
        }

        private sealed class InternalFormattedDateOnlyConverter : JsonConverter<DateOnly>
        {
            private readonly JsonConverter<DateOnly?> _converter;

            public InternalFormattedDateOnlyConverter(string dateFormat)
            {
                _converter = new InternalFormattedNullableDateOnlyConverter(dateFormat);
            }

            public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                DateOnly? result = _converter.Read(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
            {
                _converter.Write(writer, value, options);
            }
        }
    }
}
#endif
