#if NET5_0_OR_GREATER
using System;
using System.Globalization;

namespace Newtonsoft.Json.Converters.Common
{
    public abstract partial class FormattedTimeOnlyConverterBase : JsonConverter
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
            return typeof(TimeOnly) == objectType ||
                   typeof(TimeOnly?) == objectType;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            JsonConverter<TimeOnly?> converter = new InternalFormattedTimeOnlyConverter(FormatString);
            TimeOnly? result = converter.ReadJson(reader, objectType, (TimeOnly?)existingValue, (TimeOnly?)existingValue is not null, serializer);
            if (objectType == typeof(TimeOnly))
                return result.GetValueOrDefault();
            return result;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            JsonConverter<TimeOnly?> converter = new InternalFormattedTimeOnlyConverter(FormatString);
            converter.WriteJson(writer, (TimeOnly?)value, serializer);
        }
    }

    partial class FormattedTimeOnlyConverterBase
    {
        private sealed class InternalFormattedTimeOnlyConverter : JsonConverter<TimeOnly?>
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

            public InternalFormattedTimeOnlyConverter(string formatString)
            {
                _formatString = formatString;
            }

            public override TimeOnly? ReadJson(JsonReader reader, Type objectType, TimeOnly? existingValue, bool hasExistingValue, JsonSerializer serializer)
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

                    TimeOnly result;
                    if (TimeOnly.TryParseExact(value, _formatString, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out result))
                        return result;
                    if (TimeOnly.TryParse(value, out result))
                        return result;

                    throw new JsonSerializationException($"Could not parse String '{value}' to TimeOnly.");
                }

                throw new JsonSerializationException($"Unexpected token type '{reader.TokenType}' when deserializing. Path '{reader.Path}'.");
            }

            public override void WriteJson(JsonWriter writer, TimeOnly? value, JsonSerializer serializer)
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
