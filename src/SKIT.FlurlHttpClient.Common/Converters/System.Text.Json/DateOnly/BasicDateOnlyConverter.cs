#if NET5_0_OR_GREATER
namespace System.Text.Json.Serialization.Common
{
    /// <summary>
    /// 一个 JSON 转换器，可针对指定适配类型做如下形式的对象转换。
    /// <code>
    ///   .NET → DateOnly Foo { get; } = new DateOnly(2000, 1, 1);
    ///   JSON → { "Foo": "2000-01-01" }
    /// </code>
    /// 
    /// 适配类型：
    /// <code>  <see cref="DateOnly"/> <see cref="DateOnly"/>?</code>
    /// </summary>
    public sealed class BasicDateOnlyConverter : FormattedDateOnlyConverterBase
    {
        protected override string FormatString
        {
            get { return "yyyy-MM-dd"; }
        }
    }
}
#endif
