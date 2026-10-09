using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    using SKIT.FlurlHttpClient.Internal.CodeAnalysis;

    /// <summary>
    /// 表示类型声明分析器的上下文信息。
    /// </summary>
    public sealed class TypeDeclarationAnalyzerContext
    {
        private readonly TypeDeclarationAnalyzerOptions _options;
        private readonly IReadOnlyList<TypeDeclarationAnalysisUnit> _units;

        /// <summary>
        /// 获取所属分析器的配置项。
        /// 注意这是一个浅克隆的副本。
        /// </summary>
        public TypeDeclarationAnalyzerOptions Options
        {
            get { return new TypeDeclarationAnalyzerOptions(_options); }
        }

        /// <summary>
        /// 获取当前分析的接口模型集合。
        /// </summary>
        public IEnumerable<TypeDeclarationAnalysisUnit> UnitsOfSDKModels
        {
            get { return _units.Where(unit => unit.Kind == TypeDeclarationKind.RequestModel || unit.Kind == TypeDeclarationKind.ResponseModel); }
        }

        /// <summary>
        /// 获取当前分析的接口方法集合。
        /// </summary>
        public IEnumerable<TypeDeclarationAnalysisUnit> UnitsOfSDKMethods
        {
            get { return _units.Where(unit => unit.Kind == TypeDeclarationKind.ExecutingMethod); }
        }

        /// <summary>
        /// 获取当前分析的回调通知事件集合。
        /// </summary>
        public IEnumerable<TypeDeclarationAnalysisUnit> UnitsOfSDKWebhooks
        {
            get { return _units.Where(unit => unit.Kind == TypeDeclarationKind.WebhookEvent); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="units"></param>
        internal TypeDeclarationAnalyzerContext(TypeDeclarationAnalyzerOptions options, IReadOnlyList<TypeDeclarationAnalysisUnit> units)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _units = units ?? throw new ArgumentNullException(nameof(units));
        }

        /// <summary>
        /// 判断指定分析单元是否禁用了指定规则。
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public bool IsRuleDisallowed(TypeDeclarationAnalysisUnit unit, string ruleId)
        {
            MemberInfo member = unit.MemberInfo;

            if (member.GetCustomAttributes<DisallowAnalyzerRuleAttribute>(inherit: true).Any(attribute => MatchRuleId(attribute.RuleId, ruleId)))
                return true;

            return member is MethodInfo method &&
                   method.DeclaringType is Type declaringType &&
                   declaringType.GetCustomAttributes<DisallowAnalyzerRuleAttribute>(inherit: true)
                       .Any(attribute => MatchRuleId(attribute.RuleId, ruleId));

            static bool MatchRuleId(string a, string b)
            {
                return string.Equals(a, b, StringComparison.OrdinalIgnoreCase) ||
                       string.Equals(a, $"{TypeDeclarationRules.RULE_ID_PREFIX}/{b}", StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
