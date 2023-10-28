using System.Text.RegularExpressions;

namespace System.Text.Json.Serialization.Common
{
    /// <summary>
    /// 一个 JSON 转换器，可针对指定适配类型做如下形式的对象转换。
    /// <code>
    ///   .NET → bool Foo { get; } = true;
    ///   JSON → { "Foo": "true" }
    /// </code>
    /// 
    /// 适配类型：
    /// <code>  <see cref="bool"/> <see cref="bool"/>?</code>
    /// </summary>
    public sealed partial class TextualBooleanConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(bool) == typeToConvert ||
                   typeof(bool?) == typeToConvert;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeof(bool) == typeToConvert)
                return new InternalTextualBooleanConverter();
            if (typeof(bool?) == typeToConvert)
                return new InternalTextualNullableBooleanConverter();

            throw new NotSupportedException();
        }
    }

    partial class TextualBooleanConverter
    {
        private sealed class InternalTextualNullableBooleanConverter : JsonConverter<bool?>
        {
            private const string TRUE_VALUE = "true";
            private const string FALSE_VALUE = "false";

            private static readonly Regex TRUE_REGEX = new Regex("^([T|t]rue|TRUE)$", RegexOptions.Compiled);
            private static readonly Regex FALSE_REGEX = new Regex("^([F|f]alse|FALSE)$", RegexOptions.Compiled);

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
                else if (reader.TokenType == JsonTokenType.String)
                {
                    string? value = reader.GetString();
                    if (string.IsNullOrEmpty(value))
                        return null;

                    if (TRUE_REGEX.IsMatch(value))
                        return true;
                    else if (FALSE_REGEX.IsMatch(value))
                        return false;

                    throw new JsonException($"Could not parse String '{value}' to Boolean.");
                }

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
            }

            public override void Write(Utf8JsonWriter writer, bool? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteStringValue(value.Value ? TRUE_VALUE : FALSE_VALUE);
            }

            public override bool? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string propName = reader.GetString()!;
                if (string.IsNullOrEmpty(propName))
                    return null;

                if (TRUE_REGEX.IsMatch(propName))
                    return true;
                else if (FALSE_REGEX.IsMatch(propName))
                    return false;

                throw new JsonException($"Could not parse String '{propName}' to Boolean.");
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, bool? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WritePropertyName(string.Empty);
                else
                    writer.WritePropertyName(value.Value ? TRUE_VALUE.ToString() : FALSE_VALUE.ToString());
            }
        }

        private sealed class InternalTextualBooleanConverter : JsonConverter<bool>
        {
            private readonly JsonConverter<bool?> _converter = new InternalTextualNullableBooleanConverter();

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
