namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests
{
    using SKIT.FlurlHttpClient.Tools.CodeAnalyzer.Helpers;

    public class TestCase_ActivatorHelperTest
    {
        private sealed class MockClass
        {
            public bool? PropertyAsBool { get; set; }

            public int? PropertyAsInt32 { get; set; }

            public long? PropertyAsInt64 { get; set; }

            public decimal? PropertyAsDecimal { get; set; }

            public string? PropertyAsString { get; set; }

            public DateTimeOffset? PropertyAsDateTime { get; set; }

            public string[]? PropertyAsStringArray { get; set; }

            public MockClass? PropertyAsObject { get; set; }

            public MockClass[]? PropertyAsArray { get; set; }

            public IList<MockClass>? PropertyAsList { get; set; }

            public IDictionary<string, MockClass>? PropertyAsDictionary { get; set; }
        }

        [Fact(DisplayName = "测试用例：工具类之 ActivatorHelper")]
        public void TestActivatorHelper()
        {
            {
                var actualObj = ActivatorHelper.CreateInitializedInstance<MockClass>();

                Assert.True(actualObj.PropertyAsBool);
                Assert.True(actualObj.PropertyAsInt32 > 0);
                Assert.True(actualObj.PropertyAsInt64 > 0);
                Assert.True(actualObj.PropertyAsDecimal > 0);
                Assert.NotNull(actualObj.PropertyAsString);
                Assert.NotNull(actualObj.PropertyAsDateTime);
                Assert.NotNull(actualObj.PropertyAsStringArray);
                Assert.NotNull(actualObj.PropertyAsObject);
                Assert.NotNull(actualObj.PropertyAsArray);
                Assert.NotNull(actualObj.PropertyAsList);
                Assert.NotNull(actualObj.PropertyAsDictionary);
            }

            {
                var actualObj = (MockClass)ActivatorHelper.CreateInitializedInstance(typeof(MockClass));

                Assert.True(actualObj.PropertyAsBool);
                Assert.True(actualObj.PropertyAsInt32 > 0);
                Assert.True(actualObj.PropertyAsInt64 > 0);
                Assert.True(actualObj.PropertyAsDecimal > 0);
                Assert.NotNull(actualObj.PropertyAsString);
                Assert.NotNull(actualObj.PropertyAsDateTime);
                Assert.NotNull(actualObj.PropertyAsStringArray);
                Assert.NotNull(actualObj.PropertyAsObject);
                Assert.NotNull(actualObj.PropertyAsArray);
                Assert.NotNull(actualObj.PropertyAsList);
                Assert.NotNull(actualObj.PropertyAsDictionary);
            }
        }
    }
}
