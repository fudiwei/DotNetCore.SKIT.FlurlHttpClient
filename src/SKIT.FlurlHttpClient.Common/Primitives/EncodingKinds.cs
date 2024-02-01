namespace SKIT.FlurlHttpClient.Primitives
{
    /// <summary>
    /// 指定编码方式。
    /// </summary>
    public enum EncodingKinds
    {
        /// <summary>
        /// 未指定，取决于具体实现。
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// 未编码。
        /// </summary>
        Literal = 1,

        /// <summary>
        /// 十六进制编码。
        /// </summary>
        Hex = 2,

        /// <summary>
        /// Base64 编码。
        /// </summary>
        Base64 = 3
    }
}
