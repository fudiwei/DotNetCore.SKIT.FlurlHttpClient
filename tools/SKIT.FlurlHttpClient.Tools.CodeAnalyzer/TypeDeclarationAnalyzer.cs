using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    using SKIT.FlurlHttpClient.Tools.CodeAnalyzer.Helpers;

    /// <summary>
    /// 类型声明分析器。用于分析 SDK 中的类型声明问题。
    /// </summary>
    public sealed class TypeDeclarationAnalyzer
    {
        private readonly TypeDeclarationAnalyzerOptions _options;
        private readonly List<(string Id, TypeDeclarationRule Rule)> _rules = new();
        private readonly HashSet<string> _ruleIds = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public TypeDeclarationAnalyzer(TypeDeclarationAnalyzerOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));

            if (_options.Assembly is null)
                throw new ArgumentException("The assembly to analyze cannot be null.", nameof(options));
            if (string.IsNullOrWhiteSpace(_options.SDKModelsNamespace) && _options.ReportOnMissingSDKModels)
                throw new ArgumentException("The SDK models namespace cannot be null or empty.", nameof(options));
            if (string.IsNullOrWhiteSpace(_options.SDKMethodsNamespace) && _options.ReportOnMissingSDKMethods)
                throw new ArgumentException("The SDK methods namespace cannot be null or empty.", nameof(options));
            if (string.IsNullOrWhiteSpace(_options.SDKWebhooksNamespace) && _options.ReportOnMissingSDKWebhooks)
                throw new ArgumentException("The SDK webhooks namespace cannot be null or empty.", nameof(options));

            foreach ((string id, TypeDeclarationRule rule) in TypeDeclarationRules.Defaults)
            {
                AddRule(id, rule);
            }
        }

        /// <summary>
        /// 添加自定义规则。
        /// </summary>
        /// <param name="id">规则 ID。</param>
        /// <param name="rule">规则。</param>
        public TypeDeclarationAnalyzer AddRule(string id, TypeDeclarationRule rule)
        {
            if (id is null)
                throw new ArgumentNullException(nameof(id));
            if (rule is null)
                throw new ArgumentNullException(nameof(rule));
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("The rule ID cannot be null or empty.", nameof(id));
            if (!_ruleIds.Add(id))
                throw new ArgumentException($"The rule ID \"{id}\" already exists.", nameof(id));

            _rules.Add((id, rule));
            return this;
        }

        /// <summary>
        /// 执行分析，并返回发现的问题列表。
        /// </summary>
        public IReadOnlyList<TypeDeclarationAnalysisIssue> Analyze()
        {
            var temp = new List<TypeDeclarationAnalysisIssue>();

            var sdkModels = ScanSDKModels();
            var sdkMethods = ScanSDKMethods();
            var sdkWebhooks = ScanSDKWebhooks();

            if (_options.ReportOnMissingSDKModels && sdkModels.Length == 0)
                temp.Add(new TypeDeclarationAnalysisIssue($"{TypeDeclarationRules.RULE_ID_PREFIX}/missing-models", $"未在程序集 \"{_options.Assembly}\" 的命名空间 \"{_options.SDKModelsNamespace}\" 中扫描到表示 SDK 模型的类型声明。"));
            if (_options.ReportOnMissingSDKMethods && sdkMethods.Length == 0)
                temp.Add(new TypeDeclarationAnalysisIssue($"{TypeDeclarationRules.RULE_ID_PREFIX}/missing-methods", $"未在程序集 \"{_options.Assembly}\" 的命名空间 \"{_options.SDKMethodsNamespace}\" 中扫描到表示 SDK 方法的类型声明。"));
            if (_options.ReportOnMissingSDKWebhooks && sdkWebhooks.Length == 0)
                temp.Add(new TypeDeclarationAnalysisIssue($"{TypeDeclarationRules.RULE_ID_PREFIX}/missing-webhooks", $"未在程序集 \"{_options.Assembly}\" 的命名空间 \"{_options.SDKWebhooksNamespace}\" 中扫描到表示 SDK 回调通知事件的类型声明。"));

            var units = Array.Empty<TypeDeclarationAnalysisUnit>()
                .Concat(sdkModels)
                .Concat(sdkMethods)
                .Concat(sdkWebhooks)
                .ToArray();
            var context = new TypeDeclarationAnalyzerContext(_options, units);

            foreach ((string id, TypeDeclarationRule rule) in _rules)
            {
                if (_options.DisallowedRules is not null && _options.DisallowedRules.Contains(id))
                    continue;

                IEnumerable<TypeDeclarationAnalysisIssue> issues = rule(context)
                    ?? throw new InvalidOperationException($"The rule \"{id}\" returns an invalid result.");
                foreach (TypeDeclarationAnalysisIssue issue in issues)
                {
                    if (issue is null)
                        throw new InvalidOperationException($"The rule \"{id}\" returns a null issue.");
                    else if (!string.IsNullOrEmpty(issue.RuleId) && !string.Equals(issue.RuleId, id, StringComparison.OrdinalIgnoreCase))
                        throw new InvalidOperationException($"The rule \"{id}\" returns an issue with a conflicting rule ID \"{issue.RuleId}\".");
                    else
                        issue.RuleId = id;

                    if (issue.IsGlobal || !context.IsRuleDisallowed(issue.Unit!, issue.RuleId))
                        temp.Add(issue);
                }
            }

            return temp.AsReadOnly();
        }

        private TypeDeclarationAnalysisUnit[] ScanSDKModels()
        {
            if (string.IsNullOrEmpty(_options.SDKModelsNamespace))
                return Array.Empty<TypeDeclarationAnalysisUnit>();

            return ReflectionHelper.GetExportedTypes(_options.Assembly)
                .Where(type => type.IsNonAbstractClass() && type.IsNonNestedClass() && string.Equals(type.Namespace, _options.SDKModelsNamespace))
                .Where(type => _options.IgnoreTypes?.Invoke(type) != true)
                .Select(type =>
                {
                    if (typeof(ICommonRequest).IsAssignableFrom(type))
                        return new TypeDeclarationAnalysisUnit(TypeDeclarationKind.RequestModel, type);
                    if (typeof(ICommonResponse).IsAssignableFrom(type))
                        return new TypeDeclarationAnalysisUnit(TypeDeclarationKind.ResponseModel, type);
                    return null;
                })
                .Where(unit => unit is not null)
                .Cast<TypeDeclarationAnalysisUnit>()
                .ToArray();
        }

        private TypeDeclarationAnalysisUnit[] ScanSDKMethods()
        {
            if (string.IsNullOrEmpty(_options.SDKMethodsNamespace))
                return Array.Empty<TypeDeclarationAnalysisUnit>();

            return ReflectionHelper.GetExportedTypes(_options.Assembly)
                .Where(type => type.IsAbstract && type.IsStaticClass() && string.Equals(type.Namespace, _options.SDKMethodsNamespace))
                .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Static))
                .Where(method =>
                    _options.IgnoreMethods?.Invoke(method) != true &&
                    method.IsDefined(typeof(ExtensionAttribute), inherit: false) &&
                    method.Name.StartsWith(Consts.METHOD_NAMING_PREFIX, StringComparison.Ordinal) &&
                    method.Name.EndsWith(Consts.METHOD_NAMING_SUFFIX, StringComparison.Ordinal) &&
                    method.GetParameters().FirstOrDefault() is ParameterInfo clientParameter &&
                    typeof(ICommonClient).IsAssignableFrom(clientParameter.ParameterType)
                )
                .Where(method => _options.IgnoreMethods?.Invoke(method) != true)
                .Distinct()
                .Select(method => new TypeDeclarationAnalysisUnit(TypeDeclarationKind.ExecutingMethod, method))
                .ToArray();
        }

        private TypeDeclarationAnalysisUnit[] ScanSDKWebhooks()
        {
            if (string.IsNullOrEmpty(_options.SDKWebhooksNamespace))
                return Array.Empty<TypeDeclarationAnalysisUnit>();

            return ReflectionHelper.GetExportedTypes(_options.Assembly)
                .Where(type => type.IsNonAbstractClass() && type.IsNonNestedClass() && string.Equals(type.Namespace, _options.SDKWebhooksNamespace))
                .Where(type => _options.IgnoreTypes?.Invoke(type) != true)
                .Select(type =>
                {
                    if (typeof(ICommonWebhookEvent).IsAssignableFrom(type))
                        return new TypeDeclarationAnalysisUnit(TypeDeclarationKind.WebhookEvent, type);
                    return null;
                })
                .Where(unit => unit is not null)
                .Cast<TypeDeclarationAnalysisUnit>()
                .ToArray();
        }
    }
}
