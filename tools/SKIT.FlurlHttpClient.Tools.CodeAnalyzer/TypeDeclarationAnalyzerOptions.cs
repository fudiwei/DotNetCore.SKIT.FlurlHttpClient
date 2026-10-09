using System;
using System.Reflection;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    /// <summary>
    /// 表示类型声明分析器的配置项。
    /// </summary>
    public sealed class TypeDeclarationAnalyzerOptions
    {
        /// <summary>
        /// 获取或设置待分析的程序集。
        /// </summary>
        public Assembly Assembly { get; set; }

        /// <summary>
        /// 获取或设置待分析的接口模型所在的命名空间。
        /// <code>示例值："SKIT.FlurlHttpClient.MySDK.Models"</code>
        /// </summary>
        public string SDKModelsNamespace { get; set; }

        /// <summary>
        /// 获取或设置当前分析的接口方法所在的命名空间。
        /// <code>示例值："SKIT.FlurlHttpClient.MySDK"</code>
        /// </summary>
        public string SDKMethodsNamespace { get; set; }

        /// <summary>
        /// 获取或设置待分析的回调通知事件所在的命名空间。
        /// <code>示例值："SKIT.FlurlHttpClient.MySDK.Events"</code>
        /// </summary>
        public string SDKWebhooksNamespace { get; set; }

        /// <summary>
        /// 获取或设置一个函数，该函数用于判断是否忽略分析指定的类型。
        /// </summary>
        public Func<Type, bool>? IgnoreTypes { get; set; }

        /// <summary>
        /// 获取或设置一个函数，该函数用于判断是否忽略分析指定的方法。
        /// </summary>
        public Func<MethodInfo, bool>? IgnoreMethods { get; set; }

        /// <summary>
        /// 获取或设置一个字符串数组，该数组用于指定要禁用的规则 ID。
        /// </summary>
        public string[]? DisallowedRules { get; set; }

        /// <summary>
        /// 获取或设置一个布尔值，该值指示是否在未找到表示接口模型的类型声明时报告问题。
        /// </summary>
        public bool ReportOnMissingSDKModels { get; set; }

        /// <summary>
        /// 获取或设置一个布尔值，该值指示是否在未找到表示接口方法的类型声明时报告问题。
        /// </summary>
        public bool ReportOnMissingSDKMethods { get; set; }

        /// <summary>
        /// 获取或设置一个布尔值，该值指示是否在未找到表示回调通知事件的类型声明时报告问题。
        /// </summary>
        public bool ReportOnMissingSDKWebhooks { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TypeDeclarationAnalyzerOptions()
        {
            Assembly = default!;
            SDKModelsNamespace = string.Empty;
            SDKMethodsNamespace = string.Empty;
            SDKWebhooksNamespace = string.Empty;
        }

        internal TypeDeclarationAnalyzerOptions(TypeDeclarationAnalyzerOptions options)
            : this()
        {
            if (options is null) throw new ArgumentNullException(nameof(options));

            Assembly = options.Assembly;
            SDKModelsNamespace = options.SDKModelsNamespace;
            SDKMethodsNamespace = options.SDKMethodsNamespace;
            SDKWebhooksNamespace = options.SDKWebhooksNamespace;
            IgnoreTypes = options.IgnoreTypes;
            IgnoreMethods = options.IgnoreMethods;
            DisallowedRules = (string[]?)options.DisallowedRules?.Clone();
            ReportOnMissingSDKModels = options.ReportOnMissingSDKModels;
            ReportOnMissingSDKMethods = options.ReportOnMissingSDKMethods;
            ReportOnMissingSDKWebhooks = options.ReportOnMissingSDKWebhooks;
        }

        /// <summary>
        /// 根据指定的 SDK 客户端类型加载默认配置项。
        /// </summary>
        /// <param name="sdkClientType"></param>
        /// <returns></returns>
        public static TypeDeclarationAnalyzerOptions LoadFromClient(Type sdkClientType)
        {
            Assembly assembly = Assembly.GetAssembly(sdkClientType)!;
            return new TypeDeclarationAnalyzerOptions()
            {
                Assembly = assembly,
                SDKModelsNamespace = $"{sdkClientType.Namespace}.Models",
                SDKMethodsNamespace = $"{sdkClientType.Namespace}",
                SDKWebhooksNamespace = $"{sdkClientType.Namespace}.Events"
            };
        }
    }
}
