using NUnit.Framework;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases
{
    public class TestCase_JsonConverterOfNumericalStringTest
    {
        private sealed class MockObject
        {
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.NumericalStringConverter))]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.NumericalStringConverter))]
            public string? Property { get; set; }
        }

        private static void TestCustomJsonConverter(IJsonSerializer jsonSerializer)
        {
            Assert.Multiple(() =>
            {
                var expectObj = new MockObject() { Property = "" };
                var actualJson = jsonSerializer.Serialize(expectObj);
                var actualObj = jsonSerializer.Deserialize<MockObject>(actualJson);

                Assert.That(actualJson, Is.EqualTo("{\"Property\":\"\"}"));

                Assert.That(actualObj.Property, Is.EqualTo(expectObj.Property));
            });

            Assert.Multiple(() =>
            {
                var expectObj = new MockObject() { Property = "32767" };
                var actualJson = jsonSerializer.Serialize(expectObj);
                var actualObj = jsonSerializer.Deserialize<MockObject>(actualJson);

                Assert.That(actualJson, Is.EqualTo("{\"Property\":32767}"));

                Assert.That(actualObj.Property, Is.EqualTo(expectObj.Property));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"Property\":\"32767\"}").Property, Is.EqualTo(expectObj.Property));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"Property\":32767}").Property, Is.EqualTo(expectObj.Property));
            });

            Assert.Multiple(() =>
            {
                var expectObj = new MockObject() { Property = "-32768" };
                var actualJson = jsonSerializer.Serialize(expectObj);
                var actualObj = jsonSerializer.Deserialize<MockObject>(actualJson);

                Assert.That(actualJson, Is.EqualTo("{\"Property\":-32768}"));

                Assert.That(actualObj.Property, Is.EqualTo(expectObj.Property));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"Property\":\"-32768\"}").Property, Is.EqualTo(expectObj.Property));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"Property\":-32768}").Property, Is.EqualTo(expectObj.Property));
            });

            Assert.Multiple(() =>
            {
                var expectObj = new MockObject() { Property = "2147483647" };
                var actualJson = jsonSerializer.Serialize(expectObj);
                var actualObj = jsonSerializer.Deserialize<MockObject>(actualJson);

                Assert.That(actualJson, Is.EqualTo("{\"Property\":2147483647}"));

                Assert.That(actualObj.Property, Is.EqualTo(expectObj.Property));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"Property\":\"2147483647\"}").Property, Is.EqualTo(expectObj.Property));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"Property\":2147483647}").Property, Is.EqualTo(expectObj.Property));
            });

            Assert.Multiple(() =>
            {
                var expectObj = new MockObject() { Property = "-2147483648" };
                var actualJson = jsonSerializer.Serialize(expectObj);
                var actualObj = jsonSerializer.Deserialize<MockObject>(actualJson);

                Assert.That(actualJson, Is.EqualTo("{\"Property\":-2147483648}"));

                Assert.That(actualObj.Property, Is.EqualTo(expectObj.Property));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"Property\":\"-2147483648\"}").Property, Is.EqualTo(expectObj.Property));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"Property\":-2147483648}").Property, Is.EqualTo(expectObj.Property));
            });

            Assert.Multiple(() =>
            {
                var expectObj = new MockObject() { Property = "9223372036854775807" };
                var actualJson = jsonSerializer.Serialize(expectObj);
                var actualObj = jsonSerializer.Deserialize<MockObject>(actualJson);

                Assert.That(actualJson, Is.EqualTo("{\"Property\":9223372036854775807}"));

                Assert.That(actualObj.Property, Is.EqualTo(expectObj.Property));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"Property\":\"9223372036854775807\"}").Property, Is.EqualTo(expectObj.Property));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"Property\":9223372036854775807}").Property, Is.EqualTo(expectObj.Property));
            });

            Assert.Multiple(() =>
            {
                var expectObj = new MockObject() { Property = "-9223372036854775808" };
                var actualJson = jsonSerializer.Serialize(expectObj);
                var actualObj = jsonSerializer.Deserialize<MockObject>(actualJson);

                Assert.That(actualJson, Is.EqualTo("{\"Property\":-9223372036854775808}"));

                Assert.That(actualObj.Property, Is.EqualTo(expectObj.Property));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"Property\":\"-9223372036854775808\"}").Property, Is.EqualTo(expectObj.Property));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"Property\":-9223372036854775808}").Property, Is.EqualTo(expectObj.Property));
            });

            Assert.Multiple(() =>
            { 
                var expectObj = new MockObject() { Property = "18446744073709551615" };
                var actualJson = jsonSerializer.Serialize(expectObj);
                var actualObj = jsonSerializer.Deserialize<MockObject>(actualJson);

                Assert.That(actualJson, Is.EqualTo("{\"Property\":18446744073709551615}"));

                Assert.That(actualObj.Property, Is.EqualTo(expectObj.Property));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"Property\":\"18446744073709551615\"}").Property, Is.EqualTo(expectObj.Property));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"Property\":18446744073709551615}").Property, Is.EqualTo(expectObj.Property));
            });

            Assert.Multiple(() =>
            {
                var expectObj = new MockObject() { Property = "1.23" };
                var actualJson = jsonSerializer.Serialize(expectObj);
                var actualObj = jsonSerializer.Deserialize<MockObject>(actualJson);

                Assert.That(actualJson, Is.EqualTo("{\"Property\":1.23}"));

                Assert.That(actualObj.Property, Is.EqualTo(expectObj.Property));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"Property\":\"1.23\"}").Property, Is.EqualTo(expectObj.Property));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"Property\":1.23}").Property, Is.EqualTo(expectObj.Property));
            });

            Assert.Multiple(() =>
            {
                var expectObj = new MockObject() { Property = "-1.23" };
                var actualJson = jsonSerializer.Serialize(expectObj);
                var actualObj = jsonSerializer.Deserialize<MockObject>(actualJson);

                Assert.That(actualJson, Is.EqualTo("{\"Property\":-1.23}"));

                Assert.That(actualObj.Property, Is.EqualTo(expectObj.Property));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"Property\":\"-1.23\"}").Property, Is.EqualTo(expectObj.Property));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"Property\":-1.23}").Property, Is.EqualTo(expectObj.Property));
            });
        }

        [Test(Description = "测试用例：自定义 Newtosoft.Json.JsonConverter 之 NumericalStringConverter")]
        public void TestNewtosoftJsonConverter()
        {
            var jsonSettings = NewtonsoftJsonSerializer.GetDefaultSerializerSettings();
            jsonSettings.Formatting = Newtonsoft.Json.Formatting.None;

            TestCustomJsonConverter(new NewtonsoftJsonSerializer(jsonSettings));
        }

        [Test(Description = "测试用例：自定义 System.Text.Json.Serialization.JsonConverter 之 NumericalStringConverter")]
        public void TestSystemTextJsonConverter()
        {
            var jsonOptions = SystemTextJsonSerializer.GetDefaultSerializerOptions();
            jsonOptions.WriteIndented = false;

            TestCustomJsonConverter(new SystemTextJsonSerializer(jsonOptions));
        }
    }
}
