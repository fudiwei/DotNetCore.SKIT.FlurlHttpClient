namespace SKIT.FlurlHttpClient
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
        /// 十六进制编码。
        /// </summary>
        Hex = 1,

        /// <summary>
        /// Base64 编码。
        /// </summary>
        Base64 = 2
    }
}
