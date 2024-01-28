using System;
using System.IO;
using System.Reflection;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    /// <summary>
    /// 源文件质量分析器选项。
    /// </summary>
    public class SourceFileAnalyzerOptions : IAnalyzerOptions
    {
        /// <inheritdoc/>
        public Assembly SdkAssembly { get; set; }

        /// <summary>
        /// 获取或设置表示 API 请求模型的类型声明所在的命名空间。
        /// <code>示例值：SKIT.FlurlHttpClient.MySdk.Models</code>
        /// </summary>
        public string SdkRequestModelDeclarationNamespace { get; set; }

        /// <summary>
        /// 获取或设置表示 API 响应模型的类型声明所在的命名空间。
        /// <code>示例值：SKIT.FlurlHttpClient.MySdk.Models</code>
        /// </summary>
        public string SdkResponseModelDeclarationNamespace { get; set; }

        /// <summary>
        /// 获取或设置表示 API 回调通知事件模型的类型声明所在的命名空间。
        /// <code>示例值：SKIT.FlurlHttpClient.MySdk.Events</code>
        /// </summary>
        public string SdkWebhookEventDeclarationNamespace { get; set; }

        /// <summary>
        /// 获取或设置源码项目根目录。
        /// <code>示例值：/path/to/src/SKIT.FlurlHttpClient.MySdk</code>
        /// </summary>
        public string ProjectSourceRootDirectory { get; set; }

        /// <summary>
        /// 获取或设置源码项目中用于存放 API 请求模型类的代码文件的子目录。
        /// <code>示例值：Models</code>
        /// </summary>
        public string ProjectSourceRequestModelClassCodeSubDirectory { get; set; }

        /// <summary>
        /// 获取或设置源码项目中用于存放 API 响应模型类的代码文件的子目录。
        /// <code>示例值：Models</code>
        /// </summary>
        public string ProjectSourceResponseModelClassCodeSubDirectory { get; set; }

        /// <summary>
        /// 获取或设置源码项目中用于存放 API 接口方法类的代码文件的子目录。
        /// <code>示例值：Extensions</code>
        /// </summary>
        public string ProjectSourceExecutingExtensionClassCodeSubDirectory { get; set; }

        /// <summary>
        /// 获取或设置源码项目中用于存放 API 接口方法类的代码文件匹配文件名所使用的正则表达式（如设置将替换默认值）。
        /// </summary>
        public string? ProjectSourceExecutingExtensionClassCodeFileNameRegex { get; set; }

        /// <summary>
        /// 获取或设置源码项目中用于存放 API 回调通知事件模型类的代码文件的子目录。
        /// <code>示例值：Events</code>
        /// </summary>
        public string ProjectSourceWebhookEventClassCodeSubDirectory { get; set; }

        /// <summary>
        /// 获取或设置测试项目根目录。
        /// <code>示例值：/path/to/tests/SKIT.FlurlHttpClient.MySdk</code>
        /// </summary>
        public string ProjectTestRootDirectory { get; set; }

        /// <summary>
        /// 获取或设置测试项目中用于存放 API 请求模型类的序列化后的样例文件的子目录。
        /// <code>示例值：ModelSamples</code>
        /// </summary>
        public string ProjectTestRequestModelSerializationSampleSubDirectory { get; set; }

        /// <summary>
        /// 获取或设置测试项目中用于存放 API 响应模型类的序列化后的样例文件的子目录。
        /// <code>示例值：ModelSamples</code>
        /// </summary>
        public string ProjectTestResponseModelSerializationSampleSubDirectory { get; set; }

        /// <summary>
        /// 获取或设置测试项目中用于存放 API 请求或响应模型类的序列化后的样例文件的子目录。
        /// <code>示例值：EventSamples</code>
        /// </summary>
        public string ProjectTestWebhookEventSerializationSampleSubDirectory { get; set; }

        /// <summary>
        /// 获取或设置要忽略的表示 API 请求模型类的代码文件。
        /// </summary>
        public Func<FileInfo, bool>? IgnoreRequestModelClassCodeFiles { get; set; }

        /// <summary>
        /// 获取或设置要忽略的表示 API 响应模型类的代码文件。
        /// </summary>
        public Func<FileInfo, bool>? IgnoreResponseModelClassCodeFiles { get; set; }

        /// <summary>
        /// 获取或设置要忽略的表示 API 接口方法类的代码文件。
        /// </summary>
        public Func<FileInfo, bool>? IgnoreExecutingExtensionClassCodeFiles { get; set; }

        /// <summary>
        /// 获取或设置要忽略的表示 API 回调通知事件模型类的代码文件。
        /// </summary>
        public Func<FileInfo, bool>? IgnoreWebhookEventClassCodeFiles { get; set; }

        /// <summary>
        /// 获取或设置要忽略的表示 API 请求模型类的序列化后的样例文件。
        /// </summary>
        public Func<FileInfo, bool>? IgnoreRequestModelSerializationSampleFiles { get; set; }

        /// <summary>
        /// 获取或设置要忽略的表示 API 响应模型类的序列化后的样例文件。
        /// </summary>
        public Func<FileInfo, bool>? IgnoreResponseModelSerializationSampleFiles { get; set; }

        /// <summary>
        /// 获取或设置要忽略的表示 API 回调通知事件模型类的序列化后的样例文件。
        /// </summary>
        public Func<FileInfo, bool>? IgnoreWebhookEventSerializationSampleFiles { get; set; }

        /// <summary>
        /// 获取或设置一个布尔值，该值指示是否在未找到表示 API 请求模型类的代码文件时抛出异常。
        /// </summary>
        public bool ThrowOnNotFoundRequestModelClassCodeFiles { get; set; }

        /// <summary>
        /// 获取或设置一个布尔值，该值指示是否在未找到表示 API 响应模型类的代码文件时抛出异常。
        /// </summary>
        public bool ThrowOnNotFoundResponseModelClassCodeFiles { get; set; }

        /// <summary>
        /// 获取或设置一个布尔值，该值指示是否在未找到表示 API 接口方法类的代码文件时抛出异常。
        /// </summary>
        public bool ThrowOnNotFoundExecutingExtensionClassCodeFiles { get; set; }

        /// <summary>
        /// 获取或设置一个布尔值，该值指示是否在未找到表示 API 回调通知事件模型类的代码文件时抛出异常。
        /// </summary>
        public bool ThrowOnNotFoundWebhookEventClassCodeFiles { get; set; }

        /// <summary>
        /// 获取或设置一个布尔值，该值指示是否在未找到表示 API 请求模型类的序列化后的样例文件时抛出异常。
        /// </summary>
        public bool ThrowOnNotFoundRequestModelSerializationSampleFiles { get; set; }

        /// <summary>
        /// 获取或设置一个布尔值，该值指示是否在未找到表示 API 响应模型类的序列化后的样例文件时抛出异常。
        /// </summary>
        public bool ThrowOnNotFoundResponseModelSerializationSampleFiles { get; set; }

        /// <summary>
        /// 获取或设置一个布尔值，该值指示是否在未找到表示 API 回调通知事件模型类的序列化后的样例文件时抛出异常。
        /// </summary>
        public bool ThrowOnNotFoundWebhookEventSerializationSampleFiles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SourceFileAnalyzerOptions()
        {
            SdkAssembly = Assembly.GetExecutingAssembly();
            SdkRequestModelDeclarationNamespace = SdkAssembly.GetName().Name! + ".Models";
            SdkResponseModelDeclarationNamespace = SdkRequestModelDeclarationNamespace;
            SdkWebhookEventDeclarationNamespace = SdkAssembly.GetName().Name! + ".Events";
            ProjectSourceRootDirectory = string.Empty;
            ProjectSourceRequestModelClassCodeSubDirectory = "Models";
            ProjectSourceResponseModelClassCodeSubDirectory = ProjectSourceRequestModelClassCodeSubDirectory;
            ProjectSourceExecutingExtensionClassCodeSubDirectory = "Extensions";
            ProjectSourceWebhookEventClassCodeSubDirectory = "Events";
            ProjectTestRootDirectory = string.Empty;
            ProjectTestRequestModelSerializationSampleSubDirectory = "ModelSamples";
            ProjectTestResponseModelSerializationSampleSubDirectory = ProjectTestRequestModelSerializationSampleSubDirectory;
            ProjectTestWebhookEventSerializationSampleSubDirectory = "EventSamples";
        }
    }
}
