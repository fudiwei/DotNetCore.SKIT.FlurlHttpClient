using System;
using System.IO;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    /// <summary>
    /// 表示静态文件分析器的最小分析单元，通常对应一个具体的类型或方法。
    /// </summary>
    public record StaticFileAnalysisUnit
    {
        /// <summary>
        /// 获取当前分析单元的种类。
        /// </summary>
        public StaticFileKind Kind { get; }

        /// <summary>
        /// 获取当前分析单元的文件信息。
        /// </summary>
        public FileInfo FileInfo { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="fileInfo"></param>
        public StaticFileAnalysisUnit(StaticFileKind kind, FileInfo fileInfo)
        {
            Kind = kind;
            FileInfo = fileInfo ?? throw new ArgumentNullException(nameof(fileInfo));

            if (!Enum.IsDefined(typeof(StaticFileKind), kind) || kind == StaticFileKind.Unknown)
                throw new ArgumentOutOfRangeException(nameof(kind), $"The static file kind \"{kind}\" is not supported.");
        }
    }
}
