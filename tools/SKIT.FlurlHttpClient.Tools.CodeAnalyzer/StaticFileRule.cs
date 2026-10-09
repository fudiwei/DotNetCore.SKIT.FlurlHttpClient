using System.Collections.Generic;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    /// <summary>
    /// 表示静态文件分析器的规则。
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public delegate IEnumerable<StaticFileAnalysisIssue> StaticFileRule(StaticFileAnalyzerContext context);
}
