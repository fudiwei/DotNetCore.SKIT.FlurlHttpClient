#if NET5_0_OR_GREATER
using System;

namespace Newtonsoft.Json.Converters.Common
{
    /// <summary>
    /// 一个 JSON 转换器，可针对指定适配类型做如下形式的对象转换。
    /// <code>
    ///   .NET → DateOnly Foo { get; } = new DateOnly(2000, 1, 1);
    ///   JSON → { "Foo": "20000101" }
    /// </code>
    /// 
    /// 适配类型：
    /// <code>  <see cref="DateOnly"/> <see cref="DateOnly"/>?</code>
    /// </summary>
    public sealed class DigitalDateOnlyConverter : FormattedDateOnlyConverterBase
    {
        protected override string FormatString
        {
            get { return "yyyyMMdd"; }
        }
    }
}
#endif
