using System.Globalization;
using System.Text.Json.Serialization;

namespace System.Text.Json.Converters.Common
{
    public abstract partial class FormattedDateTimeOffsetConverterBase : JsonConverterFactory
    {
        protected abstract string FormatString { get; }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(DateTimeOffset) == typeToConvert ||
                   typeof(DateTimeOffset?) == typeToConvert;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeof(DateTimeOffset) == typeToConvert)
                return new InternalFormattedDateTimeOffsetConverter(FormatString);
            else if (typeof(DateTimeOffset?) == typeToConvert)
                return new InternalFormattedNullableDateTimeOffsetConverter(FormatString);

            throw new NotSupportedException();
        }
    }

    partial class FormattedDateTimeOffsetConverterBase
    {
        private sealed class InternalFormattedNullableDateTimeOffsetConverter : JsonConverter<DateTimeOffset?>
        {
            private readonly string _formatString;

            public InternalFormattedNullableDateTimeOffsetConverter(string formatString)
            {
                _formatString = formatString;
            }

            public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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

                    DateTimeOffset result;
                    if (DateTimeOffset.TryParseExact(value, _formatString, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out result))
                        return result;
                    if (DateTimeOffset.TryParse(value, out result))
                        return result;

                    throw new JsonException($"Could not parse String '{value}' to DateTimeOffset.");
                }

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
            }

            public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteStringValue(value.Value.ToString(_formatString, DateTimeFormatInfo.InvariantInfo));
            }
        }

        private sealed class InternalFormattedDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
        {
            private readonly JsonConverter<DateTimeOffset?> _converter;

            public InternalFormattedDateTimeOffsetConverter(string dateFormat)
            {
                _converter = new InternalFormattedNullableDateTimeOffsetConverter(dateFormat);
            }

            public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                DateTimeOffset? result = _converter.Read(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
            {
                _converter.Write(writer, value, options);
            }
        }
    }
}
