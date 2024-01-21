using System;

namespace Newtonsoft.Json.Converters.Common
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
    public sealed class TextualBooleanReadOnlyConverter : JsonConverter
    {
        private static readonly JsonConverter _converter = new TextualBooleanConverter();

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
