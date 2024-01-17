using System.Collections.Generic;
using NUnit.Framework;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases.JsonConverter
{
    using SKIT.FlurlHttpClient.Configuration;

    public class TestCase_JsonConverterOfTextualNumberListTest
    {
        private sealed class MockObject
        {
            [Newtonsoft.Json.JsonProperty(Order = 101)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberListConverter))]
            public IList<byte>? PropertyAsByte { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 102)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberListConverter))]
            public IList<sbyte>? PropertyAsSByte { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 103)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberListConverter))]
            public IList<short>? PropertyAsInt16 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 104)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberListConverter))]
            public IList<ushort>? PropertyAsUInt16 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 105)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberListConverter))]
            public IList<int>? PropertyAsInt32 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 106)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberListConverter))]
            public IList<uint>? PropertyAsUInt32 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 107)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberListConverter))]
            public IList<long>? PropertyAsInt64 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 108)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberListConverter))]
            public IList<ulong>? PropertyAsUInt64 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 109)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberListConverter))]
            public IList<float>? PropertyAsFloat { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 110)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberListConverter))]
            public IList<double>? PropertyAsDouble { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 111)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberListConverter))]
            public IList<decimal>? PropertyAsDecimal { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 201)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberListConverter))]
            public IList<byte?>? PropertyAsNullableByte { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 202)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberListConverter))]
            public IList<sbyte?>? PropertyAsNullableSByte { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 203)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberListConverter))]
            public IList<short?>? PropertyAsNullableInt16 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 204)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberListConverter))]
            public IList<ushort?>? PropertyAsNullableUInt16 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 205)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberListConverter))]
            public IList<int?>? PropertyAsNullableInt32 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 206)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberListConverter))]
            public IList<uint?>? PropertyAsNullableUInt32 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 207)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberListConverter))]
            public IList<long?>? PropertyAsNullableInt64 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 208)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberListConverter))]
            public IList<ulong?>? PropertyAsNullableUInt64 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 209)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberListConverter))]
            public IList<float?>? PropertyAsNullableFloat { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 210)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberListConverter))]
            public IList<double?>? PropertyAsNullableDouble { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 211)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberListConverter))]
            public IList<decimal?>? PropertyAsNullableDecimal { get; set; }
        }

        private static void TestCustomJsonConverter(IJsonSerializer jsonSerializer)
        {
            Assert.Multiple(() =>
            {
                var expectObj = new MockObject()
                {
                    PropertyAsByte = new List<byte>() { byte.MaxValue },
                    PropertyAsSByte = new List<sbyte>() { sbyte.MaxValue },
                    PropertyAsInt16 = new List<short>() { short.MaxValue },
                    PropertyAsUInt16 = new List<ushort>() { ushort.MaxValue },
                    PropertyAsInt32 = new List<int>() { int.MaxValue },
                    PropertyAsUInt32 = new List<uint>() { uint.MaxValue },
                    PropertyAsInt64 = new List<long>() { long.MaxValue },
                    PropertyAsUInt64 = new List<ulong>() { ulong.MaxValue },
                    PropertyAsFloat = new List<float>() { 1.23F },
                    PropertyAsDouble = new List<double>() { 1.23D },
                    PropertyAsDecimal = new List<decimal>() { 1.23M }
                };
                var actualJson = jsonSerializer.Serialize(expectObj);
                var actualObj = jsonSerializer.Deserialize<MockObject>(actualJson);

                Assert.That(actualJson, Does.Contain("\"PropertyAsByte\":[\"255\"]"));
                Assert.That(actualJson, Does.Contain("\"PropertyAsSByte\":[\"127\"]"));
                Assert.That(actualJson, Does.Contain("\"PropertyAsInt16\":[\"32767\"]"));
                Assert.That(actualJson, Does.Contain("\"PropertyAsUInt16\":[\"65535\"]"));
                Assert.That(actualJson, Does.Contain("\"PropertyAsInt32\":[\"2147483647\"]"));
                Assert.That(actualJson, Does.Contain("\"PropertyAsUInt32\":[\"4294967295\"]"));
                Assert.That(actualJson, Does.Contain("\"PropertyAsInt64\":[\"9223372036854775807\"]"));
                Assert.That(actualJson, Does.Contain("\"PropertyAsUInt64\":[\"18446744073709551615\"]"));
                Assert.That(actualJson, Does.Contain("\"PropertyAsDecimal\":[\"1.23\"]"));

                Assert.That(actualObj.PropertyAsByte, Is.EqualTo(expectObj.PropertyAsByte));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsByte\":[\"255\"]}").PropertyAsByte, Is.EqualTo(expectObj.PropertyAsByte));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsByte\":[255]}").PropertyAsByte, Is.EqualTo(expectObj.PropertyAsByte));

                Assert.That(actualObj.PropertyAsSByte, Is.EqualTo(expectObj.PropertyAsSByte));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsSByte\":[\"127\"]}").PropertyAsSByte, Is.EqualTo(expectObj.PropertyAsSByte));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsSByte\":[127]}").PropertyAsSByte, Is.EqualTo(expectObj.PropertyAsSByte));

                Assert.That(actualObj.PropertyAsInt16, Is.EqualTo(expectObj.PropertyAsInt16));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsInt16\":[\"32767\"]}").PropertyAsInt16, Is.EqualTo(expectObj.PropertyAsInt16));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsInt16\":[32767]}").PropertyAsInt16, Is.EqualTo(expectObj.PropertyAsInt16));

                Assert.That(actualObj.PropertyAsUInt16, Is.EqualTo(expectObj.PropertyAsUInt16));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsUInt16\":[\"65535\"]}").PropertyAsUInt16, Is.EqualTo(expectObj.PropertyAsUInt16));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsUInt16\":[65535]}").PropertyAsUInt16, Is.EqualTo(expectObj.PropertyAsUInt16));

                Assert.That(actualObj.PropertyAsInt32, Is.EqualTo(expectObj.PropertyAsInt32));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsInt32\":[\"2147483647\"]}").PropertyAsInt32, Is.EqualTo(expectObj.PropertyAsInt32));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsInt32\":[2147483647]}").PropertyAsInt32, Is.EqualTo(expectObj.PropertyAsInt32));

                Assert.That(actualObj.PropertyAsUInt32, Is.EqualTo(expectObj.PropertyAsUInt32));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsUInt32\":[\"4294967295\"]}").PropertyAsUInt32, Is.EqualTo(expectObj.PropertyAsUInt32));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsUInt32\":[4294967295]}").PropertyAsUInt32, Is.EqualTo(expectObj.PropertyAsUInt32));

                Assert.That(actualObj.PropertyAsInt64, Is.EqualTo(expectObj.PropertyAsInt64));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsInt64\":[\"9223372036854775807\"]}").PropertyAsInt64, Is.EqualTo(expectObj.PropertyAsInt64));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsInt64\":[9223372036854775807]}").PropertyAsInt64, Is.EqualTo(expectObj.PropertyAsInt64));

                Assert.That(actualObj.PropertyAsUInt64, Is.EqualTo(expectObj.PropertyAsUInt64));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsUInt64\":[\"18446744073709551615\"]}").PropertyAsUInt64, Is.EqualTo(expectObj.PropertyAsUInt64));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsUInt64\":[18446744073709551615]}").PropertyAsUInt64, Is.EqualTo(expectObj.PropertyAsUInt64));

                Assert.That(actualObj.PropertyAsFloat, Is.EqualTo(expectObj.PropertyAsFloat));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsFloat\":[\"1.23\"]}").PropertyAsFloat, Is.EqualTo(expectObj.PropertyAsFloat));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsFloat\":[1.23]}").PropertyAsFloat, Is.EqualTo(expectObj.PropertyAsFloat));

                Assert.That(actualObj.PropertyAsDouble, Is.EqualTo(expectObj.PropertyAsDouble));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsDouble\":[\"1.23\"]}").PropertyAsDouble, Is.EqualTo(expectObj.PropertyAsDouble));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsDouble\":[1.23]}").PropertyAsDouble, Is.EqualTo(expectObj.PropertyAsDouble));

                Assert.That(actualObj.PropertyAsDecimal, Is.EqualTo(expectObj.PropertyAsDecimal));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsDecimal\":[\"1.23\"]}").PropertyAsDecimal, Is.EqualTo(expectObj.PropertyAsDecimal));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsDecimal\":[1.23]}").PropertyAsDecimal, Is.EqualTo(expectObj.PropertyAsDecimal));
            });

            Assert.Multiple(() =>
            {
                var expectObj = new MockObject()
                {
                    PropertyAsNullableByte = new byte?[] { null, byte.MaxValue },
                    PropertyAsNullableSByte = new sbyte?[] { null, sbyte.MaxValue },
                    PropertyAsNullableInt16 = new short?[] { null, short.MaxValue },
                    PropertyAsNullableUInt16 = new ushort?[] { null, ushort.MaxValue },
                    PropertyAsNullableInt32 = new int?[] { null, int.MaxValue },
                    PropertyAsNullableUInt32 = new uint?[] { null, uint.MaxValue },
                    PropertyAsNullableInt64 = new long?[] { null, long.MaxValue },
                    PropertyAsNullableUInt64 = new ulong?[] { null, ulong.MaxValue },
                    PropertyAsNullableFloat = new float?[] { null, 1.23F, float.NaN, float.PositiveInfinity, float.NegativeInfinity },
                    PropertyAsNullableDouble = new double?[] { null, 1.23D, double.NaN, double.PositiveInfinity, double.NegativeInfinity },
                    PropertyAsNullableDecimal = new decimal?[] { null, 1.23M }
                };
                var actualJson = jsonSerializer.Serialize(expectObj);
                var actualObj = jsonSerializer.Deserialize<MockObject>(actualJson);

                Assert.That(actualJson, Does.Contain("\"PropertyAsNullableByte\":[null,\"255\"]"));
                Assert.That(actualJson, Does.Contain("\"PropertyAsNullableSByte\":[null,\"127\"]"));
                Assert.That(actualJson, Does.Contain("\"PropertyAsNullableInt16\":[null,\"32767\"]"));
                Assert.That(actualJson, Does.Contain("\"PropertyAsNullableUInt16\":[null,\"65535\"]"));
                Assert.That(actualJson, Does.Contain("\"PropertyAsNullableInt32\":[null,\"2147483647\"]"));
                Assert.That(actualJson, Does.Contain("\"PropertyAsNullableUInt32\":[null,\"4294967295\"]"));
                Assert.That(actualJson, Does.Contain("\"PropertyAsNullableInt64\":[null,\"9223372036854775807\"]"));
                Assert.That(actualJson, Does.Contain("\"PropertyAsNullableUInt64\":[null,\"18446744073709551615\"]"));
                Assert.That(actualJson, Does.Contain("\"PropertyAsNullableDecimal\":[null,\"1.23\"]"));

                Assert.That(actualObj.PropertyAsNullableByte, Is.EqualTo(expectObj.PropertyAsNullableByte));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableByte\":[\"\",\"255\"]}").PropertyAsNullableByte, Is.EqualTo(expectObj.PropertyAsNullableByte));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableByte\":[null,255]}").PropertyAsNullableByte, Is.EqualTo(expectObj.PropertyAsNullableByte));

                Assert.That(actualObj.PropertyAsNullableSByte, Is.EqualTo(expectObj.PropertyAsNullableSByte));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableSByte\":[\"\",\"127\"]}").PropertyAsNullableSByte, Is.EqualTo(expectObj.PropertyAsNullableSByte));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableSByte\":[null,127]}").PropertyAsNullableSByte, Is.EqualTo(expectObj.PropertyAsNullableSByte));

                Assert.That(actualObj.PropertyAsNullableInt16, Is.EqualTo(expectObj.PropertyAsNullableInt16));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableInt16\":[\"\",\"32767\"]}").PropertyAsNullableInt16, Is.EqualTo(expectObj.PropertyAsNullableInt16));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableInt16\":[null,32767]}").PropertyAsNullableInt16, Is.EqualTo(expectObj.PropertyAsNullableInt16));

                Assert.That(actualObj.PropertyAsNullableUInt16, Is.EqualTo(expectObj.PropertyAsNullableUInt16));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableUInt16\":[\"\",\"65535\"]}").PropertyAsNullableUInt16, Is.EqualTo(expectObj.PropertyAsNullableUInt16));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableUInt16\":[null,65535]}").PropertyAsNullableUInt16, Is.EqualTo(expectObj.PropertyAsNullableUInt16));

                Assert.That(actualObj.PropertyAsNullableInt32, Is.EqualTo(expectObj.PropertyAsNullableInt32));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableInt32\":[\"\",\"2147483647\"]}").PropertyAsNullableInt32, Is.EqualTo(expectObj.PropertyAsNullableInt32));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableInt32\":[null,2147483647]}").PropertyAsNullableInt32, Is.EqualTo(expectObj.PropertyAsNullableInt32));

                Assert.That(actualObj.PropertyAsNullableUInt32, Is.EqualTo(expectObj.PropertyAsNullableUInt32));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableUInt32\":[\"\",\"4294967295\"]}").PropertyAsNullableUInt32, Is.EqualTo(expectObj.PropertyAsNullableUInt32));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableUInt32\":[null,4294967295]}").PropertyAsNullableUInt32, Is.EqualTo(expectObj.PropertyAsNullableUInt32));

                Assert.That(actualObj.PropertyAsNullableInt64, Is.EqualTo(expectObj.PropertyAsNullableInt64));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableInt64\":[\"\",\"9223372036854775807\"]}").PropertyAsNullableInt64, Is.EqualTo(expectObj.PropertyAsNullableInt64));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableInt64\":[null,9223372036854775807]}").PropertyAsNullableInt64, Is.EqualTo(expectObj.PropertyAsNullableInt64));

                Assert.That(actualObj.PropertyAsNullableUInt64, Is.EqualTo(expectObj.PropertyAsNullableUInt64));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableUInt64\":[\"\",\"18446744073709551615\"]}").PropertyAsNullableUInt64, Is.EqualTo(expectObj.PropertyAsNullableUInt64));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableUInt64\":[null,18446744073709551615]}").PropertyAsNullableUInt64, Is.EqualTo(expectObj.PropertyAsNullableUInt64));

                Assert.That(actualObj.PropertyAsNullableFloat, Is.EqualTo(expectObj.PropertyAsNullableFloat));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableFloat\":[\"\",\"1.23\",\"NaN\",\"Infinity\",\"-Infinity\"]}").PropertyAsNullableFloat, Is.EqualTo(expectObj.PropertyAsNullableFloat));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableFloat\":[null,1.23,\"NaN\",\"Infinity\",\"-Infinity\"]}").PropertyAsNullableFloat, Is.EqualTo(expectObj.PropertyAsNullableFloat));

                Assert.That(actualObj.PropertyAsNullableDouble, Is.EqualTo(expectObj.PropertyAsNullableDouble));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableDouble\":[\"\",\"1.23\",\"NaN\",\"Infinity\",\"-Infinity\"]}").PropertyAsNullableDouble, Is.EqualTo(expectObj.PropertyAsNullableDouble));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableDouble\":[null,1.23,\"NaN\",\"Infinity\",\"-Infinity\"]}").PropertyAsNullableDouble, Is.EqualTo(expectObj.PropertyAsNullableDouble));

                Assert.That(actualObj.PropertyAsNullableDecimal, Is.EqualTo(expectObj.PropertyAsNullableDecimal));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableDecimal\":[\"\",\"1.23\"]}").PropertyAsNullableDecimal, Is.EqualTo(expectObj.PropertyAsNullableDecimal));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableDecimal\":[null,1.23]}").PropertyAsNullableDecimal, Is.EqualTo(expectObj.PropertyAsNullableDecimal));
            });
        }

        [Test(Description = "测试用例：自定义 Newtosoft.Json.JsonConverter 之 TextualNumberListConverter")]
        public void TestNewtosoftJsonConverter()
        {
            var jsonSettings = NewtonsoftJsonSerializer.GetDefaultSerializerSettings();
            jsonSettings.Formatting = Newtonsoft.Json.Formatting.None;
            jsonSettings.FloatFormatHandling = Newtonsoft.Json.FloatFormatHandling.String;

            TestCustomJsonConverter(new NewtonsoftJsonSerializer(jsonSettings));
        }
    }
}
