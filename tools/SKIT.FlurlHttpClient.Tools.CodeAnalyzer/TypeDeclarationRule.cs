using System.Collections.Generic;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    /// <summary>
    /// 表示类型声明分析器的规则。
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public delegate IEnumerable<TypeDeclarationAnalysisIssue> TypeDeclarationRule(TypeDeclarationAnalyzerContext context);
}
