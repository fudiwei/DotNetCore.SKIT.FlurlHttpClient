using System;

namespace Newtonsoft.Json.Converters.Common
{
    /// <summary>
    /// 一个 JSON 转换器，可针对指定适配类型做如下形式的对象转换。
    /// <code>
    ///   .NET → bool Foo { get; } = true;
    ///   JSON → { "Foo": 1 }
    /// </code>
    /// 
    /// 适配类型：
    /// <code>  <see cref="bool"/> <see cref="bool"/>?</code>
    /// </summary>
    public sealed partial class NumericalBooleanConverter : JsonConverter
    {
        private readonly JsonConverter<bool?> _converter = new InternalNumericalBooleanConverter();

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
            return typeof(bool) == objectType ||
                   typeof(bool?) == objectType;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            bool? result = _converter.ReadJson(reader, objectType, (bool?)existingValue, (bool?)existingValue != null, serializer);
            if (objectType == typeof(bool))
                return result.GetValueOrDefault();
            return result;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            _converter.WriteJson(writer, (bool?)value, serializer);
        }
    }

    partial class NumericalBooleanConverter
    {
        private sealed class InternalNumericalBooleanConverter : JsonConverter<bool?>
        {
            private const int TRUE_VALUE = 1;
            private const int FALSE_VALUE = 0;

            public override bool CanRead
            {
                get { return true; }
            }

            public override bool CanWrite
            {
                get { return true; }
            }

            public override bool? ReadJson(JsonReader reader, Type objectType, bool? existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null)
                {
                    return existingValue;
                }
                else if (reader.TokenType == JsonToken.Boolean)
                {
                    return serializer.Deserialize<bool>(reader);
                }
                else if (reader.TokenType == JsonToken.Integer)
                {
                    int value = serializer.Deserialize<int>(reader);
                    return Convert.ToBoolean(value);
                }
                else if (reader.TokenType == JsonToken.String)
                {
                    string? value = serializer.Deserialize<string>(reader);
                    if (string.IsNullOrEmpty(value))
                        return existingValue;

                    if (int.TryParse(value, out int result))
                        return Convert.ToBoolean(result);

                    throw new JsonSerializationException($"Could not parse String '{value}' to Int32.");
                }

                throw new JsonSerializationException($"Unexpected token type '{reader.TokenType}' when deserializing. Path '{reader.Path}'.");
            }

            public override void WriteJson(JsonWriter writer, bool? value, JsonSerializer serializer)
            {
                if (value is null)
                    writer.WriteNull();
                else
                    writer.WriteValue(value.Value ? TRUE_VALUE : FALSE_VALUE);
            }
        }
    }
}
