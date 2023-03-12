using System.Text.Json.Serialization;

namespace System.Text.Json.Converters.Common
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
    public sealed partial class NumericalBooleanConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(bool) == typeToConvert ||
                   typeof(bool?) == typeToConvert;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeof(bool) == typeToConvert)
                return new InternalNumericalBooleanConverter();
            if (typeof(bool?) == typeToConvert)
                return new InternalNumericalNullableBooleanConverter();

            throw new NotSupportedException();
        }
    }

    partial class NumericalBooleanConverter
    {
        private sealed class InternalNumericalNullableBooleanConverter : JsonConverter<bool?>
        {
            private const int TRUE_VALUE = 1;
            private const int FALSE_VALUE = 0;

            public override bool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return null;
                }
                else if (reader.TokenType == JsonTokenType.True)
                {
                    return true;
                }
                else if (reader.TokenType == JsonTokenType.False)
                {
                    return false;
                }
                else if (reader.TokenType == JsonTokenType.Number)
                {
                    int value = reader.GetInt32();
                    return Convert.ToBoolean(value);
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    if ((options.NumberHandling & JsonNumberHandling.AllowReadingFromString) > 0)
                    {
                        string? value = reader.GetString();
                        if (string.IsNullOrEmpty(value))
                            return null;

                        if (int.TryParse(value, out int result))
                            return Convert.ToBoolean(result);

                        throw new JsonException($"Could not parse String '{value}' to Int32.");
                    }
                }

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
            }

            public override void Write(Utf8JsonWriter writer, bool? value, JsonSerializerOptions options)
            {
                if (value is null)
                {
                    writer.WriteNullValue();
                }
                else
                {
                    if ((options.NumberHandling & JsonNumberHandling.WriteAsString) > 0)
                        writer.WriteStringValue(value.Value ? TRUE_VALUE.ToString() : FALSE_VALUE.ToString());
                    else
                        writer.WriteNumberValue(value.Value ? TRUE_VALUE : FALSE_VALUE);
                }
            }

            public override bool? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string propName = reader.GetString()!;
                if (string.IsNullOrEmpty(propName))
                    return null;

                if (int.TryParse(propName, out int result))
                    return Convert.ToBoolean(result);

                throw new JsonException($"Could not parse String '{propName}' to Int32.");
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, bool? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WritePropertyName(string.Empty);
                else
                    writer.WritePropertyName(value.Value ? TRUE_VALUE.ToString() : FALSE_VALUE.ToString());
            }
        }

        private sealed class InternalNumericalBooleanConverter : JsonConverter<bool>
        {
            private readonly JsonConverter<bool?> _converter = new InternalNumericalNullableBooleanConverter();

            public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                bool? result = _converter.Read(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
            {
                _converter.Write(writer, value, options);
            }

            public override bool ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                bool? result = _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }
    }
}
