using NUnit.Framework;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases.JsonConverter
{
    using SKIT.FlurlHttpClient.Configuration;

    public class TestCase_JsonConverterOfTextualNumberArrayTest
    {
        private sealed class MockObject
        {
            [Newtonsoft.Json.JsonProperty(Order = 101)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberArrayConverter))]
            public byte[]? PropertyAsByte { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 102)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberArrayConverter))]
            public sbyte[]? PropertyAsSByte { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 103)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberArrayConverter))]
            public short[]? PropertyAsInt16 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 104)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberArrayConverter))]
            public ushort[]? PropertyAsUInt16 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 105)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberArrayConverter))]
            public int[]? PropertyAsInt32 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 106)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberArrayConverter))]
            public uint[]? PropertyAsUInt32 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 107)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberArrayConverter))]
            public long[]? PropertyAsInt64 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 108)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberArrayConverter))]
            public ulong[]? PropertyAsUInt64 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 109)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberArrayConverter))]
            public float[]? PropertyAsFloat { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 110)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberArrayConverter))]
            public double[]? PropertyAsDouble { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 111)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberArrayConverter))]
            public decimal[]? PropertyAsDecimal { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 201)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberArrayConverter))]
            public byte?[]? PropertyAsNullableByte { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 202)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberArrayConverter))]
            public sbyte?[]? PropertyAsNullableSByte { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 203)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberArrayConverter))]
            public short?[]? PropertyAsNullableInt16 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 204)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberArrayConverter))]
            public ushort?[]? PropertyAsNullableUInt16 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 205)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberArrayConverter))]
            public int?[]? PropertyAsNullableInt32 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 206)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberArrayConverter))]
            public uint?[]? PropertyAsNullableUInt32 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 207)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberArrayConverter))]
            public long?[]? PropertyAsNullableInt64 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 208)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberArrayConverter))]
            public ulong?[]? PropertyAsNullableUInt64 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 209)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberArrayConverter))]
            public float?[]? PropertyAsNullableFloat { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 210)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberArrayConverter))]
            public double?[]? PropertyAsNullableDouble { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 211)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberArrayConverter))]
            public decimal?[]? PropertyAsNullableDecimal { get; set; }
        }

        private static void TestCustomJsonConverter(IJsonSerializer jsonSerializer)
        {
            var mockObj1 = new MockObject()
            {
                PropertyAsByte = new byte[] { byte.MaxValue },
                PropertyAsSByte = new sbyte[] { sbyte.MaxValue },
                PropertyAsInt16 = new short[] { short.MaxValue },
                PropertyAsUInt16 = new ushort[] { ushort.MaxValue },
                PropertyAsInt32 = new int[] { int.MaxValue },
                PropertyAsUInt32 = new uint[] { uint.MaxValue },
                PropertyAsInt64 = new long[] { long.MaxValue },
                PropertyAsUInt64 = new ulong[] { ulong.MaxValue },
                PropertyAsFloat = new float[] { 1.23F },
                PropertyAsDouble = new double[] { 1.23D },
                PropertyAsDecimal = new decimal[] { 1.23M }
            };
            var actualJson1 = jsonSerializer.Serialize(mockObj1);
            var actualObj1 = jsonSerializer.Deserialize<MockObject>(actualJson1);
            StringAssert.Contains("\"PropertyAsByte\":[\"255\"]", actualJson1);
            StringAssert.Contains("\"PropertyAsSByte\":[\"127\"]", actualJson1);
            StringAssert.Contains("\"PropertyAsInt16\":[\"32767\"]", actualJson1);
            StringAssert.Contains("\"PropertyAsUInt16\":[\"65535\"]", actualJson1);
            StringAssert.Contains("\"PropertyAsInt32\":[\"2147483647\"]", actualJson1);
            StringAssert.Contains("\"PropertyAsUInt32\":[\"4294967295\"]", actualJson1);
            StringAssert.Contains("\"PropertyAsInt64\":[\"9223372036854775807\"]", actualJson1);
            StringAssert.Contains("\"PropertyAsUInt64\":[\"18446744073709551615\"]", actualJson1);
            StringAssert.Contains("\"PropertyAsDecimal\":[\"1.23\"]", actualJson1);
            CollectionAssert.AreEqual(mockObj1.PropertyAsByte, actualObj1.PropertyAsByte);
            CollectionAssert.AreEqual(mockObj1.PropertyAsByte, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsByte\":[255]}").PropertyAsByte);
            CollectionAssert.AreEqual(mockObj1.PropertyAsSByte, actualObj1.PropertyAsSByte);
            CollectionAssert.AreEqual(mockObj1.PropertyAsSByte, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsSByte\":[127]}").PropertyAsSByte);
            CollectionAssert.AreEqual(mockObj1.PropertyAsInt16, actualObj1.PropertyAsInt16);
            CollectionAssert.AreEqual(mockObj1.PropertyAsInt16, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsInt16\":[32767]}").PropertyAsInt16);
            CollectionAssert.AreEqual(mockObj1.PropertyAsUInt16, actualObj1.PropertyAsUInt16);
            CollectionAssert.AreEqual(mockObj1.PropertyAsUInt16, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsUInt16\":[65535]}").PropertyAsUInt16);
            CollectionAssert.AreEqual(mockObj1.PropertyAsInt32, actualObj1.PropertyAsInt32);
            CollectionAssert.AreEqual(mockObj1.PropertyAsInt32, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsInt32\":[2147483647]}").PropertyAsInt32);
            CollectionAssert.AreEqual(mockObj1.PropertyAsUInt32, actualObj1.PropertyAsUInt32);
            CollectionAssert.AreEqual(mockObj1.PropertyAsUInt32, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsUInt32\":[4294967295]}").PropertyAsUInt32);
            CollectionAssert.AreEqual(mockObj1.PropertyAsInt64, actualObj1.PropertyAsInt64);
            CollectionAssert.AreEqual(mockObj1.PropertyAsInt64, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsInt64\":[9223372036854775807]}").PropertyAsInt64);
            CollectionAssert.AreEqual(mockObj1.PropertyAsUInt64, actualObj1.PropertyAsUInt64);
            CollectionAssert.AreEqual(mockObj1.PropertyAsUInt64, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsUInt64\":[18446744073709551615]}").PropertyAsUInt64);
            CollectionAssert.AreEqual(mockObj1.PropertyAsFloat, actualObj1.PropertyAsFloat);
            CollectionAssert.AreEqual(mockObj1.PropertyAsFloat, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsFloat\":[1.23]}").PropertyAsFloat);
            CollectionAssert.AreEqual(mockObj1.PropertyAsDouble, actualObj1.PropertyAsDouble);
            CollectionAssert.AreEqual(mockObj1.PropertyAsDouble, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsDouble\":[1.23]}").PropertyAsDouble);
            CollectionAssert.AreEqual(mockObj1.PropertyAsDecimal, actualObj1.PropertyAsDecimal);
            CollectionAssert.AreEqual(mockObj1.PropertyAsDecimal, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsDecimal\":[1.23]}").PropertyAsDecimal);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableByte, actualObj1.PropertyAsNullableByte);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableByte, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableByte\":null}").PropertyAsNullableByte);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableSByte, actualObj1.PropertyAsNullableSByte);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableSByte, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableSByte\":null}").PropertyAsNullableSByte);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableInt16, actualObj1.PropertyAsNullableInt16);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableInt16, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableInt16\":null}").PropertyAsNullableInt16);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableUInt16, actualObj1.PropertyAsNullableUInt16);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableUInt16, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableUInt16\":null}").PropertyAsNullableUInt16);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableInt32, actualObj1.PropertyAsNullableInt32);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableInt32, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableInt32\":null}").PropertyAsNullableInt32);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableUInt32, actualObj1.PropertyAsNullableUInt32);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableUInt32, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableUInt32\":null}").PropertyAsNullableUInt32);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableInt64, actualObj1.PropertyAsNullableInt64);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableInt64, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableInt64\":null}").PropertyAsNullableInt64);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableUInt64, actualObj1.PropertyAsNullableUInt64);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableUInt64, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableUInt64\":null}").PropertyAsNullableUInt64);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableFloat, actualObj1.PropertyAsNullableFloat);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableFloat, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableFloat\":null}").PropertyAsNullableFloat);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableDouble, actualObj1.PropertyAsNullableDouble);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableDouble, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableDouble\":null}").PropertyAsNullableDouble);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableDecimal, actualObj1.PropertyAsNullableDecimal);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableDecimal, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableDecimal\":null}").PropertyAsNullableDecimal);

            var mockObj2 = new MockObject()
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
            var actualJson2 = jsonSerializer.Serialize(mockObj2);
            var actualObj2 = jsonSerializer.Deserialize<MockObject>(actualJson2);
            CollectionAssert.AreEqual(mockObj2.PropertyAsByte, actualObj2.PropertyAsByte);
            CollectionAssert.AreEqual(mockObj2.PropertyAsByte, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsByte\":null}").PropertyAsByte);
            CollectionAssert.AreEqual(mockObj2.PropertyAsSByte, actualObj2.PropertyAsSByte);
            CollectionAssert.AreEqual(mockObj2.PropertyAsSByte, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsSByte\":null}").PropertyAsSByte);
            CollectionAssert.AreEqual(mockObj2.PropertyAsInt16, actualObj2.PropertyAsInt16);
            CollectionAssert.AreEqual(mockObj2.PropertyAsInt16, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsInt16\":null}").PropertyAsInt16);
            CollectionAssert.AreEqual(mockObj2.PropertyAsUInt16, actualObj2.PropertyAsUInt16);
            CollectionAssert.AreEqual(mockObj2.PropertyAsUInt16, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsUInt16\":null}").PropertyAsUInt16);
            CollectionAssert.AreEqual(mockObj2.PropertyAsInt32, actualObj2.PropertyAsInt32);
            CollectionAssert.AreEqual(mockObj2.PropertyAsInt32, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsInt32\":null}").PropertyAsInt32);
            CollectionAssert.AreEqual(mockObj2.PropertyAsUInt32, actualObj2.PropertyAsUInt32);
            CollectionAssert.AreEqual(mockObj2.PropertyAsUInt32, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsUInt32\":null}").PropertyAsUInt32);
            CollectionAssert.AreEqual(mockObj2.PropertyAsInt64, actualObj2.PropertyAsInt64);
            CollectionAssert.AreEqual(mockObj2.PropertyAsInt64, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsInt64\":null}").PropertyAsInt64);
            CollectionAssert.AreEqual(mockObj2.PropertyAsUInt64, actualObj2.PropertyAsUInt64);
            CollectionAssert.AreEqual(mockObj2.PropertyAsUInt64, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsUInt64\":null}").PropertyAsUInt64);
            CollectionAssert.AreEqual(mockObj2.PropertyAsFloat, actualObj2.PropertyAsFloat);
            CollectionAssert.AreEqual(mockObj2.PropertyAsFloat, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsFloat\":null}").PropertyAsFloat);
            CollectionAssert.AreEqual(mockObj2.PropertyAsDouble, actualObj2.PropertyAsDouble);
            CollectionAssert.AreEqual(mockObj2.PropertyAsDouble, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsDouble\":null}").PropertyAsDouble);
            CollectionAssert.AreEqual(mockObj2.PropertyAsDecimal, actualObj2.PropertyAsDecimal);
            CollectionAssert.AreEqual(mockObj2.PropertyAsDecimal, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsDecimal\":null}").PropertyAsDecimal);
            StringAssert.Contains("\"PropertyAsNullableByte\":[null,\"255\"]", actualJson2);
            StringAssert.Contains("\"PropertyAsNullableSByte\":[null,\"127\"]", actualJson2);
            StringAssert.Contains("\"PropertyAsNullableInt16\":[null,\"32767\"]", actualJson2);
            StringAssert.Contains("\"PropertyAsNullableUInt16\":[null,\"65535\"]", actualJson2);
            StringAssert.Contains("\"PropertyAsNullableInt32\":[null,\"2147483647\"]", actualJson2);
            StringAssert.Contains("\"PropertyAsNullableUInt32\":[null,\"4294967295\"]", actualJson2);
            StringAssert.Contains("\"PropertyAsNullableInt64\":[null,\"9223372036854775807\"]", actualJson2);
            StringAssert.Contains("\"PropertyAsNullableUInt64\":[null,\"18446744073709551615\"]", actualJson2);
            StringAssert.Contains("\"PropertyAsNullableDecimal\":[null,\"1.23\"]", actualJson2);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableByte, actualObj2.PropertyAsNullableByte);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableByte, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableByte\":[null,255]}").PropertyAsNullableByte);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableSByte, actualObj2.PropertyAsNullableSByte);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableSByte, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableSByte\":[null,127]}").PropertyAsNullableSByte);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableInt16, actualObj2.PropertyAsNullableInt16);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableInt16, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableInt16\":[null,32767]}").PropertyAsNullableInt16);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableUInt16, actualObj2.PropertyAsNullableUInt16);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableUInt16, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableUInt16\":[null,65535]}").PropertyAsNullableUInt16);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableInt32, actualObj2.PropertyAsNullableInt32);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableInt32, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableInt32\":[null,2147483647]}").PropertyAsNullableInt32);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableUInt32, actualObj2.PropertyAsNullableUInt32);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableUInt32, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableUInt32\":[null,4294967295]}").PropertyAsNullableUInt32);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableInt64, actualObj2.PropertyAsNullableInt64);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableInt64, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableInt64\":[null,9223372036854775807]}").PropertyAsNullableInt64);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableUInt64, actualObj2.PropertyAsNullableUInt64);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableUInt64, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableUInt64\":[null,18446744073709551615]}").PropertyAsNullableUInt64);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableFloat, actualObj2.PropertyAsNullableFloat);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableFloat, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableFloat\":[null,1.23,\"NaN\",\"Infinity\",\"-Infinity\"]}").PropertyAsNullableFloat);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableDouble, actualObj2.PropertyAsNullableDouble);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableDouble, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableDouble\":[null,1.23,\"NaN\",\"Infinity\",\"-Infinity\"]}").PropertyAsNullableDouble);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableDecimal, actualObj2.PropertyAsNullableDecimal);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableDecimal, jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableDecimal\":[null,1.23]}").PropertyAsNullableDecimal);
        }

        [Test(Description = "测试用例：自定义 Newtosoft.Json.JsonConverter 之 TextualNumberArrayConverter")]
        public void TestNewtosoftJsonConverter()
        {
            var jsonSettings = NewtonsoftJsonSerializer.GetDefaultSerializerSettings();
            jsonSettings.Formatting = Newtonsoft.Json.Formatting.None;
            jsonSettings.FloatFormatHandling = Newtonsoft.Json.FloatFormatHandling.String;

            TestCustomJsonConverter(new NewtonsoftJsonSerializer(jsonSettings));
        }
    }
}