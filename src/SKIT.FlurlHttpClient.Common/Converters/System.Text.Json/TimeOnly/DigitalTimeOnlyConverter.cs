#if NET5_0_OR_GREATER
using System;

namespace System.Text.Json.Converters.Common
{
    /// <summary>
    /// 一个 JSON 转换器，可针对指定适配类型做如下形式的对象转换。
    /// <code>
    ///   .NET → TimeOnly Foo { get; } = new TimeOnly(23, 59, 59);
    ///   JSON → { "Foo": "235959" }
    /// </code>
    /// 
    /// 适配类型：
    /// <code>  <see cref="TimeOnly"/> <see cref="TimeOnly"/>?</code>
    /// </summary>
    public sealed class DigitalTimeOnlyConverter : FormattedTimeOnlyConverterBase
    {
        protected override string FormatString
        {
            get { return "HHmmss"; }
        }
    }
}
#endif
