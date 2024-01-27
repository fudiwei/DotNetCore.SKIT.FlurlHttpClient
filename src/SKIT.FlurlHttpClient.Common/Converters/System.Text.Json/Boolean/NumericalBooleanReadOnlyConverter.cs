namespace System.Text.Json.Serialization.Common
{
    /// <summary>
    /// 一个 JSON 转换器，可针对指定适配类型做如下形式的对象转换。与 <seealso cref="NumericalBooleanConverter"/> 类似，但转换过程是单向只读的。
    /// <code>
    ///   .NET → bool Foo { get; } = true;
    ///   JSON → { "Foo": 1 }
    /// </code>
    /// 
    /// 适配类型：
    /// <code>  <see cref="bool"/> <see cref="bool"/>?</code>
    /// </summary>
    public sealed partial class NumericalBooleanReadOnlyConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(bool) == typeToConvert ||
                   typeof(bool?) == typeToConvert;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeof(bool) == typeToConvert)
                return new InternalNumericalBooleanReadOnlyConverter();
            if (typeof(bool?) == typeToConvert)
                return new InternalNumericalNullableBooleanReadOnlyConverter();

            throw new NotSupportedException();
        }
    }

    partial class NumericalBooleanReadOnlyConverter
    {
        private sealed class InternalNumericalNullableBooleanReadOnlyConverter : JsonConverter<bool?>
        {
            private static readonly JsonConverterFactory _factory = new NumericalBooleanConverter();
            private static readonly JsonConverter<bool?> _fallback = (JsonConverter<bool?>)JsonSerializerOptions.Default.GetConverter(typeof(bool?));

            public override bool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                JsonConverter<bool?> converter = (JsonConverter<bool?>)_factory.CreateConverter(typeof(bool?), options)!;
                return converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, bool? value, JsonSerializerOptions options)
            {
                _fallback.Write(writer, value, options);
            }
        }

        private sealed class InternalNumericalBooleanReadOnlyConverter : JsonConverter<bool>
        {
            private static readonly JsonConverter<bool?> _converter = new InternalNumericalNullableBooleanReadOnlyConverter();

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
