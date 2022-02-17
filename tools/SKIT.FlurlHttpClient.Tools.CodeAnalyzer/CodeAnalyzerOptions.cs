namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    public class CodeAnalyzerOptions
    {
        /// <summary>
        /// 获取或设置待分析的程序集名称。
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// 获取或设置源码工作目录。
        /// </summary>
        public string WorkDirectoryForSourceCode { get; set; }

        /// <summary>
        /// 获取或设置示例工作目录。
        /// </summary>
        public string WorkDirectoryForTestSample { get; set; }

        /// <summary>
        /// 获取或设置是否允许空的 API 模型类型；否则将在分析断言时抛出异常。
        /// </summary>
        public bool AllowNotFoundModelTypes { get; set; }

        /// <summary>
        /// 获取或设置是否允许空的 API 模型类型；否则将在分析断言时抛出异常。
        /// </summary>
        public bool AllowNotFoundEventTypes { get; set; }

        /// <summary>
        /// 获取或设置是否允许空的 API 模型示例文件；否则将在分析断言时抛出异常。
        /// </summary>
        public bool AllowNotFoundModelSamples { get; set; }

        /// <summary>
        /// 获取或设置是否允许空的 API 事件示例文件；否则将在分析断言时抛出异常。
        /// </summary>
        public bool AllowNotFoundEventSamples { get; set; }

        public CodeAnalyzerOptions()
        {
            AssemblyName = string.Empty;
            WorkDirectoryForSourceCode = string.Empty;
            WorkDirectoryForTestSample = string.Empty;
        }
    }
}
