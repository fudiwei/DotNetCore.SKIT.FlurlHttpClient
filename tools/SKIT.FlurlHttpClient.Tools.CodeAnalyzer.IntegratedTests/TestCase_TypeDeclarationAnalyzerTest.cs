namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests
{
    using SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests.MockSDK;
    using SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests.MockSDK.Models;

    public class TestCase_TypeDeclarationAnalyzerTest
    {
        private static TypeDeclarationAnalyzerOptions BuildAnalyzerOptions()
        {
            var options = TypeDeclarationAnalyzerOptions.LoadFromClient(typeof(MockClient));
            options.ReportOnMissingSDKModels = true;
            options.ReportOnMissingSDKMethods = true;
            options.ReportOnMissingSDKWebhooks = true;

            return options;
        }

        [Fact(DisplayName = "测试用例：类型声明分析器")]
        public void TestAnalyzer_Analyze()
        {
            var analyzer = new TypeDeclarationAnalyzer(BuildAnalyzerOptions());

            var issues = analyzer.Analyze();

            Assert.Empty(issues);
        }

        [Fact(DisplayName = "测试用例：类型声明分析器之自定义规则与过滤")]
        public void TestAnalyzer_AddRule_CustomRulesAndFilters()
        {
            TypeDeclarationAnalyzerOptions options = BuildAnalyzerOptions();
            options.IgnoreTypes = type => type == typeof(GetFooRequest);
            options.IgnoreMethods = method => method.Name == "ExecutePostFooAsync";

            var analyzer = new TypeDeclarationAnalyzer(options);
            analyzer.AddRule("CUSTOM", context =>
                Array.Empty<TypeDeclarationAnalysisIssue>()
                    .Concat(context.UnitsOfSDKModels.Select(unit => new TypeDeclarationAnalysisIssue("CUSTOM", "model", unit)))
                    .Concat(context.UnitsOfSDKMethods.Select(unit => new TypeDeclarationAnalysisIssue("CUSTOM", "method", unit)))
            );

            var issues = analyzer.Analyze();

            Assert.NotNull(issues);
            Assert.DoesNotContain(issues, issue => issue.Unit?.Kind == TypeDeclarationKind.RequestModel && issue.Unit?.GetMemberAsType() == typeof(GetFooRequest));
            Assert.DoesNotContain(issues, issue => issue.Unit?.Kind == TypeDeclarationKind.ExecutingMethod && issue.Unit?.GetMemberAsMethodInfo()?.Name == "ExecutePostFooAsync");
            Assert.Contains(issues, issue => issue.Unit?.Kind == TypeDeclarationKind.ExecutingMethod && issue.Unit?.GetMemberAsMethodInfo()?.Name == "ExecuteGetFooAsync");
        }

        [Fact(DisplayName = "测试用例：类型声明分析器之规则 ID 冲突")]
        public void TestAnalyzer_AddRule_DuplicateIds()
        {
            var analyzer = new TypeDeclarationAnalyzer(BuildAnalyzerOptions());

            TypeDeclarationRule rule = _ => Array.Empty<TypeDeclarationAnalysisIssue>();
            analyzer.AddRule("CUSTOM", rule);
            analyzer.AddRule("CUSTOM_ANOTHER", rule);

            Assert.Throws<ArgumentException>(() => analyzer.AddRule("CUSTOM", rule));
        }
    }
}
