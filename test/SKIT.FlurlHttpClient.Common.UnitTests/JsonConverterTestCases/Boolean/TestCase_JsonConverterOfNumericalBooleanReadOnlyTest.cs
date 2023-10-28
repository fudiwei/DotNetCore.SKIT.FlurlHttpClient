using NUnit.Framework;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases.JsonConverter
{
    using SKIT.FlurlHttpClient.Configuration;

    public class TestCase_JsonConverterOfNumericalBooleanReadOnlyTest
    {
        private sealed class MockObject
        {
            [Newtonsoft.Json.JsonProperty(Order = 1)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.NumericalBooleanReadOnlyConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(1)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.NumericalBooleanReadOnlyConverter))]
            public bool Property { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 2)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.NumericalBooleanReadOnlyConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(2)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.NumericalBooleanReadOnlyConverter))]
            public bool? NullableProperty { get; set; }
        }

        private static void TestCustomJsonConverter(IJsonSerializer jsonSerializer)
        {
            var mockObj1 = new MockObject() { NullableProperty = null };
            var actualJson1 = jsonSerializer.Serialize(mockObj1);
            var actualObj1 = jsonSerializer.Deserialize<MockObject>(actualJson1);
            Assert.AreEqual("{\"Property\":false}", actualJson1);
            Assert.AreEqual(mockObj1.Property, actualObj1.Property);
            Assert.AreEqual(mockObj1.NullableProperty, actualObj1.NullableProperty);

            var mockObj2 = new MockObject() { Property = false, NullableProperty = false };
            var actualJson2 = jsonSerializer.Serialize(mockObj2);
            var actualObj2 = jsonSerializer.Deserialize<MockObject>(actualJson2);
            Assert.AreEqual("{\"Property\":false,\"NullableProperty\":false}", actualJson2);
            Assert.AreEqual(mockObj2.Property, actualObj2.Property);
            Assert.AreEqual(mockObj2.Property, jsonSerializer.Deserialize<MockObject>("{\"Property\":0}").Property);
            Assert.AreEqual(mockObj2.Property, jsonSerializer.Deserialize<MockObject>("{\"Property\":\"0\"}").Property);
            Assert.AreEqual(mockObj2.NullableProperty, actualObj2.NullableProperty);
            Assert.AreEqual(mockObj2.NullableProperty, jsonSerializer.Deserialize<MockObject>("{\"NullableProperty\":0}").NullableProperty);
            Assert.AreEqual(mockObj2.NullableProperty, jsonSerializer.Deserialize<MockObject>("{\"NullableProperty\":\"0\"}").NullableProperty);

            var mockObj3 = new MockObject() { Property = true, NullableProperty = true };
            var actualJson3 = jsonSerializer.Serialize(mockObj3);
            var actualObj3 = jsonSerializer.Deserialize<MockObject>(actualJson3);
            Assert.AreEqual("{\"Property\":true,\"NullableProperty\":true}", actualJson3);
            Assert.AreEqual(mockObj3.Property, actualObj3.Property);
            Assert.AreEqual(mockObj3.Property, jsonSerializer.Deserialize<MockObject>("{\"Property\":1}").Property);
            Assert.AreEqual(mockObj3.Property, jsonSerializer.Deserialize<MockObject>("{\"Property\":\"1\"}").Property);
            Assert.AreEqual(mockObj3.NullableProperty, actualObj3.NullableProperty);
            Assert.AreEqual(mockObj3.NullableProperty, jsonSerializer.Deserialize<MockObject>("{\"NullableProperty\":1}").NullableProperty);
            Assert.AreEqual(mockObj3.NullableProperty, jsonSerializer.Deserialize<MockObject>("{\"NullableProperty\":\"1\"}").NullableProperty);
        }

        [Test(Description = "测试用例：自定义 Newtosoft.Json.JsonConverter 之 NumericalBooleanReadOnlyConverter")]
        public void TestNewtosoftJsonConverter()
        {
            var jsonSettings = NewtonsoftJsonSerializer.GetDefaultSerializerSettings();
            jsonSettings.Formatting = Newtonsoft.Json.Formatting.None;
            jsonSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

            TestCustomJsonConverter(new NewtonsoftJsonSerializer(jsonSettings));
        }

        [Test(Description = "测试用例：自定义 System.Text.Json.Serialization.JsonConverter 之 NumericalBooleanReadOnlyConverter")]
        public void TestSystemTextJsonConverter()
        {
            var jsonOptions = SystemTextJsonSerializer.GetDefaultSerializerOptions();
            jsonOptions.WriteIndented = false;
            jsonOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            jsonOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;

            TestCustomJsonConverter(new SystemTextJsonSerializer(jsonOptions));
        }
    }
}
