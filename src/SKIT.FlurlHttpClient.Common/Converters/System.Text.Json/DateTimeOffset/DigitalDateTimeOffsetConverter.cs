using System;

namespace System.Text.Json.Converters.Common
{
    /// <summary>
    /// 一个 JSON 转换器，可针对指定适配类型做如下形式的对象转换。
    /// <code>
    ///   .NET → DateTimeOffset Foo { get; } = new DateTimeOffset(2000, 1, 1, 23, 59, 59);
    ///   JSON → { "Foo": "20000101235959" }
    /// </code>
    /// 
    /// 适配类型：
    /// <code>  <see cref="DateTimeOffset"/> <see cref="DateTimeOffset"/>?</code>
    /// </summary>
    public sealed class DigitalDateTimeOffsetConverter : FormattedDateTimeOffsetConverterBase
    {
        protected override string FormatString
        {
            get { return "yyyyMMddHHmmss"; }
        }
    }
}
