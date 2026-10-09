using System;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    /// <summary>
    /// 表示静态文件分析器发现的问题。
    /// </summary>
    public sealed class StaticFileAnalysisIssue
    {
        /// <summary>
        /// 获取规则 ID。
        /// </summary>
        public string RuleId { get; internal set; }

        /// <summary>
        /// 获取问题错误消息。
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// 获取问题对应的分析单元。
        /// </summary>
        public StaticFileAnalysisUnit? Unit { get; }

        /// <summary>
        /// 获取一个布尔值，指示是否为全局问题（即不属于任何单元）。
        /// </summary>
        public bool IsGlobal { get { return Unit is null; } }

        /// <summary>
        /// 
        /// </summary>
        public StaticFileAnalysisIssue(string ruleId, string message)
        {
            RuleId = ruleId ?? throw new ArgumentNullException(nameof(ruleId));
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        /// <summary>
        /// 
        /// </summary>
        public StaticFileAnalysisIssue(string ruleId, string message, StaticFileAnalysisUnit unit)
        {
            RuleId = ruleId ?? throw new ArgumentNullException(nameof(ruleId));
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Unit = unit ?? throw new ArgumentNullException(nameof(unit));
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Unit is null
                ? $"[{RuleId}] {Message}"
                : $"[{RuleId}] {Unit.FileInfo.FullName}: {Message}";
        }
    }
}
