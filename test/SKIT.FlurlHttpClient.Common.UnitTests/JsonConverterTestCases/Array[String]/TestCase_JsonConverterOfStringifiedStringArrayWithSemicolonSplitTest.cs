using NUnit.Framework;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases
{
    using SKIT.FlurlHttpClient.Configuration;

    public class TestCase_JsonConverterOfStringifiedStringArrayWithSemicolonSplitTest
    {
        private sealed class MockObject
        {
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedStringArrayWithSemicolonSplitConverter))]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.StringifiedStringArrayWithSemicolonSplitConverter))]
            public string[]? Property { get; set; }
        }

        private static void TestCustomJsonConverter(IJsonSerializer jsonSerializer)
        {
            Assert.Multiple(() =>
            {
                var expectObj = new MockObject() { Property = new string[] { "a", "b", "c" } };
                var actualJson = jsonSerializer.Serialize(expectObj);
                var actualObj = jsonSerializer.Deserialize<MockObject>(actualJson);

                Assert.That(actualJson, Is.EqualTo("{\"Property\":\"a;b;c\"}"));

                Assert.That(actualObj.Property, Is.EqualTo(expectObj.Property));
            });
        }

        [Test(Description = "测试用例：自定义 Newtosoft.Json.JsonConverter 之 TextualStringArrayWithSemicolonSplitConverter")]
        public void TestNewtosoftJsonConverter()
        {
            var jsonSettings = NewtonsoftJsonSerializer.GetDefaultSerializerSettings();
            jsonSettings.Formatting = Newtonsoft.Json.Formatting.None;

            TestCustomJsonConverter(new NewtonsoftJsonSerializer(jsonSettings));
        }

        [Test(Description = "测试用例：自定义 System.Text.Json.Serialization.JsonConverter 之 TextualStringArrayWithSemicolonSplitConverter")]
        public void TestSystemTextJsonConverter()
        {
            var jsonOptions = SystemTextJsonSerializer.GetDefaultSerializerOptions();
            jsonOptions.WriteIndented = false;

            TestCustomJsonConverter(new SystemTextJsonSerializer(jsonOptions));
        }
    }
}
