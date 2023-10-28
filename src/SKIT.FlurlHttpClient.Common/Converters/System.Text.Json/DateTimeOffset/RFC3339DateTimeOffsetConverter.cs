namespace System.Text.Json.Serialization.Common
{
    /// <summary>
    /// 一个 JSON 转换器，可针对指定适配类型做如下形式的对象转换。
    /// <code>
    ///   .NET → DateTimeOffset Foo { get; } = new DateTimeOffset(2000, 1, 1, 23, 59, 59);
    ///   JSON → { "Foo": "2000-01-01T23:59:59+08:00" }
    /// </code>
    /// 
    /// 适配类型：
    /// <code>  <see cref="DateTimeOffset"/> <see cref="DateTimeOffset"/>?</code>
    /// </summary>
    public sealed class Rfc3339DateTimeOffsetConverter : FormattedDateTimeOffsetConverterBase
    {
        protected override string FormatString
        {
            get { return "yyyy-MM-dd'T'HH:mm:sszzz"; }
        }
    }
}
