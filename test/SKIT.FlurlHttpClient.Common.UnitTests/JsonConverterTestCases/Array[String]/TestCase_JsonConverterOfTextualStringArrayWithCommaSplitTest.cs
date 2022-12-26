using NUnit.Framework;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases.JsonConverter
{
    using SKIT.FlurlHttpClient.Configuration;

    public class TestCase_JsonConverterOfTextualStringArrayWithCommaSplitTest
    {
        private sealed class MockObject
        {
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualStringArrayWithCommaSplitConverter))]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.TextualStringArrayWithCommaSplitConverter))]
            public string[]? Property { get; set; }
        }

        private static void TestCustomJsonConverter(IJsonSerializer jsonSerializer)
        {
            var mockObj1 = new MockObject() { Property = new string[] { "a", "b", "c" } };
            var actualJson1 = jsonSerializer.Serialize(mockObj1);
            var actualObj1 = jsonSerializer.Deserialize<MockObject>(actualJson1);
            Assert.AreEqual("{\"Property\":\"a,b,c\"}", actualJson1);
            CollectionAssert.AreEqual(mockObj1.Property, actualObj1.Property);
        }

        [Test(Description = "测试用例：自定义 Newtosoft.Json.JsonConverter 之 TextualStringArrayWithCommaSplitConverter")]
        public void TestNewtosoftJsonConverter()
        {
            var jsonSettings = NewtonsoftJsonSerializer.GetDefaultSerializerSettings();
            jsonSettings.Formatting = Newtonsoft.Json.Formatting.None;

            TestCustomJsonConverter(new NewtonsoftJsonSerializer(jsonSettings));
        }

        [Test(Description = "测试用例：自定义 System.Text.Json.Serialization.JsonConverter 之 TextualStringArrayWithCommaSplitConverter")]
        public void TestSystemTextJsonConverter()
        {
            var jsonOptions = SystemTextJsonSerializer.GetDefaultSerializerOptions();
            jsonOptions.WriteIndented = false;

            TestCustomJsonConverter(new SystemTextJsonSerializer(jsonOptions));
        }
    }
}
