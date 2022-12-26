using System;

namespace Newtonsoft.Json.Converters.Common
{
    public sealed partial class UnixMillisecondsDateTimeOffsetConverter : JsonConverter
    {
        private readonly JsonConverter<DateTimeOffset?> _converter = new InternalUnixMillisecondsDateTimeOffsetConverter();

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
            DateTimeOffset? result = _converter.ReadJson(reader, objectType, (DateTimeOffset?)existingValue, (DateTimeOffset?)existingValue != null, serializer);
            if (objectType == typeof(DateTimeOffset))
                return result.GetValueOrDefault();
            return result;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            _converter.WriteJson(writer, (DateTimeOffset?)value, serializer);
        }
    }

    partial class UnixMillisecondsDateTimeOffsetConverter
    {
        private sealed class InternalUnixMillisecondsDateTimeOffsetConverter : JsonConverter<DateTimeOffset?>
        {
            public override bool CanRead
            {
                get { return true; }
            }

            public override bool CanWrite
            {
                get { return true; }
            }

            public override DateTimeOffset? ReadJson(JsonReader reader, Type objectType, DateTimeOffset? existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null)
                {
                    return existingValue;
                }
                else if (reader.TokenType == JsonToken.Integer)
                {
                    long value = serializer.Deserialize<long>(reader);
                    return DateTimeOffset.FromUnixTimeMilliseconds(value);
                }
                else if (reader.TokenType == JsonToken.String)
                {
                    string? value = serializer.Deserialize<string>(reader);
                    if (string.IsNullOrEmpty(value))
                        return existingValue;

                    if (long.TryParse(value, out long n))
                        return DateTimeOffset.FromUnixTimeMilliseconds(n);

                    throw new JsonSerializationException($"Could not parse String '{value}' to Int64.");
                }

                throw new JsonSerializationException($"Unexpected token type '{reader.TokenType}' when deserializing. Path '{reader.Path}'.");
            }

            public override void WriteJson(JsonWriter writer, DateTimeOffset? value, JsonSerializer serializer)
            {
                if (value is null)
                    writer.WriteNull();
                else
                    writer.WriteValue(value.Value.ToUnixTimeMilliseconds());
            }
        }
    }
}
