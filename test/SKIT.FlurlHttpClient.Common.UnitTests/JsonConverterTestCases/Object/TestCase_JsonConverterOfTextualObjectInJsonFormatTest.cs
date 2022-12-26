using System.Collections.Generic;
using NUnit.Framework;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases.JsonConverter
{
    using SKIT.FlurlHttpClient.Configuration;

    public class TestCase_JsonConverterOfTextualObjectInJsonFormat
    {
        private sealed class MockObject
        {
            [Newtonsoft.Json.JsonProperty(nameof(PlainObject), Order = 1)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualObjectInJsonFormatConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(1)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.TextualObjectInJsonFormatConverter))]
            public MockNestedObject? PlainObject { get; set; }

            [Newtonsoft.Json.JsonProperty(nameof(CollectionObject), Order = 2)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualObjectInJsonFormatConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(2)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.TextualObjectInJsonFormatConverter))]
            public IList<string>? CollectionObject { get; set; }
        }

        private sealed class MockNestedObject
        {
            [Newtonsoft.Json.JsonProperty(nameof(BooleanProperty), Order = 1)]
            [System.Text.Json.Serialization.JsonPropertyOrder(1)]
            public bool BooleanProperty { get; set; }

            [Newtonsoft.Json.JsonProperty(nameof(BooleanPropertyWithConverter), Order = 1)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.NumericalBooleanConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(1)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.NumericalBooleanConverter))]
            public bool BooleanPropertyWithConverter { get; set; }
        }

        private static void TestCustomJsonConverter(IJsonSerializer jsonSerializer)
        {
            var mockObj1 = new MockObject()
            {
                PlainObject = new MockNestedObject()
                {
                    BooleanProperty = true,
                    BooleanPropertyWithConverter = true
                },
                CollectionObject = new List<string>() { "hello world" }
            };
            var actualJson1 = jsonSerializer.Serialize(mockObj1);
            var actualObj1 = jsonSerializer.Deserialize<MockObject>(actualJson1);
            Assert.AreEqual("{\"PlainObject\":\"{\\\"BooleanProperty\\\":true,\\\"BooleanPropertyWithConverter\\\":1}\",\"CollectionObject\":\"[\\\"hello world\\\"]\"}", actualJson1);
            Assert.AreEqual(mockObj1.PlainObject!.BooleanProperty, actualObj1.PlainObject?.BooleanProperty);
            Assert.AreEqual(mockObj1.PlainObject!.BooleanPropertyWithConverter, actualObj1.PlainObject?.BooleanPropertyWithConverter);
            CollectionAssert.AreEqual(mockObj1.CollectionObject, actualObj1.CollectionObject);
        }

        [Test(Description = "测试用例：自定义 Newtosoft.Json.JsonConverter 之 TextualObjectInJsonFormatConverter")]
        public void TestNewtosoftJsonConverter()
        {
            var jsonSettings = NewtonsoftJsonSerializer.GetDefaultSerializerSettings();
            jsonSettings.Formatting = Newtonsoft.Json.Formatting.None;

            TestCustomJsonConverter(new NewtonsoftJsonSerializer(jsonSettings));
        }

        [Test(Description = "测试用例：自定义 System.Text.Json.Serialization.JsonConverter 之 TextualObjectInJsonFormatConverter")]
        public void TestSystemTextJsonConverter()
        {
            var jsonOptions = SystemTextJsonSerializer.GetDefaultSerializerOptions();
            jsonOptions.WriteIndented = false;

            TestCustomJsonConverter(new SystemTextJsonSerializer(jsonOptions));
        }
    }
}
