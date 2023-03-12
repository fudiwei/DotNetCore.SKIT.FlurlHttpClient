using System.Collections.Generic;

namespace Newtonsoft.Json.Converters.Common
{
    /// <summary>
    /// 一个 JSON 转换器，可针对指定适配类型做如下形式的对象转换。
    /// <code>
    ///   .NET → IList&lt;string&gt; Foo { get; } = new List&lt;string&gt;() { "a", "b" };
    ///   JSON → { "Foo": "a;b" }
    /// </code>
    /// 
    /// 适配类型：
    /// <code>  <see cref="IList{T}"/>(T=<see cref="string"/>) <see cref="List{T}"/>(T=<see cref="string"/>)</code>
    /// </summary>
    public sealed class StringifiedStringListWithSemicolonSplitConverter : StringifiedStringListWithSplitConverterBase
    {
        protected override string Separator
        {
            get { return ";"; }
        }
    }
}
