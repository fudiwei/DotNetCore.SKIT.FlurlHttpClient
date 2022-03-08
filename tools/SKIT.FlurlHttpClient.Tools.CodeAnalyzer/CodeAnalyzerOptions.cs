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

        /// 获取或设置源码工作目录下的 API 模型代码文件子目录。
        /// <para>默认值："Models"</para>
        public string WorkSubDirectoryForApiModels { get; set; }

        /// 获取或设置源码工作目录下的 API 事件代码文件子目录。
        /// <para>默认值："Events"</para>
        public string WorkSubDirectoryForApiEvents { get; set; }

        /// 获取或设置源码工作目录下的 API 方法代码文件子目录。
        /// <para>默认值："Extensions"</para>
        public string WorkSubDirectoryForApiMethods { get; set; }

        /// <summary>
        /// 获取或设置示例工作目录。
        /// </summary>
        public string WorkDirectoryForTestSample { get; set; }

        /// 获取或设置示例工作目录下的 API 模型示例文件子目录。
        /// <para>默认值："ModelSamples"</para>
        public string WorkSubDirectoryForApiModelSamples { get; set; }

        /// 获取或设置示例工作目录下的 API 事件示例文件子目录。
        /// <para>默认值："EventSamples"</para>
        public string WorkSubDirectoryForApiEventSamples { get; set; }

        /// <summary>
        /// 获取或设置每个 API 模型的内嵌类型包装标识。
        /// <para>默认值："Types"</para>
        /// </summary>
        public string TargetSdkApiModelInnerNestedTypesWrapperIdentifier { get; set; }

        /// <summary>
        /// 获取或设置每个 API 模型的程序集下级命名空间标识。
        /// <para>默认值："Models"</para>
        /// </summary>
        public string TargetSdkApiModelNamespaceUnderAssemblyIdentifier { get; set; }

        /// <summary>
        /// 获取或设置每个 API 事件的程序集下级命名空间标识。
        /// <para>默认值："Events"</para>
        /// </summary>
        public string TargetSdkApiEventNamespaceUnderAssemblyIdentifier { get; set; }

        /// <summary>
        /// 获取或设置每个 API 方法的程序集下级命名空间标识。
        /// <para>默认值：""</para>
        /// </summary>
        public string TargetSdkApiMethodNamespaceUnderAssemblyIdentifier { get; set; }

        /// <summary>
        /// 获取或设置每个 API 方法的 FlurlRequest 构造方法标识。
        /// <para>默认值："CreateRequest"</para>
        /// </summary>
        public string TargetSdkApiMethodInnerFlurlRequestInitializerIdentifier { get; set; }

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
            WorkSubDirectoryForApiModels = "Models";
            WorkSubDirectoryForApiEvents = "Events";
            WorkSubDirectoryForApiMethods = "Extensions";
            WorkSubDirectoryForApiModelSamples = "ModelSamples";
            WorkSubDirectoryForApiEventSamples = "EventSamples";
            TargetSdkApiModelInnerNestedTypesWrapperIdentifier = "Types";
            TargetSdkApiModelNamespaceUnderAssemblyIdentifier = "Models";
            TargetSdkApiEventNamespaceUnderAssemblyIdentifier = "Events";
            TargetSdkApiMethodNamespaceUnderAssemblyIdentifier = "";
            TargetSdkApiMethodInnerFlurlRequestInitializerIdentifier = "CreateRequest";
        }
    }
}
