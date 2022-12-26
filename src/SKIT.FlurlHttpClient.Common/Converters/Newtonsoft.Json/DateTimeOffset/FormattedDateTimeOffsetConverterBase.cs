using System;
using System.Globalization;

namespace Newtonsoft.Json.Converters.Common
{
    public abstract partial class FormattedDateTimeOffsetConverterBase : JsonConverter
    {
        protected abstract string DateFormat { get; }

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
            return typeof(DateTimeOffset) == objectType ||
                   typeof(DateTimeOffset?) == objectType;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            JsonConverter<DateTimeOffset?> converter = new InternalFormattedDateTimeOffsetConverter(DateFormat);
            DateTimeOffset? result = converter.ReadJson(reader, objectType, (DateTimeOffset?)existingValue, (DateTimeOffset?)existingValue != null, serializer);
            if (objectType == typeof(DateTimeOffset))
                return result.GetValueOrDefault();
            return result;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            JsonConverter<DateTimeOffset?> converter = new InternalFormattedDateTimeOffsetConverter(DateFormat);
            converter.WriteJson(writer, (DateTimeOffset?)value, serializer);
        }
    }

    partial class FormattedDateTimeOffsetConverterBase
    {
        private sealed class InternalFormattedDateTimeOffsetConverter : JsonConverter<DateTimeOffset?>
        {
            private readonly string _dateFormat;

            public override bool CanRead
            {
                get { return true; }
            }

            public override bool CanWrite
            {
                get { return true; }
            }

            public InternalFormattedDateTimeOffsetConverter(string dateFormat)
            {
                _dateFormat = dateFormat;
            }

            public override DateTimeOffset? ReadJson(JsonReader reader, Type objectType, DateTimeOffset? existingValue, bool hasExistingValue, JsonSerializer serializer)
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

                    DateTimeOffset result;
                    if (DateTimeOffset.TryParseExact(value, _dateFormat, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out result))
                        return result;
                    if (DateTimeOffset.TryParse(value, out result))
                        return result;

                    throw new JsonSerializationException($"Could not parse String '{value}' to DateTimeOffset.");
                }
                else if (reader.TokenType == JsonToken.Date)
                {
                    reader.DateFormatString = _dateFormat;
                    return serializer.Deserialize<DateTimeOffset>(reader);
                }

                throw new JsonSerializationException($"Unexpected token type '{reader.TokenType}' when deserializing. Path '{reader.Path}'.");
            }

            public override void WriteJson(JsonWriter writer, DateTimeOffset? value, JsonSerializer serializer)
            {
                if (value is null)
                    writer.WriteNull();
                else
                    writer.WriteValue(value.Value.ToString(_dateFormat, DateTimeFormatInfo.InvariantInfo));
            }
        }
    }
}
