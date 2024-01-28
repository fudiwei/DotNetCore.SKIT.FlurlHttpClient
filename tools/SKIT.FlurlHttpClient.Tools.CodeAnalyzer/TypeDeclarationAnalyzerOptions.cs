using System;
using System.Reflection;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    /// <summary>
    /// 类型声明质量分析器选项。
    /// </summary>
    public class TypeDeclarationAnalyzerOptions : IAnalyzerOptions
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
        /// 获取或设置表示 API 接口方法的类型声明所在的命名空间。
        /// <code>示例值：SKIT.FlurlHttpClient.MySdk</code>
        /// </summary>
        public string SdkExecutingExtensionDeclarationNamespace { get; set; }

        /// <summary>
        /// 获取或设置表示 API 接口方法的类型声明的匹配类名所使用的正则表达式（如设置将替换默认值）。
        /// </summary>
        public string? SdkExecutingExtensionDeclarationClassNameRegex { get; set; }

        /// <summary>
        /// 获取或设置表示 API 回调通知事件模型的类型声明所在的命名空间。
        /// <code>示例值：SKIT.FlurlHttpClient.MySdk.Events</code>
        /// </summary>
        public string SdkWebhookEventDeclarationNamespace { get; set; }

        /// <summary>
        /// 获取或设置要忽略的表示 API 请求模型的类型。
        /// </summary>
        public Func<Type, bool>? IgnoreRequestModelTypes { get; set; }

        /// <summary>
        /// 获取或设置要忽略的表示 API 响应模型的类型。
        /// </summary>
        public Func<Type, bool>? IgnoreResponseModelTypes { get; set; }

        /// <summary>
        /// 获取或设置要忽略的表示 API 接口方法的类型。
        /// </summary>
        public Func<Type, bool>? IgnoreExecutingExtensionTypes { get; set; }

        /// <summary>
        /// 获取或设置要忽略的表示 API 接口方法的函数。
        /// </summary>
        public Func<MethodInfo, bool>? IgnoreExecutingExtensionMethods { get; set; }

        /// <summary>
        /// 获取或设置要忽略的表示 API 回调通知事件模型的类型。
        /// </summary>
        public Func<Type, bool>? IgnoreWebhookEventTypes { get; set; }

        /// <summary>
        /// 获取或设置一个布尔值，该值指示是否在未找到表示 API 请求模型的类型声明时抛出异常。
        /// </summary>
        public bool ThrowOnNotFoundRequestModelTypes { get; set; }

        /// <summary>
        /// 获取或设置一个布尔值，该值指示是否在未找到表示 API 响应模型的类型声明时抛出异常。
        /// </summary>
        public bool ThrowOnNotFoundResponseModelTypes { get; set; }

        /// <summary>
        /// 获取或设置一个布尔值，该值指示是否在未找到表示 API 接口方法的类型声明时抛出异常。
        /// </summary>
        public bool ThrowOnNotFoundExecutingExtensionTypes { get; set; }

        /// <summary>
        /// 获取或设置一个布尔值，该值指示是否在未找到表示 API 回调通知事件模型的类型声明时抛出异常。
        /// </summary>
        public bool ThrowOnNotFoundWebhookEventTypes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TypeDeclarationAnalyzerOptions()
        {
            SdkAssembly = Assembly.GetExecutingAssembly()!;
            SdkRequestModelDeclarationNamespace = SdkAssembly.GetName().Name! + ".Models";
            SdkResponseModelDeclarationNamespace = SdkRequestModelDeclarationNamespace;
            SdkExecutingExtensionDeclarationNamespace = SdkAssembly.GetName().Name!;
            SdkWebhookEventDeclarationNamespace = SdkAssembly.GetName().Name! + ".Events";
        }
    }
}
