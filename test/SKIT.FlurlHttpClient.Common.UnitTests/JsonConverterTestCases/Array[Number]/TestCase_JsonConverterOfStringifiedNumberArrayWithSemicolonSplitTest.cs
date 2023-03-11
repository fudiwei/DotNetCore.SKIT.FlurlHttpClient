using NUnit.Framework;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases.JsonConverter
{
    using SKIT.FlurlHttpClient.Configuration;

    public class TestCase_JsonConverterOfStringifiedNumberArrayWithSemicolonSplitTest
    {
        private sealed class MockObject
        {
            [Newtonsoft.Json.JsonProperty(Order = 101)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(101)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            public byte[]? PropertyAsByte { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 102)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(102)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            public sbyte[]? PropertyAsSByte { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 103)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(103)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            public short[]? PropertyAsInt16 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 104)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(104)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            public ushort[]? PropertyAsUInt16 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 105)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(105)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            public int[]? PropertyAsInt32 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 106)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(106)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            public uint[]? PropertyAsUInt32 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 107)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(107)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            public long[]? PropertyAsInt64 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 108)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(108)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            public ulong[]? PropertyAsUInt64 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 109)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(109)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            public float[]? PropertyAsFloat { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 110)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(110)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            public double[]? PropertyAsDouble { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 111)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(111)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            public decimal[]? PropertyAsDecimal { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 201)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(201)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            public byte?[]? PropertyAsNullableByte { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 202)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(202)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            public sbyte?[]? PropertyAsNullableSByte { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 203)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(203)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            public short?[]? PropertyAsNullableInt16 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 204)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(204)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            public ushort?[]? PropertyAsNullableUInt16 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 205)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(205)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            public int?[]? PropertyAsNullableInt32 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 206)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(206)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            public uint?[]? PropertyAsNullableUInt32 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 207)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(207)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            public long?[]? PropertyAsNullableInt64 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 208)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(208)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            public ulong?[]? PropertyAsNullableUInt64 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 209)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(209)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            public float?[]? PropertyAsNullableFloat { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 210)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(210)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            public double?[]? PropertyAsNullableDouble { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 211)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(211)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.StringifiedNumberArrayWithSemicolonSplitConverter))]
            public decimal?[]? PropertyAsNullableDecimal { get; set; }
        }

        private static void TestCustomJsonConverter(IJsonSerializer jsonSerializer)
        {
            var mockObj1 = new MockObject()
            {
                PropertyAsByte = new byte[] { byte.MinValue, byte.MaxValue },
                PropertyAsSByte = new sbyte[] { sbyte.MinValue, sbyte.MaxValue },
                PropertyAsInt16 = new short[] { short.MinValue, short.MaxValue },
                PropertyAsUInt16 = new ushort[] { ushort.MinValue, ushort.MaxValue },
                PropertyAsInt32 = new int[] { int.MinValue, int.MaxValue },
                PropertyAsUInt32 = new uint[] { uint.MinValue, uint.MaxValue },
                PropertyAsInt64 = new long[] { long.MinValue, long.MaxValue },
                PropertyAsUInt64 = new ulong[] { ulong.MinValue, ulong.MaxValue },
                PropertyAsFloat = new float[] { -1.23F, 1.23F },
                PropertyAsDouble = new double[] { -1.23D, 1.23D },
                PropertyAsDecimal = new decimal[] { -1.23M, 1.23M }
            };
            var actualJson1 = jsonSerializer.Serialize(mockObj1);
            var actualObj1 = jsonSerializer.Deserialize<MockObject>(actualJson1);
            StringAssert.Contains("\"PropertyAsByte\":\"0;255\"", actualJson1);
            StringAssert.Contains("\"PropertyAsSByte\":\"-128;127\"", actualJson1);
            StringAssert.Contains("\"PropertyAsInt16\":\"-32768;32767\"", actualJson1);
            StringAssert.Contains("\"PropertyAsUInt16\":\"0;65535\"", actualJson1);
            StringAssert.Contains("\"PropertyAsInt32\":\"-2147483648;2147483647\"", actualJson1);
            StringAssert.Contains("\"PropertyAsUInt32\":\"0;4294967295\"", actualJson1);
            StringAssert.Contains("\"PropertyAsInt64\":\"-9223372036854775808;9223372036854775807\"", actualJson1);
            StringAssert.Contains("\"PropertyAsUInt64\":\"0;18446744073709551615\"", actualJson1);
            StringAssert.Contains("\"PropertyAsDecimal\":\"-1.23;1.23\"", actualJson1);
            CollectionAssert.AreEqual(mockObj1.PropertyAsByte, actualObj1.PropertyAsByte);
            CollectionAssert.AreEqual(mockObj1.PropertyAsSByte, actualObj1.PropertyAsSByte);
            CollectionAssert.AreEqual(mockObj1.PropertyAsInt16, actualObj1.PropertyAsInt16);
            CollectionAssert.AreEqual(mockObj1.PropertyAsUInt16, actualObj1.PropertyAsUInt16);
            CollectionAssert.AreEqual(mockObj1.PropertyAsInt32, actualObj1.PropertyAsInt32);
            CollectionAssert.AreEqual(mockObj1.PropertyAsUInt32, actualObj1.PropertyAsUInt32);
            CollectionAssert.AreEqual(mockObj1.PropertyAsInt64, actualObj1.PropertyAsInt64);
            CollectionAssert.AreEqual(mockObj1.PropertyAsUInt64, actualObj1.PropertyAsUInt64);
            CollectionAssert.AreEqual(mockObj1.PropertyAsFloat, actualObj1.PropertyAsFloat);
            CollectionAssert.AreEqual(mockObj1.PropertyAsDouble, actualObj1.PropertyAsDouble);
            CollectionAssert.AreEqual(mockObj1.PropertyAsDecimal, actualObj1.PropertyAsDecimal);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableByte, actualObj1.PropertyAsNullableByte);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableSByte, actualObj1.PropertyAsNullableSByte);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableInt16, actualObj1.PropertyAsNullableInt16);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableUInt16, actualObj1.PropertyAsNullableUInt16);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableInt32, actualObj1.PropertyAsNullableInt32);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableUInt32, actualObj1.PropertyAsNullableUInt32);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableInt64, actualObj1.PropertyAsNullableInt64);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableUInt64, actualObj1.PropertyAsNullableUInt64);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableFloat, actualObj1.PropertyAsNullableFloat);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableDouble, actualObj1.PropertyAsNullableDouble);
            CollectionAssert.AreEqual(mockObj1.PropertyAsNullableDecimal, actualObj1.PropertyAsNullableDecimal);

            var mockObj2 = new MockObject()
            {
                PropertyAsNullableByte = new byte?[] { null, byte.MinValue, byte.MaxValue },
                PropertyAsNullableSByte = new sbyte?[] { null, sbyte.MinValue, sbyte.MaxValue },
                PropertyAsNullableInt16 = new short?[] { null, short.MinValue, short.MaxValue },
                PropertyAsNullableUInt16 = new ushort?[] { null, ushort.MinValue, ushort.MaxValue },
                PropertyAsNullableInt32 = new int?[] { null, int.MinValue, int.MaxValue },
                PropertyAsNullableUInt32 = new uint?[] { null, uint.MinValue, uint.MaxValue },
                PropertyAsNullableInt64 = new long?[] { null, long.MinValue, long.MaxValue },
                PropertyAsNullableUInt64 = new ulong?[] { null, ulong.MinValue, ulong.MaxValue },
                PropertyAsNullableFloat = new float?[] { null, -1.23F, 1.23F, float.NaN, float.PositiveInfinity, float.NegativeInfinity },
                PropertyAsNullableDouble = new double?[] { null, -1.23D, 1.23D, double.NaN, double.PositiveInfinity, double.NegativeInfinity },
                PropertyAsNullableDecimal = new decimal?[] { null, -1.23M, 1.23M }
            };
            var actualJson2 = jsonSerializer.Serialize(mockObj2);
            var actualObj2 = jsonSerializer.Deserialize<MockObject>(actualJson2);
            StringAssert.Contains("\"PropertyAsNullableByte\":\";0;255\"", actualJson2);
            StringAssert.Contains("\"PropertyAsNullableSByte\":\";-128;127\"", actualJson2);
            StringAssert.Contains("\"PropertyAsNullableInt16\":\";-32768;32767\"", actualJson2);
            StringAssert.Contains("\"PropertyAsNullableUInt16\":\";0;65535\"", actualJson2);
            StringAssert.Contains("\"PropertyAsNullableInt32\":\";-2147483648;2147483647\"", actualJson2);
            StringAssert.Contains("\"PropertyAsNullableUInt32\":\";0;4294967295\"", actualJson2);
            StringAssert.Contains("\"PropertyAsNullableInt64\":\";-9223372036854775808;9223372036854775807\"", actualJson2);
            StringAssert.Contains("\"PropertyAsNullableUInt64\":\";0;18446744073709551615\"", actualJson2);
            StringAssert.Contains("\"PropertyAsNullableDecimal\":\";-1.23;1.23\"", actualJson2);
            CollectionAssert.AreEqual(mockObj2.PropertyAsByte, actualObj2.PropertyAsByte);
            CollectionAssert.AreEqual(mockObj2.PropertyAsSByte, actualObj2.PropertyAsSByte);
            CollectionAssert.AreEqual(mockObj2.PropertyAsInt16, actualObj2.PropertyAsInt16);
            CollectionAssert.AreEqual(mockObj2.PropertyAsUInt16, actualObj2.PropertyAsUInt16);
            CollectionAssert.AreEqual(mockObj2.PropertyAsInt32, actualObj2.PropertyAsInt32);
            CollectionAssert.AreEqual(mockObj2.PropertyAsUInt32, actualObj2.PropertyAsUInt32);
            CollectionAssert.AreEqual(mockObj2.PropertyAsInt64, actualObj2.PropertyAsInt64);
            CollectionAssert.AreEqual(mockObj2.PropertyAsUInt64, actualObj2.PropertyAsUInt64);
            CollectionAssert.AreEqual(mockObj2.PropertyAsFloat, actualObj2.PropertyAsFloat);
            CollectionAssert.AreEqual(mockObj2.PropertyAsDouble, actualObj2.PropertyAsDouble);
            CollectionAssert.AreEqual(mockObj2.PropertyAsDecimal, actualObj2.PropertyAsDecimal);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableByte, actualObj2.PropertyAsNullableByte);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableSByte, actualObj2.PropertyAsNullableSByte);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableInt16, actualObj2.PropertyAsNullableInt16);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableUInt16, actualObj2.PropertyAsNullableUInt16);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableInt32, actualObj2.PropertyAsNullableInt32);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableUInt32, actualObj2.PropertyAsNullableUInt32);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableInt64, actualObj2.PropertyAsNullableInt64);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableUInt64, actualObj2.PropertyAsNullableUInt64);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableFloat, actualObj2.PropertyAsNullableFloat);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableDouble, actualObj2.PropertyAsNullableDouble);
            CollectionAssert.AreEqual(mockObj2.PropertyAsNullableDecimal, actualObj2.PropertyAsNullableDecimal);
        }

        [Test(Description = "测试用例：自定义 Newtosoft.Json.JsonConverter 之 TextualNumberArrayWithSemicolonSplitConverter")]
        public void TestNewtosoftJsonConverter()
        {
            var jsonSettings = NewtonsoftJsonSerializer.GetDefaultSerializerSettings();
            jsonSettings.Formatting = Newtonsoft.Json.Formatting.None;
            jsonSettings.FloatFormatHandling = Newtonsoft.Json.FloatFormatHandling.String;

            TestCustomJsonConverter(new NewtonsoftJsonSerializer(jsonSettings));
        }

        [Test(Description = "测试用例：自定义 System.Text.Json.Serialization.JsonConverter 之 TextualNumberArrayWithSemicolonSplitConverter")]
        public void TestSystemTextJsonConverter()
        {
            var jsonOptions = SystemTextJsonSerializer.GetDefaultSerializerOptions();
            jsonOptions.WriteIndented = false;
            jsonOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals;

            TestCustomJsonConverter(new SystemTextJsonSerializer(jsonOptions));
        }
    }
}
