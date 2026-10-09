using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    /// <summary>
    /// 静态文件分析器。用于分析 SDK 中的代码质量。
    /// </summary>
    public sealed class StaticFileAnalyzer
    {
        private readonly StaticFileAnalyzerOptions _options;
        private readonly List<(string Id, StaticFileRule Rule)> _rules = new();
        private readonly HashSet<string> _ruleIds = new(StringComparer.Ordinal);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public StaticFileAnalyzer(StaticFileAnalyzerOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _options.ProjectSDKModelsCodeFileNameMatchRegex ??= new Regex("(Request|Response)\\.cs$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            _options.ProjectSDKMethodsCodeFileNameMatchRegex ??= new Regex("[a-zA-Z0-9]+ClientExecute[a-zA-Z0-9]+Extensions\\.cs$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            _options.ProjectSDKWebhooksCodeFileNameMatchRegex ??= new Regex("\\.cs$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            if (_options.Assembly is null)
                throw new ArgumentException("The assembly to analyze cannot be null.", nameof(options));
            if (string.IsNullOrWhiteSpace(_options.SDKModelsNamespace) && _options.ReportOnMissingSDKModelsCodes)
                throw new ArgumentException("The SDK models namespace cannot be null or empty.", nameof(options));
            if (string.IsNullOrWhiteSpace(_options.SDKMethodsNamespace) && _options.ReportOnMissingSDKMethodsCodes)
                throw new ArgumentException("The SDK methods namespace cannot be null or empty.", nameof(options));
            if (string.IsNullOrWhiteSpace(_options.SDKWebhooksNamespace) && _options.ReportOnMissingSDKWebhooksCodes)
                throw new ArgumentException("The SDK webhooks namespace cannot be null or empty.", nameof(options));
            if (string.IsNullOrWhiteSpace(_options.ProjectRootPath))
                throw new ArgumentException("The project root path cannot be null or empty.", nameof(options));
            if (string.IsNullOrWhiteSpace(_options.ProjectSDKModelsCodeFolder) && _options.ReportOnMissingSDKModelsCodes)
                throw new ArgumentException("The project SDK models code folder cannot be null or empty.", nameof(options));
            if (string.IsNullOrWhiteSpace(_options.ProjectSDKMethodsCodeFolder) && _options.ReportOnMissingSDKMethodsCodes)
                throw new ArgumentException("The project SDK methods code folder cannot be null or empty.", nameof(options));
            if (string.IsNullOrWhiteSpace(_options.ProjectSDKWebhooksCodeFolder) && _options.ReportOnMissingSDKWebhooksCodes)
                throw new ArgumentException("The project SDK webhooks code folder cannot be null or empty.", nameof(options));
            if (string.IsNullOrWhiteSpace(_options.ProjectSDKModelsFixtureFolder) && _options.ReportOnMissingSDKModelsFixtures)
                throw new ArgumentException("The project SDK models fixtures path cannot be null or empty.", nameof(options));
            if (string.IsNullOrWhiteSpace(_options.ProjectSDKWebhooksFixtureFolder) && _options.ReportOnMissingSDKWebhooksFixtures)
                throw new ArgumentException("The project SDK webhooks fixtures path cannot be null or empty.", nameof(options));

            foreach ((string id, StaticFileRule rule) in StaticFileRules.Defaults)
            {
                AddRule(id, rule);
            }
        }

        /// <summary>
        /// 添加自定义规则。
        /// </summary>
        /// <param name="id">规则 ID。</param>
        /// <param name="rule">规则。</param>
        /// <returns></returns>
        public StaticFileAnalyzer AddRule(string id, StaticFileRule rule)
        {
            if (id is null) throw new ArgumentNullException(nameof(id));
            if (rule is null) throw new ArgumentNullException(nameof(rule));
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
        public IReadOnlyList<StaticFileAnalysisIssue> Analyze()
        {
            var temp = new List<StaticFileAnalysisIssue>();

            var sdkModelsCodes = ScanSDKModelsCodes();
            var sdkModelsFixtures = ScanSDKModelsFixtures();
            var sdkMethodsCodes = ScanSDKMethodsCodes();
            var sdkWebhooksCodes = ScanSDKWebhooksCodes();
            var sdkWebhooksFixtures = ScanSDKWebhooksFixtures();

            if (_options.ReportOnMissingSDKModelsCodes && sdkModelsCodes.Length == 0)
                temp.Add(new StaticFileAnalysisIssue($"{StaticFileRules.RULE_ID_PREFIX}/missing-models-codes", $"未在路径 \"{_options.GetProjectSDKModelsCodeFullPath()}\" 下扫描到表示 SDK 模型的源代码文件。"));
            if (_options.ReportOnMissingSDKModelsFixtures && sdkModelsFixtures.Length == 0)
                temp.Add(new StaticFileAnalysisIssue($"{StaticFileRules.RULE_ID_PREFIX}/missing-models-fixtures", $"未在路径 \"{_options.GetProjectSDKModelsFixtureFullPath()}\" 下扫描到表示 SDK 模型的样例文件。"));
            if (_options.ReportOnMissingSDKMethodsCodes && sdkMethodsCodes.Length == 0)
                temp.Add(new StaticFileAnalysisIssue($"{StaticFileRules.RULE_ID_PREFIX}/missing-methods-codes", $"未在路径 \"{_options.GetProjectSDKMethodsCodeFullPath()}\" 下扫描到表示 SDK 方法的源代码文件。"));
            if (_options.ReportOnMissingSDKWebhooksCodes && sdkWebhooksCodes.Length == 0)
                temp.Add(new StaticFileAnalysisIssue($"{StaticFileRules.RULE_ID_PREFIX}/missing-webhooks-codes", $"未在路径 \"{_options.GetProjectSDKWebhooksCodeFullPath()}\" 下扫描到表示 SDK 回调通知事件的源代码文件。"));
            if (_options.ReportOnMissingSDKWebhooksFixtures && sdkWebhooksFixtures.Length == 0)
                temp.Add(new StaticFileAnalysisIssue($"{StaticFileRules.RULE_ID_PREFIX}/missing-webhooks-fixtures", $"未在路径 \"{_options.GetProjectSDKWebhooksFixtureFullPath()}\" 下扫描到表示 SDK 回调通知事件的样例文件。"));

            var units = Array.Empty<StaticFileAnalysisUnit>()
                .Concat(sdkModelsCodes)
                .Concat(sdkModelsFixtures)
                .Concat(sdkMethodsCodes)
                .Concat(sdkWebhooksCodes)
                .Concat(sdkWebhooksFixtures)
                .ToArray();
            var context = new StaticFileAnalyzerContext(_options, units);

            foreach ((string id, StaticFileRule rule) in _rules)
            {
                if (_options.DisallowedRules is not null && _options.DisallowedRules.Contains(id))
                    continue;

                IEnumerable<StaticFileAnalysisIssue> issues = rule(context)
                    ?? throw new InvalidOperationException($"The rule \"{id}\" returns an invalid result.");
                foreach (StaticFileAnalysisIssue issue in issues)
                {
                    if (issue is null)
                        throw new InvalidOperationException($"The rule \"{id}\" returns a null issue.");
                    if (!string.IsNullOrEmpty(issue.RuleId) && !string.Equals(issue.RuleId, id, StringComparison.OrdinalIgnoreCase))
                        throw new InvalidOperationException($"The rule \"{id}\" returns an issue with a conflicting rule ID \"{issue.RuleId}\".");
                    else
                        issue.RuleId = id;

                    if (issue.IsGlobal || !context.IsRuleDisallowed(issue.Unit!, issue.RuleId))
                        temp.Add(issue);
                }
            }

            return temp.AsReadOnly();
        }

        private StaticFileAnalysisUnit[] ScanSDKModelsCodes()
        {
            string? path = _options.GetProjectSDKModelsCodeFullPath();
            if (path is null)
                return Array.Empty<StaticFileAnalysisUnit>();

            DirectoryInfo directory = new DirectoryInfo(path);
            if (!directory.Exists)
                return Array.Empty<StaticFileAnalysisUnit>();

            return directory
                .GetFiles("*", SearchOption.AllDirectories)
                .Where(file => _options.ProjectSDKModelsCodeFileNameMatchRegex is null ? true : _options.ProjectSDKModelsCodeFileNameMatchRegex.IsMatch(file.Name))
                .Where(file => _options.IgnoreFiles?.Invoke(file) != true)
                .Select(file =>
                {
                    if (Path.GetFileNameWithoutExtension(file.Name).EndsWith(Consts.REQUEST_NAMING_SUFFIX, StringComparison.Ordinal))
                        return new StaticFileAnalysisUnit(StaticFileKind.RequestModelCode, file);
                    if (Path.GetFileNameWithoutExtension(file.Name).EndsWith(Consts.RESPONSE_NAMING_SUFFIX, StringComparison.Ordinal))
                        return new StaticFileAnalysisUnit(StaticFileKind.ResponseModelCode, file);
                    return null;
                })
                .Where(unit => unit is not null)
                .Cast<StaticFileAnalysisUnit>()
                .ToArray();
        }

        private StaticFileAnalysisUnit[] ScanSDKModelsFixtures()
        {
            string? path = _options.GetProjectSDKModelsFixtureFullPath();
            if (path is null)
                return Array.Empty<StaticFileAnalysisUnit>();

            DirectoryInfo directory = new DirectoryInfo(path);
            if (!directory.Exists)
                return Array.Empty<StaticFileAnalysisUnit>();

            return directory
                .GetFiles("*", SearchOption.AllDirectories)
                .Where(file => _options.IgnoreFiles?.Invoke(file) != true)
                .Select(file =>
                {
                    if (Path.GetFileNameWithoutExtension(file.Name).EndsWith(Consts.REQUEST_NAMING_SUFFIX, StringComparison.Ordinal))
                        return new StaticFileAnalysisUnit(StaticFileKind.RequestModelFixture, file);
                    if (Path.GetFileNameWithoutExtension(file.Name).EndsWith(Consts.RESPONSE_NAMING_SUFFIX, StringComparison.Ordinal))
                        return new StaticFileAnalysisUnit(StaticFileKind.ResponseModelFixture, file);
                    return null;
                })
                .Where(unit => unit is not null)
                .Cast<StaticFileAnalysisUnit>()
                .ToArray();
        }

        private StaticFileAnalysisUnit[] ScanSDKMethodsCodes()
        {
            string? path = _options.GetProjectSDKMethodsCodeFullPath();
            if (path is null)
                return Array.Empty<StaticFileAnalysisUnit>();

            DirectoryInfo directory = new DirectoryInfo(path);
            if (!directory.Exists)
                return Array.Empty<StaticFileAnalysisUnit>();

            return directory
                .GetFiles("*", SearchOption.AllDirectories)
                .Where(file => _options.ProjectSDKMethodsCodeFileNameMatchRegex is null ? true : _options.ProjectSDKMethodsCodeFileNameMatchRegex.IsMatch(file.Name))
                .Where(file => _options.IgnoreFiles?.Invoke(file) != true)
                .Select(file => new StaticFileAnalysisUnit(StaticFileKind.ExecutingMethodCode, file))
                .ToArray();
        }

        private StaticFileAnalysisUnit[] ScanSDKWebhooksCodes()
        {
            string? path = _options.GetProjectSDKWebhooksCodeFullPath();
            if (path is null)
                return Array.Empty<StaticFileAnalysisUnit>();

            DirectoryInfo directory = new DirectoryInfo(path);
            if (!directory.Exists)
                return Array.Empty<StaticFileAnalysisUnit>();

            return directory
                .GetFiles("*", SearchOption.AllDirectories)
                .Where(file => _options.ProjectSDKWebhooksCodeFileNameMatchRegex is null ? true : _options.ProjectSDKWebhooksCodeFileNameMatchRegex.IsMatch(file.Name))
                .Where(file => _options.IgnoreFiles?.Invoke(file) != true)
                .Select(file => new StaticFileAnalysisUnit(StaticFileKind.WebhookEventCode, file))
                .ToArray();
        }

        private StaticFileAnalysisUnit[] ScanSDKWebhooksFixtures()
        {
            string? path = _options.GetProjectSDKWebhooksFixtureFullPath();
            if (path is null)
                return Array.Empty<StaticFileAnalysisUnit>();

            DirectoryInfo directory = new DirectoryInfo(path);
            if (!directory.Exists)
                return Array.Empty<StaticFileAnalysisUnit>();

            return directory
                .GetFiles("*", SearchOption.AllDirectories)
                .Where(file => _options.IgnoreFiles?.Invoke(file) != true)
                .Select(file => new StaticFileAnalysisUnit(StaticFileKind.WebhookEventFixture, file))
                .ToArray();
        }
    }
}
