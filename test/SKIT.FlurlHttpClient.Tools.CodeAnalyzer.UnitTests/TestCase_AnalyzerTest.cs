using System.Reflection;
using NUnit.Framework;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests
{
    using System;
    using System.IO;
    using SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests.MockSdk;

    public class TestCase_AnalyzerTest
    {
        [Test(Description = "测试用例：质量分析器之 TypeDeclarationAnalyzer")]
        public void TestTypeDeclarationAnalyzer()
        {
            Assert.DoesNotThrow(() =>
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
        }

        [Test(Description = "测试用例：质量分析器之 SourceFileAnalyzer")]
        public void TestSourceFileAnalyzer()
        {
            Assert.DoesNotThrow(() =>
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
        }
    }
}
