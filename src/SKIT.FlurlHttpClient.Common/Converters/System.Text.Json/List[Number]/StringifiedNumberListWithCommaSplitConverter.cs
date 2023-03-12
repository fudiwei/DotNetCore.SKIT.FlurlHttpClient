using System.Collections.Generic;

namespace System.Text.Json.Converters.Common
{
    /// <summary>
    /// 一个 JSON 转换器，可针对指定适配类型做如下形式的对象转换。
    /// <code>
    ///   .NET → IList&lt;int&gt; Foo { get; } = new List&lt;int&gt;() { 1, 2 };
    ///   JSON → { "Foo": "1,2" }
    /// </code>
    /// 
    /// 适配类型：
    /// <code>  <see cref="IList{T}"/>(T=<see cref="sbyte"/>) <see cref="IList{T}"/>(T=<see cref="sbyte"/>?) <see cref="List{T}"/>(T=<see cref="sbyte"/>) <see cref="List{T}"/>(T=<see cref="sbyte"/>?)</code>
    /// <code>  <see cref="IList{T}"/>(T=<see cref="byte"/>) <see cref="IList{T}"/>(T=<see cref="byte"/>?) <see cref="List{T}"/>(T=<see cref="byte"/>) <see cref="List{T}"/>(T=<see cref="byte"/>?)</code>
    /// <code>  <see cref="IList{T}"/>(T=<see cref="ushort"/>) <see cref="IList{T}"/>(T=<see cref="ushort"/>?) <see cref="List{T}"/>(T=<see cref="ushort"/>) <see cref="List{T}"/>(T=<see cref="ushort"/>?)</code>
    /// <code>  <see cref="IList{T}"/>(T=<see cref="short"/>) <see cref="IList{T}"/>(T=<see cref="short"/>?) <see cref="List{T}"/>(T=<see cref="short"/>) <see cref="List{T}"/>(T=<see cref="short"/>?)</code>
    /// <code>  <see cref="IList{T}"/>(T=<see cref="uint"/>) <see cref="IList{T}"/>(T=<see cref="uint"/>?) <see cref="List{T}"/>(T=<see cref="uint"/>) <see cref="List{T}"/>(T=<see cref="uint"/>?)</code>
    /// <code>  <see cref="IList{T}"/>(T=<see cref="int"/>) <see cref="IList{T}"/>(T=<see cref="int"/>?) <see cref="List{T}"/>(T=<see cref="int"/>) <see cref="List{T}"/>(T=<see cref="int"/>?)</code>
    /// <code>  <see cref="IList{T}"/>(T=<see cref="ulong"/>) <see cref="IList{T}"/>(T=<see cref="ulong"/>?) <see cref="List{T}"/>(T=<see cref="ulong"/>) <see cref="List{T}"/>(T=<see cref="ulong"/>?)</code>
    /// <code>  <see cref="IList{T}"/>(T=<see cref="long"/>) <see cref="IList{T}"/>(T=<see cref="long"/>?) <see cref="List{T}"/>(T=<see cref="long"/>) <see cref="List{T}"/>(T=<see cref="long"/>?)</code>
    /// <code>  <see cref="IList{T}"/>(T=<see cref="float"/>) <see cref="IList{T}"/>(T=<see cref="float"/>?) <see cref="List{T}"/>(T=<see cref="float"/>) <see cref="List{T}"/>(T=<see cref="float"/>?)</code>
    /// <code>  <see cref="IList{T}"/>(T=<see cref="double"/>) <see cref="IList{T}"/>(T=<see cref="double"/>?) <see cref="List{T}"/>(T=<see cref="double"/>) <see cref="List{T}"/>(T=<see cref="double"/>?)</code>
    /// <code>  <see cref="IList{T}"/>(T=<see cref="decimal"/>) <see cref="IList{T}"/>(T=<see cref="decimal"/>?) <see cref="List{T}"/>(T=<see cref="decimal"/>) <see cref="List{T}"/>(T=<see cref="decimal"/>?)</code>
    /// </summary>
    public sealed class StringifiedNumberListWithCommaSplitConverter : StringifiedNumberListWithSplitConverterBase
    {
        protected override string Separator { get { return ","; } }
    }
}
