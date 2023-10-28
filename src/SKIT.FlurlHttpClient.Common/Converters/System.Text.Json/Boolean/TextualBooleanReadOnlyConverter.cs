namespace System.Text.Json.Serialization.Common
{
    /// <summary>
    /// 一个 JSON 转换器，可针对指定适配类型做如下形式的对象转换。与 <seealso cref="TextualBooleanConverter"/> 类似，但转换过程是单向只读的。
    /// <code>
    ///   .NET → bool Foo { get; } = true;
    ///   JSON → { "Foo": "true" }
    /// </code>
    /// 
    /// 适配类型：
    /// <code>  <see cref="bool"/> <see cref="bool"/>?</code>
    /// </summary>
    public sealed partial class TextualBooleanReadOnlyConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(bool) == typeToConvert ||
                   typeof(bool?) == typeToConvert;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeof(bool) == typeToConvert)
                return new InternalTextualBooleanReadOnlyConverter();
            if (typeof(bool?) == typeToConvert)
                return new InternalTextualNullableBooleanReadOnlyConverter();

            throw new NotSupportedException();
        }
    }

    partial class TextualBooleanReadOnlyConverter
    {
        private sealed class InternalTextualNullableBooleanReadOnlyConverter : JsonConverter<bool?>
        {
            private readonly JsonConverterFactory _converterFactory = new TextualBooleanConverter();

            public override bool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                JsonConverter<bool?> converter = (JsonConverter<bool?>)_converterFactory.CreateConverter(typeof(bool?), options)!;
                return converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, bool? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteBooleanValue(value.Value);
            }
        }

        private sealed class InternalTextualBooleanReadOnlyConverter : JsonConverter<bool>
        {
            private readonly JsonConverter<bool?> _converter = new InternalTextualNullableBooleanReadOnlyConverter();

            public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                bool? result = _converter.Read(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
            {
                _converter.Write(writer, value, options);
            }
        }
    }
}
