using System;

namespace Newtonsoft.Json.Converters.Common
{
    /// <summary>
    /// 一个 JSON 转换器，可针对指定适配类型做如下形式的对象转换。与 <seealso cref="NumericalStringConverter"/> 类似，但转换过程是单向只读的。
    /// <code>
    ///   .NET → string Foo { get; } = "1";
    ///   JSON → { "Foo": 1 }
    /// </code>
    /// 
    /// 适配类型：
    /// <code>  <see cref="string"/></code>
    /// </summary>
    public sealed class NumericalStringReadOnlyConverter : JsonConverter<string?>
    {
        private static readonly JsonConverter<string?> _converter = new NumericalStringConverter();

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override string? ReadJson(JsonReader reader, Type objectType, string? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return _converter.ReadJson(reader, objectType, existingValue, hasExistingValue, serializer);
        }

        public override void WriteJson(JsonWriter writer, string? value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }
    }
}
