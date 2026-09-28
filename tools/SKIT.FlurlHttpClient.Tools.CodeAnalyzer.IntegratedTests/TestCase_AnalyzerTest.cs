using System.Reflection;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests
{
    using SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests.MockSdk;

    public class TestCase_AnalyzerTest
    {
        [Fact(DisplayName = "测试用例：质量分析器之 TypeDeclarationAnalyzer")]
        public void TestTypeDeclarationAnalyzer()
        {
            Exception? exception = Record.Exception(() =>
            {
                IAnalyzer analyzer = new TypeDeclarationAnalyzer(new TypeDeclarationAnalyzerOptions()
                {
                    SdkAssembly = Assembly.GetAssembly(typeof(MockClient))!,
                    SdkRequestModelDeclarationNamespace = "SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests.MockSdk.Models",
                    SdkResponseModelDeclarationNamespace = "SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests.MockSdk.Models",
                    SdkExecutingExtensionDeclarationNamespace = "SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests.MockSdk",
                    SdkWebhookEventDeclarationNamespace = "SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests.MockSdk.Events",
                    ThrowOnNotFoundRequestModelTypes = true,
                    ThrowOnNotFoundResponseModelTypes = true,
                    ThrowOnNotFoundExecutingExtensionTypes = true,
                    ThrowOnNotFoundWebhookEventTypes = true
                });
                analyzer.AssertNoIssues();
            });

            Assert.Null(exception);
        }

        [Fact(DisplayName = "测试用例：质量分析器之 SourceFileAnalyzer")]
        public void TestSourceFileAnalyzer()
        {
            Exception? exception = Record.Exception(() =>
            {
                string workdir = Environment.CurrentDirectory;
                string rootdir = Path.Combine(workdir, "../../../");

                IAnalyzer analyzer = new SourceFileAnalyzer(new SourceFileAnalyzerOptions()
                {
                    SdkAssembly = Assembly.GetAssembly(typeof(MockClient))!,
                    SdkRequestModelDeclarationNamespace = "SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests.MockSdk.Models",
                    SdkResponseModelDeclarationNamespace = "SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests.MockSdk.Models",
                    SdkWebhookEventDeclarationNamespace = "SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests.MockSdk.Events",
                    ProjectSourceRootDirectory = rootdir,
                    ProjectSourceRequestModelClassCodeSubDirectory = "MockSdk/Models",
                    ProjectSourceResponseModelClassCodeSubDirectory = "MockSdk/Models",
                    ProjectSourceExecutingExtensionClassCodeSubDirectory = "MockSdk/Extensions",
                    ProjectSourceWebhookEventClassCodeSubDirectory = "MockSdk/Events",
                    ProjectTestRootDirectory = rootdir,
                    ProjectTestRequestModelSerializationSampleSubDirectory = "MockSdk/ModelSamples",
                    ProjectTestResponseModelSerializationSampleSubDirectory = "MockSdk/ModelSamples",
                    ProjectTestWebhookEventSerializationSampleSubDirectory = "MockSdk/EventSamples",
                    ThrowOnNotFoundRequestModelClassCodeFiles = true,
                    ThrowOnNotFoundResponseModelClassCodeFiles = true,
                    ThrowOnNotFoundExecutingExtensionClassCodeFiles = true,
                    ThrowOnNotFoundWebhookEventClassCodeFiles = true,
                    ThrowOnNotFoundRequestModelSerializationSampleFiles = true,
                    ThrowOnNotFoundResponseModelSerializationSampleFiles = true
                });
                analyzer.AssertNoIssues();
            });

            Assert.Null(exception);
        }
    }
}
