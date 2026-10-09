namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests
{
    using SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests.MockSDK;

    public class TestCase_StaticFileAnalyzerTest
    {
        private static StaticFileAnalyzerOptions BuildAnalyzerOptions()
        {
            string workdir = Environment.CurrentDirectory;
            string rootdir = Path.Combine(workdir, "../../../");

            var options = StaticFileAnalyzerOptions.LoadFromClient(typeof(MockClient));
            options.ProjectRootPath = rootdir;
            options.ProjectSDKModelsCodeFolder = "MockSDK/Models";
            options.ProjectSDKMethodsCodeFolder = "MockSDK/Extensions";
            options.ProjectSDKWebhooksCodeFolder = "MockSDK/Events";
            options.ProjectSDKModelsFixtureFolder = "MockSDK/ModelsFixtures";
            options.ProjectSDKWebhooksFixtureFolder = "MockSDK/EventsFixtures";
            options.ReportOnMissingSDKModelsCodes = true;
            options.ReportOnMissingSDKModelsFixtures = true;
            options.ReportOnMissingSDKMethodsCodes = true;
            options.ReportOnMissingSDKWebhooksCodes = true;

            return options;
        }

        [Fact(DisplayName = "测试用例：质量分析器之 StaticFileAnalyzer")]
        public void TestStaticFileAnalyzer()
        {
            var analyzer = new StaticFileAnalyzer(BuildAnalyzerOptions());

            var issues = analyzer.Analyze();

            Assert.Empty(issues);
        }

        [Fact(DisplayName = "测试用例：质量分析器之 StaticFileAnalyzer 自定义规则")]
        public void TestStaticFileAnalyzerCustomRule()
        {
            var analyzer = new StaticFileAnalyzer(BuildAnalyzerOptions());
            analyzer.AddRule("custom/example", _ => new[]
            {
                new StaticFileAnalysisIssue(string.Empty, "自定义问题")
            });

            var issues = analyzer.Analyze();

            var issue = Assert.Single(issues);
            Assert.Equal("custom/example", issue.RuleId);
            Assert.Equal("自定义问题", issue.Message);
            Assert.True(issue.IsGlobal);
        }
    }
}
