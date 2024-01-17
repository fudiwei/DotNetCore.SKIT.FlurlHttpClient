using System.Collections.Generic;
using NUnit.Framework;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases
{
    using SKIT.FlurlHttpClient.Configuration;

    public class TestCase_JsonConverterOfStringifiedObjectInJsonFormatTest
    {
        private sealed class MockObject
        {
            [Newtonsoft.Json.JsonProperty(nameof(PlainObject), Order = 1)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedObjectInJsonFormatConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(1)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.StringifiedObjectInJsonFormatConverter))]
            public MockNestedObject? PlainObject { get; set; }

            [Newtonsoft.Json.JsonProperty(nameof(CollectionObject), Order = 2)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedObjectInJsonFormatConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(2)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.StringifiedObjectInJsonFormatConverter))]
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
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.NumericalBooleanConverter))]
            public bool BooleanPropertyWithConverter { get; set; }
        }

        private static void TestCustomJsonConverter(IJsonSerializer jsonSerializer)
        {
            Assert.Multiple(() =>
            {
                var expectObj = new MockObject()
                {
                    PlainObject = new MockNestedObject()
                    {
                        BooleanProperty = true,
                        BooleanPropertyWithConverter = true
                    },
                    CollectionObject = new List<string>() { "hello world" }
                };
                var actualJson = jsonSerializer.Serialize(expectObj);
                var actualObj = jsonSerializer.Deserialize<MockObject>(actualJson);

                Assert.That(actualJson, Is.EqualTo("{\"PlainObject\":\"{\\\"BooleanProperty\\\":true,\\\"BooleanPropertyWithConverter\\\":1}\",\"CollectionObject\":\"[\\\"hello world\\\"]\"}"));

                Assert.That(actualObj.PlainObject?.BooleanProperty, Is.EqualTo(expectObj.PlainObject!.BooleanProperty));
                Assert.That(actualObj.PlainObject?.BooleanPropertyWithConverter, Is.EqualTo(expectObj.PlainObject!.BooleanPropertyWithConverter));
                Assert.That(actualObj.CollectionObject, Is.EqualTo(expectObj.CollectionObject));
            });
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
