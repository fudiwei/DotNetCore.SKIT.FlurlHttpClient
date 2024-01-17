using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases.JsonConverter
{
    using SKIT.FlurlHttpClient.Configuration;

    public class TestCase_JsonConverterOfDynamicObjectTest
    {
        private sealed class MockObject
        {
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.DynamicObjectConverter))]
            public dynamic? NullProperty { get; set; }

            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.DynamicObjectConverter))]
            public dynamic? BooleanProperty { get; set; }

            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.DynamicObjectConverter))]
            public dynamic? NumberProperty { get; set; }

            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.DynamicObjectConverter))]
            public dynamic? StringProperty { get; set; }

            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.DynamicObjectConverter))]
            public dynamic? GuidProperty { get; set; }

            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.DynamicObjectConverter))]
            public dynamic? ArrayProperty { get; set; }

            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.DynamicObjectConverter))]
            public dynamic? ListProperty { get; set; }

            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.DynamicObjectConverter))]
            public dynamic? DictionaryProperty { get; set; }

            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.DynamicObjectConverter))]
            public dynamic? AnonymousObjectProperty { get; set; }

            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.DynamicObjectConverter))]
            public dynamic? ObjectWithCustomConverterProperty { get; set; }
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
                    NullProperty = null,
                    BooleanProperty = true,
                    NumberProperty = 123456,
                    StringProperty = "abc",
                    GuidProperty = Guid.Parse("11112222-3333-4444-5555-666677778888"),
                    ArrayProperty = new object?[] { null, true, 123456, "abc" },
                    ListProperty = new List<object?>() { null, true, 123456, "abc" },
                    DictionaryProperty = new Dictionary<string, object?>() { { "k0", null }, { "k1", true }, { "k2", 123456 }, { "k3", "abc" } },
                    AnonymousObjectProperty = new { k0 = default(object), k1 = true, k2 = 123456, k3 = "abc" },
                    ObjectWithCustomConverterProperty = new MockNestedObject()
                    {
                        BooleanProperty = true,
                        BooleanPropertyWithConverter = true
                    }
                };
                var actualJson = jsonSerializer.Serialize(expectObj);
                var actualObj = jsonSerializer.Deserialize<MockObject>(actualJson);

                Assert.That(actualJson, Does.Contain("{\"BooleanProperty\":true,\"BooleanPropertyWithConverter\":1}"));

                Assert.That(actualObj.NullProperty, Is.Null);
                Assert.That(actualObj.BooleanProperty, Is.Not.Null);
                Assert.That(actualObj.NumberProperty, Is.Not.Null);
                Assert.That(actualObj.StringProperty, Is.Not.Null);
                Assert.That(actualObj.GuidProperty, Is.Not.Null);
                Assert.That(actualObj.ArrayProperty, Is.Not.Null);
                Assert.That(actualObj.ListProperty, Is.Not.Null);
                Assert.That(actualObj.DictionaryProperty, Is.Not.Null);
                Assert.That(actualObj.AnonymousObjectProperty, Is.Not.Null);
                Assert.That(actualObj.ObjectWithCustomConverterProperty, Is.Not.Null);
            });
        }

        [Test(Description = "测试用例：自定义 Newtosoft.Json.JsonConverter 之 DynamicObjectConverter")]
        public void TestNewtosoftJsonConverter()
        {
            var jsonSettings = NewtonsoftJsonSerializer.GetDefaultSerializerSettings();
            jsonSettings.Formatting = Newtonsoft.Json.Formatting.None;
            jsonSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

            TestCustomJsonConverter(new NewtonsoftJsonSerializer(jsonSettings));
        }

        [Test(Description = "测试用例：自定义 System.Text.Json.Serialization.JsonConverter 之 DynamicObjectConverter")]
        public void TestSystemTextJsonConverter()
        {
            var jsonOptions = SystemTextJsonSerializer.GetDefaultSerializerOptions();
            jsonOptions.WriteIndented = false;
            jsonOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;

            TestCustomJsonConverter(new SystemTextJsonSerializer(jsonOptions));
        }
    }
}
