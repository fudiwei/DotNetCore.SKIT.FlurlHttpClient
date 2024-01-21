using System;

namespace Newtonsoft.Json.Converters.Common
{
    /// <summary>
    /// 一个 JSON 转换器，可针对指定适配类型做如下形式的对象转换。与 <seealso cref="TextualNumberConverter"/> 类似，但转换过程是单向只读的。
    /// <code>
    ///   .NET → int Foo { get; } = 1;
    ///   JSON → { "Foo": "1" }
    /// </code>
    /// 
    /// 适配类型：
    /// <code>  <see cref="sbyte"/> <see cref="sbyte"/>?</code>
    /// <code>  <see cref="byte"/> <see cref="byte"/></code>
    /// <code>  <see cref="ushort"/> <see cref="ushort"/>?</code>
    /// <code>  <see cref="short"/> <see cref="short"/>?</code>
    /// <code>  <see cref="uint"/> <see cref="uint"/>?</code>
    /// <code>  <see cref="int"/> <see cref="int"/>?</code>
    /// <code>  <see cref="ulong"/> <see cref="ulong"/>?</code>
    /// <code>  <see cref="long"/> <see cref="long"/>?</code>
    /// <code>  <see cref="float"/> <see cref="float"/>?</code>
    /// <code>  <see cref="double"/> <see cref="double"/>?</code>
    /// <code>  <see cref="decimal"/> <see cref="decimal"/>?</code>
    /// </summary>
    public sealed class TextualNumberReadOnlyConverter : JsonConverter
    {
        private static readonly JsonConverter _converter = new TextualNumberConverter();

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return _converter.CanConvert(objectType);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            return _converter.ReadJson(reader, objectType, existingValue, serializer);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }
    }
}
