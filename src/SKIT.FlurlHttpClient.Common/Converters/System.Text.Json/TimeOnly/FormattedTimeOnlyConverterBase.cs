#if NET5_0_OR_GREATER
using System.Globalization;
using System.Text.Json.Serialization;

namespace System.Text.Json.Converters.Common
{
    public abstract partial class FormattedTimeOnlyConverterBase : JsonConverterFactory
    {
        protected abstract string FormatString { get; }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(TimeOnly) == typeToConvert ||
                   typeof(TimeOnly?) == typeToConvert;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeof(TimeOnly) == typeToConvert)
                return new InternalFormattedTimeOnlyConverter(FormatString);
            if (typeof(TimeOnly?) == typeToConvert)
                return new InternalFormattedNullableTimeOnlyConverter(FormatString);

            throw new NotSupportedException();
        }
    }

    partial class FormattedTimeOnlyConverterBase
    {
        private sealed class InternalFormattedNullableTimeOnlyConverter : JsonConverter<TimeOnly?>
        {
            private readonly string _dateFormat;

            public InternalFormattedNullableTimeOnlyConverter(string dateFormat)
            {
                _dateFormat = dateFormat;
            }

            public override TimeOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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

                    TimeOnly result;
                    if (TimeOnly.TryParseExact(value, _dateFormat, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out result))
                        return result;
                    if (TimeOnly.TryParse(value, out result))
                        return result;

                    throw new JsonException($"Could not parse String '{value}' to TimeOnly.");
                }

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
            }

            public override void Write(Utf8JsonWriter writer, TimeOnly? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteStringValue(value.Value.ToString(_dateFormat, DateTimeFormatInfo.InvariantInfo));
            }
        }

        private sealed class InternalFormattedTimeOnlyConverter : JsonConverter<TimeOnly>
        {
            private readonly JsonConverter<TimeOnly?> _converter;

            public InternalFormattedTimeOnlyConverter(string dateFormat)
            {
                _converter = new InternalFormattedNullableTimeOnlyConverter(dateFormat);
            }

            public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                TimeOnly? result = _converter.Read(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
            {
                _converter.Write(writer, value, options);
            }
        }
    }
}
#endif
