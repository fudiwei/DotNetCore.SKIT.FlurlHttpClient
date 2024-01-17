namespace System.Text.Json.Serialization.Common
{
    /// <summary>
    /// 一个 JSON 转换器，可针对指定适配类型做如下形式的对象转换。与 <seealso cref="NumericalStringConverter"/> 类似，但转换过程是单向只读的。
    /// <para>与通过 System.Text.Json.Serialization.<see cref="JsonNumberHandling.WriteAsString"/> 参数转换相比，可支持空字符串等特殊形式。</para>
    /// <code>
    ///   .NET → string Foo { get; } = "1";
    ///   JSON → { "Foo": 1 }
    /// </code>
    /// 
    /// 适配类型：
    /// <code>  <see cref="string"/></code>
    /// </summary>
    public class NumericalStringReadOnlyConverter : JsonConverter<string?>
    {
        private readonly JsonConverter<string?> _converter = new NumericalStringConverter();
        private readonly JsonConverter<string?> _fallback = (JsonConverter<string?>)JsonSerializerOptions.Default.GetConverter(typeof(string));

        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return _converter.Read(ref reader, typeToConvert, options);
        }

        public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
        {
            _fallback.Write(writer, value, options);
        }
    }
}
