using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.CodeAnalysis;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    using SKIT.FlurlHttpClient.Internal.CodeAnalysis;

    /// <summary>
    /// 表示静态文件分析器的上下文信息。
    /// </summary>
    public sealed class StaticFileAnalyzerContext
    {
        private readonly StaticFileAnalyzerOptions _options;
        private readonly IReadOnlyList<StaticFileAnalysisUnit> _units;
        private readonly ConcurrentDictionary<string, string> _filePathToContentMap;

        /// <summary>
        /// 获取所属分析器的配置项。
        /// 注意这是一个浅克隆的副本。
        /// </summary>
        public StaticFileAnalyzerOptions Options
        {
            get { return new StaticFileAnalyzerOptions(_options); }
        }

        /// <summary>
        /// 获取当前分析的接口模型源代码集合。
        /// </summary>
        public IEnumerable<StaticFileAnalysisUnit> UnitsOfSDKModelsCodes
        {
            get { return _units.Where(unit => unit.Kind == StaticFileKind.RequestModelCode || unit.Kind == StaticFileKind.ResponseModelCode); }
        }

        /// <summary>
        /// 获取当前分析的接口模型样例集合。
        /// </summary>
        public IEnumerable<StaticFileAnalysisUnit> UnitsOfSDKModelsFixtures
        {
            get { return _units.Where(unit => unit.Kind == StaticFileKind.RequestModelFixture || unit.Kind == StaticFileKind.ResponseModelFixture); }
        }

        /// <summary>
        /// 获取当前分析的接口方法源代码集合。
        /// </summary>
        public IEnumerable<StaticFileAnalysisUnit> UnitsOfSDKMethodsCodes
        {
            get { return _units.Where(unit => unit.Kind == StaticFileKind.ExecutingMethodCode); }
        }

        /// <summary>
        /// 获取当前分析的回调通知事件源代码集合。
        /// </summary>
        public IEnumerable<StaticFileAnalysisUnit> UnitsOfSDKWebhooksCodes
        {
            get { return _units.Where(unit => unit.Kind == StaticFileKind.WebhookEventCode); }
        }

        /// <summary>
        /// 获取当前分析的回调通知事件样例集合。
        /// </summary>
        public IEnumerable<StaticFileAnalysisUnit> UnitsOfSDKWebhooksFixtures
        {
            get { return _units.Where(unit => unit.Kind == StaticFileKind.WebhookEventFixture); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="units"></param>
        internal StaticFileAnalyzerContext(StaticFileAnalyzerOptions options, IReadOnlyList<StaticFileAnalysisUnit> units)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _units = units ?? throw new ArgumentNullException(nameof(units));
            _filePathToContentMap = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 读取指定分析单元的文件内容。
        /// </summary>
        public string ReadFile(StaticFileAnalysisUnit unit)
        {
            return _filePathToContentMap.GetOrAdd(unit.FileInfo.FullName, static path =>
            {
                const int MAX_PATH = 260;

                if (path.Length >= MAX_PATH)
                {
                    bool isOnWindows =
#if NET471_OR_GREATER || NETSTANDARD
                        RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#else
                        Environment.OSVersion.Platform == PlatformID.Win32NT;
#endif
                    if (isOnWindows && path.StartsWith(@"\\"))
                    {
                        path = @"\\?\UNC\" + path.Substring(2);
                    }
                    else if (isOnWindows && !path.StartsWith(@"\\?\"))
                    {
                        path = @"\\?\" + path;
                    }
                }

                return File.ReadAllText(path, Encoding.UTF8);
            });
        }

        /// <summary>
        /// 判断指定分析单元是否禁用了指定规则。
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public bool IsRuleDisallowed(StaticFileAnalysisUnit unit, string ruleId)
        {
            MemberInfo? member = null;

            switch (unit.Kind)
            {
                case StaticFileKind.RequestModelCode:
                case StaticFileKind.ResponseModelCode:
                    {
                        string className = Path.GetFileNameWithoutExtension(unit.FileInfo.Name);
                        string typeName = $"{_options.SDKModelsNamespace}.{className}";
                        Type? type = _options.Assembly.GetType(typeName, throwOnError: false, ignoreCase: true);
                        member = type;
                    }
                    break;

                case StaticFileKind.WebhookEventCode:
                    {
                        string className = Path.GetFileNameWithoutExtension(unit.FileInfo.Name);
                        string typeName = $"{_options.SDKWebhooksNamespace}.{className}";
                        Type? type = _options.Assembly.GetType(typeName, throwOnError: false, ignoreCase: true);
                        member = type;
                    }
                    break;
            }

            if (member is null)
                return false;

            if (member.GetCustomAttributes<DisallowAnalyzerRuleAttribute>(inherit: true).Any(attribute => MatchRuleId(attribute.RuleId, ruleId)))
                return true;

            return false;

            static bool MatchRuleId(string a, string b)
            {
                return string.Equals(a, b, StringComparison.OrdinalIgnoreCase) ||
                       string.Equals(a, $"{StaticFileRules.RULE_ID_PREFIX}/{b}", StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
