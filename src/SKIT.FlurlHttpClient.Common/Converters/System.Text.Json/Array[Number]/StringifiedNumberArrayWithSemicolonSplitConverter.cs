namespace System.Text.Json.Serialization.Common
{
    /// <summary>
    /// 一个 JSON 转换器，可针对指定适配类型做如下形式的对象转换。
    /// <code>
    ///   .NET → int[] Foo { get; } = new int[] { 1, 2 };
    ///   JSON → { "Foo": "1;2" }
    /// </code>
    /// 
    /// 适配类型：
    /// <code>  <see cref="sbyte"/>[] <see cref="sbyte"/>?[]</code>
    /// <code>  <see cref="byte"/>[] <see cref="byte"/>[]</code>
    /// <code>  <see cref="ushort"/>[] <see cref="ushort"/>?[]</code>
    /// <code>  <see cref="short"/>[] <see cref="short"/>?[]</code>
    /// <code>  <see cref="uint"/>[] <see cref="uint"/>?[]</code>
    /// <code>  <see cref="int"/>[] <see cref="int"/>?[]</code>
    /// <code>  <see cref="ulong"/>[] <see cref="ulong"/>?[]</code>
    /// <code>  <see cref="long"/>[] <see cref="long"/>?[]</code>
    /// <code>  <see cref="float"/>[] <see cref="float"/>?[]</code>
    /// <code>  <see cref="double"/>[] <see cref="double"/>?[]</code>
    /// <code>  <see cref="decimal"/>[] <see cref="decimal"/>?[]</code>
    /// </summary>
    public sealed class StringifiedNumberArrayWithSemicolonSplitConverter : StringifiedNumberArrayWithSplitConverterBase
    {
        protected override string Separator
        {
            get { return ";";  }
        }
    }
}
