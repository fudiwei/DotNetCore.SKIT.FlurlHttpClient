using Microsoft.CodeAnalysis.CSharp;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests
{
    using SKIT.FlurlHttpClient.Tools.CodeAnalyzer.Helpers;

    public partial class TestCase_RoslynHelperTest
    {
        [Fact(DisplayName = "测试用例：工具类 `RoslynHelper.FindTopLevelClassDeclarations`")]
        public void TestRoslynHelper_FindTopLevelClassDeclarations()
        {
            Assert.Multiple(() =>
            {
                const string source = """
                    namespace First
                    {
                        public class FirstClass { }

                        public class GenericClass<T> { }

                        public struct FirstStruct { }

                        public class OuterClass
                        {
                            public class NestedClass { }
                        }
                    }

                    namespace Second
                    {
                        public class SecondClass { }
                    }

                    public class GlobalClass { }
                    """;

                var declarations = RoslynHelper.FindTopLevelClassDeclarations(source);

                Assert.Equal(
                    new[] { "FirstClass", "OuterClass", "SecondClass" },
                    declarations.Select(declaration => declaration.Identifier.ValueText)
                );
            });

            Assert.Multiple(() =>
            {
                const string source = """
                    namespace FileScoped;

                    public class FileScopedClass { }

                    public class GenericClass<T> { }
                    """;

                var tree = CSharpSyntaxTree.ParseText(source, cancellationToken: TestContext.Current.CancellationToken);
                var declarations = RoslynHelper.FindTopLevelClassDeclarations(tree);

                var declaration = Assert.Single(declarations);
                Assert.Equal("FileScopedClass", declaration.Identifier.ValueText);
            });
        }

        [Fact(DisplayName = "测试用例：工具类 `RoslynHelper.GetTypeDocumentationSummary`")]
        public void TestRoslynHelper_GetTypeDocumentationSummary()
        {
            Assert.Multiple(() =>
            {
                const string source = """
                    namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests
                    {
                        /// <summary>
                        ///   Example summary.
                        /// </summary>
                        public class MockDocumentationClass { }
                    }
                    """;

                var tree = CSharpSyntaxTree.ParseText(source, cancellationToken: TestContext.Current.CancellationToken);

                Assert.Equal("Example summary.", RoslynHelper.GetTypeDocumentationSummary(tree, typeof(Mocks.MockDocumentationClass)));
                Assert.Equal("Example summary.", RoslynHelper.GetTypeDocumentationSummary(source, typeof(Mocks.MockDocumentationClass)));
            });

            Assert.Multiple(() =>
            {
                const string source = """
                    namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests
                    {
                        public class MockUndocumentedClass { }
                    }
                    """;

                var tree = CSharpSyntaxTree.ParseText(source, cancellationToken: TestContext.Current.CancellationToken);

                Assert.Null(RoslynHelper.GetTypeDocumentationSummary(source, typeof(Mocks.MockUndocumentedClass)));
                Assert.Null(RoslynHelper.GetTypeDocumentationSummary(source, typeof(string)));
            });

            Assert.Multiple(() =>
            {
                const string source = """
                    namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests
                    {
                        /// <summary>Generic summary.</summary>
                        public class MockGenericDocumentationClass<T> { }
                    }
                    """;

                var tree = CSharpSyntaxTree.ParseText(source, cancellationToken: TestContext.Current.CancellationToken);

                Assert.Equal("Generic summary.", RoslynHelper.GetTypeDocumentationSummary(source, typeof(Mocks.MockGenericDocumentationClass<int>)));
            });
        }

        [Fact(DisplayName = "测试用例：工具类 `RoslynHelper.GetMethodDocumentationSummary` 之方法")]
        public void TestRoslynHelper_GetMethodDocumentationSummary()
        {
            const string source = """
                namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests
                {
                    public class MockMethodClass
                    {
                        /// <summary>
                        ///   Int overload.
                        /// </summary>
                        public void Foo(int value) { }

                        /// <summary>String overload.</summary>
                        public void Foo(string value) { }

                        public void Bar() { }
                    }
                }
                """;

            var tree = CSharpSyntaxTree.ParseText(source, cancellationToken: TestContext.Current.CancellationToken);
            var type = typeof(Mocks.MockMethodClass);
            var intMethod = type.GetMethod("Foo", new[] { typeof(int) })!;
            var stringMethod = type.GetMethod("Foo", new[] { typeof(string) })!;
            var barMethod = type.GetMethod("Bar")!;

            Assert.Equal("Int overload.", RoslynHelper.GetMethodDocumentationSummary(source, intMethod));
            Assert.Equal("String overload.", RoslynHelper.GetMethodDocumentationSummary(source, stringMethod));
            Assert.Null(RoslynHelper.GetMethodDocumentationSummary(source, barMethod));
        }
    }

    partial class TestCase_RoslynHelperTest
    {
        private static class Mocks
        {
            public class MockDocumentationClass
            {
            }

            public class MockUndocumentedClass
            {
            }

            public class MockGenericDocumentationClass<T>
            {
            }

            public class MockMethodClass
            {
                public void Foo(int value) { }
                public void Foo(string value) { }
                public void Bar() { }
            }
        }
    }
}
