using NUnit.Framework;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases.JsonConverter
{
    using SKIT.FlurlHttpClient.Configuration;

    public class TestCase_JsonConverterOfNumericalStringReadOnlyTest
    {
        private sealed class MockObject
        {
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.NumericalStringReadOnlyConverter))]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.NumericalStringReadOnlyConverter))]
            public string? Property { get; set; }
        }

        private static void TestCustomJsonConverter(IJsonSerializer jsonSerializer)
        {
            var mockObj1 = new MockObject() { Property = "" };
            var actualJson1 = jsonSerializer.Serialize(mockObj1);
            var actualObj1 = jsonSerializer.Deserialize<MockObject>(actualJson1);
            Assert.AreEqual("{\"Property\":\"\"}", actualJson1);
            Assert.AreEqual(mockObj1.Property, actualObj1.Property);

            var mockObj2 = new MockObject() { Property = "32767" };
            var actualJson2 = jsonSerializer.Serialize(mockObj2);
            var actualObj2 = jsonSerializer.Deserialize<MockObject>(actualJson2);
            Assert.AreEqual("{\"Property\":\"32767\"}", actualJson2);
            Assert.AreEqual(mockObj2.Property, actualObj2.Property);
            Assert.AreEqual(mockObj2.Property, jsonSerializer.Deserialize<MockObject>("{\"Property\":32767}").Property);

            var mockObj3 = new MockObject() { Property = "-32768" };
            var actualJson3 = jsonSerializer.Serialize(mockObj3);
            var actualObj3 = jsonSerializer.Deserialize<MockObject>(actualJson3);
            Assert.AreEqual("{\"Property\":\"-32768\"}", actualJson3);
            Assert.AreEqual(mockObj3.Property, actualObj3.Property);
            Assert.AreEqual(mockObj3.Property, jsonSerializer.Deserialize<MockObject>("{\"Property\":-32768}").Property);

            var mockObj4 = new MockObject() { Property = "2147483647" };
            var actualJson4 = jsonSerializer.Serialize(mockObj4);
            var actualObj4 = jsonSerializer.Deserialize<MockObject>(actualJson4);
            Assert.AreEqual("{\"Property\":\"2147483647\"}", actualJson4);
            Assert.AreEqual(mockObj4.Property, actualObj4.Property);
            Assert.AreEqual(mockObj4.Property, jsonSerializer.Deserialize<MockObject>("{\"Property\":2147483647}").Property);

            var mockObj5 = new MockObject() { Property = "-2147483648" };
            var actualJson5 = jsonSerializer.Serialize(mockObj5);
            var actualObj5 = jsonSerializer.Deserialize<MockObject>(actualJson5);
            Assert.AreEqual("{\"Property\":\"-2147483648\"}", actualJson5);
            Assert.AreEqual(mockObj5.Property, actualObj5.Property);
            Assert.AreEqual(mockObj5.Property, jsonSerializer.Deserialize<MockObject>("{\"Property\":-2147483648}").Property);

            var mockObj6 = new MockObject() { Property = "9223372036854775807" };
            var actualJson6 = jsonSerializer.Serialize(mockObj6);
            var actualObj6 = jsonSerializer.Deserialize<MockObject>(actualJson6);
            Assert.AreEqual("{\"Property\":\"9223372036854775807\"}", actualJson6);
            Assert.AreEqual(mockObj6.Property, actualObj6.Property);
            Assert.AreEqual(mockObj6.Property, jsonSerializer.Deserialize<MockObject>("{\"Property\":9223372036854775807}").Property);

            var mockObj7 = new MockObject() { Property = "-9223372036854775808" };
            var actualJson7 = jsonSerializer.Serialize(mockObj7);
            var actualObj7 = jsonSerializer.Deserialize<MockObject>(actualJson7);
            Assert.AreEqual("{\"Property\":\"-9223372036854775808\"}", actualJson7);
            Assert.AreEqual(mockObj7.Property, actualObj7.Property);
            Assert.AreEqual(mockObj7.Property, jsonSerializer.Deserialize<MockObject>("{\"Property\":-9223372036854775808}").Property);

            var mockObj8 = new MockObject() { Property = "18446744073709551615" };
            var actualJson8 = jsonSerializer.Serialize(mockObj8);
            var actualObj8 = jsonSerializer.Deserialize<MockObject>(actualJson8);
            Assert.AreEqual("{\"Property\":\"18446744073709551615\"}", actualJson8);
            Assert.AreEqual(mockObj8.Property, actualObj8.Property);
            Assert.AreEqual(mockObj8.Property, jsonSerializer.Deserialize<MockObject>("{\"Property\":18446744073709551615}").Property);

            var mockObj9 = new MockObject() { Property = "1.23" };
            var actualJson9 = jsonSerializer.Serialize(mockObj9);
            var actualObj9 = jsonSerializer.Deserialize<MockObject>(actualJson9);
            Assert.AreEqual("{\"Property\":\"1.23\"}", actualJson9);
            Assert.AreEqual(mockObj9.Property, actualObj9.Property);
            Assert.AreEqual(mockObj9.Property, jsonSerializer.Deserialize<MockObject>("{\"Property\":1.23}").Property);

            var mockObj10 = new MockObject() { Property = "-1.23" };
            var actualJson10 = jsonSerializer.Serialize(mockObj10);
            var actualObj10 = jsonSerializer.Deserialize<MockObject>(actualJson10);
            Assert.AreEqual("{\"Property\":\"-1.23\"}", actualJson10);
            Assert.AreEqual(mockObj10.Property, actualObj10.Property);
            Assert.AreEqual(mockObj10.Property, jsonSerializer.Deserialize<MockObject>("{\"Property\":-1.23}").Property);
        }

        [Test(Description = "测试用例：自定义 Newtosoft.Json.JsonConverter 之 NumericalStringReadOnlyConverter")]
        public void TestNewtosoftJsonConverter()
        {
            var jsonSettings = NewtonsoftJsonSerializer.GetDefaultSerializerSettings();
            jsonSettings.Formatting = Newtonsoft.Json.Formatting.None;

            TestCustomJsonConverter(new NewtonsoftJsonSerializer(jsonSettings));
        }

        [Test(Description = "测试用例：自定义 System.Text.Json.Serialization.JsonConverter 之 NumericalStringReadOnlyConverter")]
        public void TestSystemTextJsonConverter()
        {
            var jsonOptions = SystemTextJsonSerializer.GetDefaultSerializerOptions();
            jsonOptions.WriteIndented = false;

            TestCustomJsonConverter(new SystemTextJsonSerializer(jsonOptions));
        }
    }
}
