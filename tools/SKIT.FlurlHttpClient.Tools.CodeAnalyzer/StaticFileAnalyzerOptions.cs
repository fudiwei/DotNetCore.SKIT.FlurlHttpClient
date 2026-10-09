using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    /// <summary>
    /// 表示静态文件分析器的配置项。
    /// </summary>
    public class StaticFileAnalyzerOptions
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
        /// 获取或设置待分析的项目路径。
        /// <code>示例值："/path/to/src/SKIT.FlurlHttpClient.MySDK"</code>
        /// </summary>
        public string ProjectRootPath { get; set; }

        /// <summary>
        /// 获取或设置待分析的接口模型源代码在项目中所在的目录。
        /// <code>示例值："Models"</code>
        /// </summary>
        public string ProjectSDKModelsCodeFolder { get; set; }

        /// <summary>
        /// 获取或设置待分析的接口模型源代码匹配文件名所使用的正则表达式。
        /// 如果非空，将取代默认值。
        /// </summary>
        public Regex? ProjectSDKModelsCodeFileNameMatchRegex { get; set; }

        /// <summary>
        /// 获取或设置待分析的接口模型样例文件在项目中所在的目录。
        /// <code>示例值："ModelsFixtures"</code>
        /// </summary>
        public string? ProjectSDKModelsFixtureFolder { get; set; }

        /// <summary>
        /// 获取或设置待分析的接口方法源代码在项目中所在的目录。
        /// <code>示例值："Extensions"</code>
        /// </summary>
        public string ProjectSDKMethodsCodeFolder { get; set; }

        /// <summary>
        /// 获取或设置待分析的接口方法源代码匹配文件名所使用的正则表达式。
        /// 如果非空，将取代默认值。
        /// </summary>
        public Regex? ProjectSDKMethodsCodeFileNameMatchRegex { get; set; }

        /// <summary>
        /// 获取或设置待分析的回调通知事件源代码在项目中所在的目录。
        /// <code>示例值："Events"</code>
        /// </summary>
        public string ProjectSDKWebhooksCodeFolder { get; set; }

        /// <summary>
        /// 获取或设置待分析的回调通知事件源代码匹配文件名所使用的正则表达式。
        /// 如果非空，将取代默认值。
        /// </summary>
        public Regex? ProjectSDKWebhooksCodeFileNameMatchRegex { get; set; }

        /// <summary>
        /// 获取或设置待分析的回调通知事件样例文件在项目中所在的目录。
        /// <code>示例值："EventsFixtures"</code>
        /// </summary>
        public string? ProjectSDKWebhooksFixtureFolder { get; set; }

        /// <summary>
        /// 获取或设置一个函数，该函数用于判断是否忽略分析指定的文件。
        /// </summary>
        public Func<FileInfo, bool>? IgnoreFiles { get; set; }

        /// <summary>
        /// 获取或设置一个字符串数组，该数组用于指定要禁用的规则 ID。
        /// </summary>
        public string[]? DisallowedRules { get; set; }

        /// <summary>
        /// 获取或设置一个字典，该字典用于自定义指定规则的配置项。
        /// </summary>
        public Dictionary<string, Dictionary<string, object>>? CustomRuleConfigurations { get; set; }

        /// <summary>
        /// 获取或设置一个布尔值，该值指示是否在未找到表示接口模型的源代码文件时报告问题。
        /// </summary>
        public bool ReportOnMissingSDKModelsCodes { get; set; }

        /// <summary>
        /// 获取或设置一个布尔值，该值指示是否在未找到表示接口模型的样例文件时报告问题。
        /// </summary>
        public bool ReportOnMissingSDKModelsFixtures { get; set; }

        /// <summary>
        /// 获取或设置一个布尔值，该值指示是否在未找到表示接口方法的源代码文件时报告问题。
        /// </summary>
        public bool ReportOnMissingSDKMethodsCodes { get; set; }

        /// <summary>
        /// 获取或设置一个布尔值，该值指示是否在未找到表示回调通知事件的源代码文件时报告问题。
        /// </summary>
        public bool ReportOnMissingSDKWebhooksCodes { get; set; }

        /// <summary>
        /// 获取或设置一个布尔值，该值指示是否在未找到表示回调通知事件的样例文件时报告问题。
        /// </summary>
        public bool ReportOnMissingSDKWebhooksFixtures { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public StaticFileAnalyzerOptions()
        {
            Assembly = default!;
            SDKModelsNamespace = string.Empty;
            SDKMethodsNamespace = string.Empty;
            SDKWebhooksNamespace = string.Empty;
            ProjectRootPath = string.Empty;
            ProjectSDKModelsCodeFolder = "Models";
            ProjectSDKModelsFixtureFolder = "ModelsFixtures";
            ProjectSDKMethodsCodeFolder = "Extensions";
            ProjectSDKWebhooksCodeFolder = "Events";
            ProjectSDKWebhooksFixtureFolder = "EventsFixtures";
        }

        internal StaticFileAnalyzerOptions(StaticFileAnalyzerOptions options)
            : this()
        {
            if (options is null) throw new ArgumentNullException(nameof(options));

            Assembly = options.Assembly;
            SDKModelsNamespace = options.SDKModelsNamespace;
            SDKMethodsNamespace = options.SDKMethodsNamespace;
            SDKWebhooksNamespace = options.SDKWebhooksNamespace;
            ProjectRootPath = options.ProjectRootPath;
            ProjectSDKModelsCodeFolder = options.ProjectSDKModelsCodeFolder;
            ProjectSDKModelsCodeFileNameMatchRegex = options.ProjectSDKModelsCodeFileNameMatchRegex;
            ProjectSDKModelsFixtureFolder = options.ProjectSDKModelsFixtureFolder;
            ProjectSDKMethodsCodeFolder = options.ProjectSDKMethodsCodeFolder;
            ProjectSDKMethodsCodeFileNameMatchRegex = options.ProjectSDKMethodsCodeFileNameMatchRegex;
            ProjectSDKWebhooksCodeFolder = options.ProjectSDKWebhooksCodeFolder;
            ProjectSDKWebhooksCodeFileNameMatchRegex = options.ProjectSDKWebhooksCodeFileNameMatchRegex;
            ProjectSDKWebhooksFixtureFolder = options.ProjectSDKWebhooksFixtureFolder;
            IgnoreFiles = options.IgnoreFiles;
            DisallowedRules = (string[]?)options.DisallowedRules?.Clone();
            CustomRuleConfigurations = options.CustomRuleConfigurations is null ? null : new(options.CustomRuleConfigurations);
            ReportOnMissingSDKModelsCodes = options.ReportOnMissingSDKModelsCodes;
            ReportOnMissingSDKModelsFixtures = options.ReportOnMissingSDKModelsFixtures;
            ReportOnMissingSDKMethodsCodes = options.ReportOnMissingSDKMethodsCodes;
            ReportOnMissingSDKWebhooksCodes = options.ReportOnMissingSDKWebhooksCodes;
            ReportOnMissingSDKWebhooksFixtures = options.ReportOnMissingSDKWebhooksFixtures;
        }

        internal string? GetProjectSDKModelsCodeFullPath()
        {
            if (string.IsNullOrEmpty(ProjectRootPath))
                return null;

            string path = Path.Combine(ProjectRootPath, ProjectSDKModelsCodeFolder);
            return Path.GetFullPath(path);
        }

        internal string? GetProjectSDKModelsFixtureFullPath()
        {
            if (string.IsNullOrEmpty(ProjectRootPath))
                return null;

            string path = Path.Combine(ProjectRootPath, ProjectSDKModelsFixtureFolder ?? string.Empty);
            return Path.GetFullPath(path);
        }

        internal string? GetProjectSDKMethodsCodeFullPath()
        {
            if (string.IsNullOrEmpty(ProjectRootPath))
                return null;

            string path = Path.Combine(ProjectRootPath, ProjectSDKMethodsCodeFolder);
            return Path.GetFullPath(path);
        }

        internal string? GetProjectSDKWebhooksCodeFullPath()
        {
            if (string.IsNullOrEmpty(ProjectRootPath))
                return null;

            string path = Path.Combine(ProjectRootPath, ProjectSDKWebhooksCodeFolder);
            return Path.GetFullPath(path);
        }

        internal string? GetProjectSDKWebhooksFixtureFullPath()
        {
            if (string.IsNullOrEmpty(ProjectRootPath))
                return null;

            string path = Path.Combine(ProjectRootPath, ProjectSDKWebhooksFixtureFolder ?? string.Empty);
            return Path.GetFullPath(path);
        }

        /// <summary>
        /// 根据指定的 SDK 客户端类型加载默认配置项。
        /// </summary>
        /// <param name="sdkClientType"></param>
        /// <returns></returns>
        public static StaticFileAnalyzerOptions LoadFromClient(Type sdkClientType)
        {
            Assembly assembly = Assembly.GetAssembly(sdkClientType)!;
            return new StaticFileAnalyzerOptions()
            {
                Assembly = assembly,
                SDKModelsNamespace = $"{sdkClientType.Namespace}.Models",
                SDKMethodsNamespace = $"{sdkClientType.Namespace}",
                SDKWebhooksNamespace = $"{sdkClientType.Namespace}.Events"
            };
        }
    }
}
