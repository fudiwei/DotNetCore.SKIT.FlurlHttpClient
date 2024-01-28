namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    /// <summary>
    /// 源码文件类型。
    /// </summary>
    public enum SourceFileKinds
    {
        /// <summary>
        /// 未知。
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// C# 代码文件（*.cs）。
        /// </summary>
        CSharp = 1,

        /// <summary>
        /// JSON 文件（*.json）。
        /// </summary>
        Json = 11,

        /// <summary>
        /// XML 文件（*.xml）。
        /// </summary>
        Xml = 12
    }
}
