using System;
using System.Collections.Generic;
using System.IO;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    /// <summary>
    /// 源文件质量分析器规则。
    /// </summary>
    /// <param name="options">分析器选项。</param>
    /// <param name="aggregation">待执行的全部分析单元。</param>
    /// <param name="current">当前执行的分析单元。</param>
    public delegate void SourceFileAnalyzerRule(
        SourceFileAnalyzerOptions options,
        IEnumerable<SourceFileAnalyzerRuleUnit> aggregation,
        SourceFileAnalyzerRuleUnit current
    );

    /// <summary>
    /// 源文件质量分析器最小分析单元。
    /// </summary>
    public record SourceFileAnalyzerRuleUnit
    {
        /// <summary>
        /// 获取文件类型。
        /// </summary>
        public SourceFileKinds FileKind { get; }

        /// <summary>
        /// 获取文件内容类型。
        /// </summary>
        public SourceFileContentKinds ContentKind { get; }

        /// <summary>
        /// 获取文件信息。
        /// </summary>
        public FileInfo FileInfo { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentKind"></param>
        /// <param name="fileInfo"></param>
        internal SourceFileAnalyzerRuleUnit(SourceFileKinds fileKind, SourceFileContentKinds contentKind, FileInfo fileInfo)
        {
            this.FileKind = fileKind;
            this.ContentKind = contentKind;
            this.FileInfo = fileInfo ?? throw new ArgumentNullException(nameof(fileInfo));
        }
    }
}
