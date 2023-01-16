#if NET5_0_OR_GREATER
using System;
using System.Globalization;

namespace Newtonsoft.Json.Converters.Common
{
    public abstract partial class FormattedDateOnlyConverterBase : JsonConverter
    {
        protected abstract string FormatString { get; }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(DateOnly) == objectType ||
                   typeof(DateOnly?) == objectType;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            JsonConverter<DateOnly?> converter = new InternalFormattedDateOnlyConverter(FormatString);
            DateOnly? result = converter.ReadJson(reader, objectType, (DateOnly?)existingValue, (DateOnly?)existingValue != null, serializer);
            if (objectType == typeof(DateOnly))
                return result.GetValueOrDefault();
            return result;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            JsonConverter<DateOnly?> converter = new InternalFormattedDateOnlyConverter(FormatString);
            converter.WriteJson(writer, (DateOnly?)value, serializer);
        }
    }

    partial class FormattedDateOnlyConverterBase
    {
        private sealed class InternalFormattedDateOnlyConverter : JsonConverter<DateOnly?>
        {
            private readonly string _formatString;

            public override bool CanRead
            {
                get { return true; }
            }

            public override bool CanWrite
            {
                get { return true; }
            }

            public InternalFormattedDateOnlyConverter(string formatString)
            {
                _formatString = formatString;
            }

            public override DateOnly? ReadJson(JsonReader reader, Type objectType, DateOnly? existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null)
                {
                    return existingValue;
                }
                else if (reader.TokenType == JsonToken.String)
                {
                    string? value = serializer.Deserialize<string>(reader);
                    if (string.IsNullOrEmpty(value))
                        return existingValue;

                    DateOnly result;
                    if (DateOnly.TryParseExact(value, _formatString, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out result))
                        return result;
                    if (DateOnly.TryParse(value, out result))
                        return result;

                    throw new JsonSerializationException($"Could not parse String '{value}' to DateOnly.");
                }

                throw new JsonSerializationException($"Unexpected token type '{reader.TokenType}' when deserializing. Path '{reader.Path}'.");
            }

            public override void WriteJson(JsonWriter writer, DateOnly? value, JsonSerializer serializer)
            {
                if (value is null)
                    writer.WriteNull();
                else
                    writer.WriteValue(value.Value.ToString(_formatString, DateTimeFormatInfo.InvariantInfo));
            }
        }
    }
}
#endif
