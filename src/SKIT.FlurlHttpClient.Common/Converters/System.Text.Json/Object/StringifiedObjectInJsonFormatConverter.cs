namespace System.Text.Json.Serialization.Common
{
    /// <summary>
    /// 一个 JSON 转换器，可针对指定适配类型做如下形式的对象转换。
    /// <code>
    ///   .NET → object Foo { get; } = new { Bar = "baz" };
    ///   JSON → { "Foo": "{\"Bar\":\"baz\"}" }
    /// </code>
    /// 
    /// 适配类型：
    /// <code>  任意类型。</code>
    /// </summary>
    public sealed partial class StringifiedObjectInJsonFormatConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return true;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new InternalStringifiedObjectInJsonFormatConverter(typeToConvert);
        }
    }

    partial class StringifiedObjectInJsonFormatConverter
    {
        internal sealed class InternalStringifiedObjectInJsonFormatConverter : JsonConverter<object?>
        {
            private readonly Type _convertType;

            public InternalStringifiedObjectInJsonFormatConverter(Type convertType)
            {
                _convertType = convertType;
            }

            public override bool CanConvert(Type typeToConvert)
            {
                return _convertType == typeToConvert ||
                       _convertType.IsAssignableFrom(typeToConvert) ||
                       typeToConvert.IsSubclassOf(_convertType);
            }

            public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return default;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    string? value = reader.GetString();
                    if (string.IsNullOrEmpty(value))
                        return default;

                    return JsonSerializer.Deserialize(value!, typeToConvert)!;
                }

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
            }

            public override void Write(Utf8JsonWriter writer, object? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteStringValue(JsonSerializer.Serialize(value, value.GetType(), options));
            }
        }
    }

    /// <summary>
    /// <seealso cref="StringifiedObjectInJsonFormatConverter"/> 的泛型版本。
    /// </summary>
    public sealed class StringifiedObjectInJsonFormatConverter<T> : JsonConverter<T>
    {
        private readonly JsonConverter<object?> _converter = new StringifiedObjectInJsonFormatConverter.InternalStringifiedObjectInJsonFormatConverter(typeof(T));

        public override bool CanConvert(Type typeToConvert)
        {
            return _converter.CanConvert(typeToConvert);
        }

        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return (T?)_converter.Read(ref reader, typeToConvert, options);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            _converter.Write(writer, value, options);
        }
    }
}
