#if NET5_0_OR_GREATER
using System;
using NUnit.Framework;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases.JsonConverter
{
    using SKIT.FlurlHttpClient.Configuration;

    public class TestCase_JsonConverterOfDigitalDateOnlyTest
    {
        private sealed class MockObject
        {
            [Newtonsoft.Json.JsonProperty(Order = 1)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.DigitalDateOnlyConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(1)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.DigitalDateOnlyConverter))]
            public DateOnly Property { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 2)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.DigitalDateOnlyConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(2)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.DigitalDateOnlyConverter))]
            public DateOnly? NullableProperty { get; set; }
        }

        private static void TestCustomJsonConverter(IJsonSerializer jsonSerializer)
        {
            DateOnly DATETIME = new DateOnly(2006, 1, 2);

            var mockObj1 = new MockObject() { Property = DATETIME, NullableProperty = null };
            var actualJson1 = jsonSerializer.Serialize(mockObj1);
            var actualObj1 = jsonSerializer.Deserialize<MockObject>(actualJson1);
            Assert.AreEqual("{\"Property\":\"20060102\"}", actualJson1);
            Assert.AreEqual(mockObj1.Property, actualObj1.Property);
            Assert.AreEqual(mockObj1.NullableProperty, actualObj1.NullableProperty);

            var mockObj2 = new MockObject() { Property = DATETIME, NullableProperty = DATETIME };
            var actualJson2 = jsonSerializer.Serialize(mockObj2);
            var actualObj2 = jsonSerializer.Deserialize<MockObject>(actualJson2);
            Assert.AreEqual("{\"Property\":\"20060102\",\"NullableProperty\":\"20060102\"}", actualJson2);
            Assert.AreEqual(mockObj2.Property, actualObj2.Property);
            Assert.AreEqual(mockObj2.NullableProperty, actualObj2.NullableProperty);
        }

        [Test(Description = "测试用例：自定义 Newtosoft.Json.JsonConverter 之 DigitalDateOnlyConverter")]
        public void TestNewtosoftJsonConverter()
        {
            var jsonSettings = NewtonsoftJsonSerializer.GetDefaultSerializerSettings();
            jsonSettings.Formatting = Newtonsoft.Json.Formatting.None;
            jsonSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

            TestCustomJsonConverter(new NewtonsoftJsonSerializer(jsonSettings));
        }

        [Test(Description = "测试用例：自定义 System.Text.Json.Serialization.JsonConverter 之 DigitalDateOnlyConverter")]
        public void TestSystemTextJsonConverter()
        {
            var jsonOptions = SystemTextJsonSerializer.GetDefaultSerializerOptions();
            jsonOptions.WriteIndented = false;
            jsonOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;

            TestCustomJsonConverter(new SystemTextJsonSerializer(jsonOptions));
        }
    }
}
#endif
