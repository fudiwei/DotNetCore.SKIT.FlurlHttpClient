using System.Collections.Generic;
using NUnit.Framework;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases
{
    public class TestCase_JsonConverterOfStringifiedNumberListWithPipeSplitTest
    {
        private sealed class MockObject
        {
            [Newtonsoft.Json.JsonProperty(Order = 101)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberListWithPipeSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(101)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.StringifiedNumberListWithPipeSplitConverter))]
            public IList<byte>? PropertyAsByte { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 102)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberListWithPipeSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(102)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.StringifiedNumberListWithPipeSplitConverter))]
            public IList<sbyte>? PropertyAsSByte { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 103)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberListWithPipeSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(103)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.StringifiedNumberListWithPipeSplitConverter))]
            public IList<short>? PropertyAsInt16 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 104)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberListWithPipeSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(104)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.StringifiedNumberListWithPipeSplitConverter))]
            public IList<ushort>? PropertyAsUInt16 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 105)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberListWithPipeSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(105)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.StringifiedNumberListWithPipeSplitConverter))]
            public IList<int>? PropertyAsInt32 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 106)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberListWithPipeSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(106)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.StringifiedNumberListWithPipeSplitConverter))]
            public IList<uint>? PropertyAsUInt32 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 107)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberListWithPipeSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(107)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.StringifiedNumberListWithPipeSplitConverter))]
            public IList<long>? PropertyAsInt64 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 108)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberListWithPipeSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(108)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.StringifiedNumberListWithPipeSplitConverter))]
            public IList<ulong>? PropertyAsUInt64 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 109)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberListWithPipeSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(109)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.StringifiedNumberListWithPipeSplitConverter))]
            public IList<float>? PropertyAsFloat { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 110)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberListWithPipeSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(110)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.StringifiedNumberListWithPipeSplitConverter))]
            public IList<double>? PropertyAsDouble { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 111)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberListWithPipeSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(111)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.StringifiedNumberListWithPipeSplitConverter))]
            public IList<decimal>? PropertyAsDecimal { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 201)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberListWithPipeSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(201)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.StringifiedNumberListWithPipeSplitConverter))]
            public IList<byte?>? PropertyAsNullableByte { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 202)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberListWithPipeSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(202)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.StringifiedNumberListWithPipeSplitConverter))]
            public IList<sbyte?>? PropertyAsNullableSByte { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 203)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberListWithPipeSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(203)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.StringifiedNumberListWithPipeSplitConverter))]
            public IList<short?>? PropertyAsNullableInt16 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 204)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberListWithPipeSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(204)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.StringifiedNumberListWithPipeSplitConverter))]
            public IList<ushort?>? PropertyAsNullableUInt16 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 205)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberListWithPipeSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(205)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.StringifiedNumberListWithPipeSplitConverter))]
            public IList<int?>? PropertyAsNullableInt32 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 206)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberListWithPipeSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(206)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.StringifiedNumberListWithPipeSplitConverter))]
            public IList<uint?>? PropertyAsNullableUInt32 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 207)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberListWithPipeSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(207)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.StringifiedNumberListWithPipeSplitConverter))]
            public IList<long?>? PropertyAsNullableInt64 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 208)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberListWithPipeSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(208)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.StringifiedNumberListWithPipeSplitConverter))]
            public IList<ulong?>? PropertyAsNullableUInt64 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 209)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberListWithPipeSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(209)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.StringifiedNumberListWithPipeSplitConverter))]
            public IList<float?>? PropertyAsNullableFloat { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 210)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberListWithPipeSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(210)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.StringifiedNumberListWithPipeSplitConverter))]
            public IList<double?>? PropertyAsNullableDouble { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 211)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedNumberListWithPipeSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(211)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.StringifiedNumberListWithPipeSplitConverter))]
            public IList<decimal?>? PropertyAsNullableDecimal { get; set; }
        }

        private static void TestCustomJsonConverter(IJsonSerializer jsonSerializer)
        {
            Assert.Multiple(() =>
            {
                var expectObj = new MockObject()
                {
                    PropertyAsByte = new List<byte>() { byte.MinValue, byte.MaxValue },
                    PropertyAsSByte = new List<sbyte>() { sbyte.MinValue, sbyte.MaxValue },
                    PropertyAsInt16 = new List<short>() { short.MinValue, short.MaxValue },
                    PropertyAsUInt16 = new List<ushort>() { ushort.MinValue, ushort.MaxValue },
                    PropertyAsInt32 = new List<int>() { int.MinValue, int.MaxValue },
                    PropertyAsUInt32 = new List<uint>() { uint.MinValue, uint.MaxValue },
                    PropertyAsInt64 = new List<long>() { long.MinValue, long.MaxValue },
                    PropertyAsUInt64 = new List<ulong>() { ulong.MinValue, ulong.MaxValue },
                    PropertyAsFloat = new List<float>() { -1.23F, 1.23F, float.NaN, float.PositiveInfinity, float.NegativeInfinity },
                    PropertyAsDouble = new List<double>() { -1.23D, 1.23D, double.NaN, double.PositiveInfinity, double.NegativeInfinity },
                    PropertyAsDecimal = new List<decimal>() { -1.23M, 1.23M }
                };
                var actualJson = jsonSerializer.Serialize(expectObj);
                var actualObj = jsonSerializer.Deserialize<MockObject>(actualJson);

                Assert.That(actualJson, Does.Contain("\"PropertyAsByte\":\"0|255\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsSByte\":\"-128|127\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsInt16\":\"-32768|32767\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsUInt16\":\"0|65535\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsInt32\":\"-2147483648|2147483647\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsUInt32\":\"0|4294967295\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsInt64\":\"-9223372036854775808|9223372036854775807\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsUInt64\":\"0|18446744073709551615\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsDecimal\":\"-1.23|1.23\""));

                Assert.That(actualObj.PropertyAsByte, Is.EqualTo(expectObj.PropertyAsByte));
                Assert.That(actualObj.PropertyAsSByte, Is.EqualTo(expectObj.PropertyAsSByte));
                Assert.That(actualObj.PropertyAsInt16, Is.EqualTo(expectObj.PropertyAsInt16));
                Assert.That(actualObj.PropertyAsUInt16, Is.EqualTo(expectObj.PropertyAsUInt16));
                Assert.That(actualObj.PropertyAsInt32, Is.EqualTo(expectObj.PropertyAsInt32));
                Assert.That(actualObj.PropertyAsUInt32, Is.EqualTo(expectObj.PropertyAsUInt32));
                Assert.That(actualObj.PropertyAsInt64, Is.EqualTo(expectObj.PropertyAsInt64));
                Assert.That(actualObj.PropertyAsUInt64, Is.EqualTo(expectObj.PropertyAsUInt64));
                Assert.That(actualObj.PropertyAsFloat, Is.EqualTo(expectObj.PropertyAsFloat));
                Assert.That(actualObj.PropertyAsDouble, Is.EqualTo(expectObj.PropertyAsDouble));
                Assert.That(actualObj.PropertyAsDecimal, Is.EqualTo(expectObj.PropertyAsDecimal));
                Assert.That(actualObj.PropertyAsNullableByte, Is.EqualTo(expectObj.PropertyAsNullableByte));
                Assert.That(actualObj.PropertyAsNullableSByte, Is.EqualTo(expectObj.PropertyAsNullableSByte));
                Assert.That(actualObj.PropertyAsNullableInt16, Is.EqualTo(expectObj.PropertyAsNullableInt16));
                Assert.That(actualObj.PropertyAsNullableUInt16, Is.EqualTo(expectObj.PropertyAsNullableUInt16));
                Assert.That(actualObj.PropertyAsNullableInt32, Is.EqualTo(expectObj.PropertyAsNullableInt32));
                Assert.That(actualObj.PropertyAsNullableUInt32, Is.EqualTo(expectObj.PropertyAsNullableUInt32));
                Assert.That(actualObj.PropertyAsNullableInt64, Is.EqualTo(expectObj.PropertyAsNullableInt64));
                Assert.That(actualObj.PropertyAsNullableUInt64, Is.EqualTo(expectObj.PropertyAsNullableUInt64));
                Assert.That(actualObj.PropertyAsNullableFloat, Is.EqualTo(expectObj.PropertyAsNullableFloat));
                Assert.That(actualObj.PropertyAsNullableDouble, Is.EqualTo(expectObj.PropertyAsNullableDouble));
                Assert.That(actualObj.PropertyAsNullableDecimal, Is.EqualTo(expectObj.PropertyAsNullableDecimal));
            });

            Assert.Multiple(() =>
            {
                var expectObj = new MockObject()
                {
                    PropertyAsNullableByte = new List<byte?>() { null, byte.MinValue, byte.MaxValue },
                    PropertyAsNullableSByte = new List<sbyte?>() { null, sbyte.MinValue, sbyte.MaxValue },
                    PropertyAsNullableInt16 = new List<short?>() { null, short.MinValue, short.MaxValue },
                    PropertyAsNullableUInt16 = new List<ushort?>() { null, ushort.MinValue, ushort.MaxValue },
                    PropertyAsNullableInt32 = new List<int?>() { null, int.MinValue, int.MaxValue },
                    PropertyAsNullableUInt32 = new List<uint?>() { null, uint.MinValue, uint.MaxValue },
                    PropertyAsNullableInt64 = new List<long?>() { null, long.MinValue, long.MaxValue },
                    PropertyAsNullableUInt64 = new List<ulong?>() { null, ulong.MinValue, ulong.MaxValue },
                    PropertyAsNullableFloat = new List<float?>() { null, -1.23F, 1.23F, float.NaN, float.PositiveInfinity, float.NegativeInfinity },
                    PropertyAsNullableDouble = new List<double?>() { null, -1.23D, 1.23D, double.NaN, double.PositiveInfinity, double.NegativeInfinity },
                    PropertyAsNullableDecimal = new List<decimal?>() { null, -1.23M, 1.23M }
                };
                var actualJson = jsonSerializer.Serialize(expectObj);
                var actualObj = jsonSerializer.Deserialize<MockObject>(actualJson);

                Assert.That(actualJson, Does.Contain("\"PropertyAsNullableByte\":\"|0|255\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsNullableSByte\":\"|-128|127\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsNullableInt16\":\"|-32768|32767\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsNullableUInt16\":\"|0|65535\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsNullableInt32\":\"|-2147483648|2147483647\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsNullableUInt32\":\"|0|4294967295\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsNullableInt64\":\"|-9223372036854775808|9223372036854775807\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsNullableUInt64\":\"|0|18446744073709551615\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsNullableDecimal\":\"|-1.23|1.23\""));

                Assert.That(actualObj.PropertyAsByte, Is.EqualTo(expectObj.PropertyAsByte));
                Assert.That(actualObj.PropertyAsSByte, Is.EqualTo(expectObj.PropertyAsSByte));
                Assert.That(actualObj.PropertyAsInt16, Is.EqualTo(expectObj.PropertyAsInt16));
                Assert.That(actualObj.PropertyAsUInt16, Is.EqualTo(expectObj.PropertyAsUInt16));
                Assert.That(actualObj.PropertyAsInt32, Is.EqualTo(expectObj.PropertyAsInt32));
                Assert.That(actualObj.PropertyAsUInt32, Is.EqualTo(expectObj.PropertyAsUInt32));
                Assert.That(actualObj.PropertyAsInt64, Is.EqualTo(expectObj.PropertyAsInt64));
                Assert.That(actualObj.PropertyAsUInt64, Is.EqualTo(expectObj.PropertyAsUInt64));
                Assert.That(actualObj.PropertyAsFloat, Is.EqualTo(expectObj.PropertyAsFloat));
                Assert.That(actualObj.PropertyAsDouble, Is.EqualTo(expectObj.PropertyAsDouble));
                Assert.That(actualObj.PropertyAsDecimal, Is.EqualTo(expectObj.PropertyAsDecimal));
                Assert.That(actualObj.PropertyAsNullableByte, Is.EqualTo(expectObj.PropertyAsNullableByte));
                Assert.That(actualObj.PropertyAsNullableSByte, Is.EqualTo(expectObj.PropertyAsNullableSByte));
                Assert.That(actualObj.PropertyAsNullableInt16, Is.EqualTo(expectObj.PropertyAsNullableInt16));
                Assert.That(actualObj.PropertyAsNullableUInt16, Is.EqualTo(expectObj.PropertyAsNullableUInt16));
                Assert.That(actualObj.PropertyAsNullableInt32, Is.EqualTo(expectObj.PropertyAsNullableInt32));
                Assert.That(actualObj.PropertyAsNullableUInt32, Is.EqualTo(expectObj.PropertyAsNullableUInt32));
                Assert.That(actualObj.PropertyAsNullableInt64, Is.EqualTo(expectObj.PropertyAsNullableInt64));
                Assert.That(actualObj.PropertyAsNullableUInt64, Is.EqualTo(expectObj.PropertyAsNullableUInt64));
                Assert.That(actualObj.PropertyAsNullableFloat, Is.EqualTo(expectObj.PropertyAsNullableFloat));
                Assert.That(actualObj.PropertyAsNullableDouble, Is.EqualTo(expectObj.PropertyAsNullableDouble));
                Assert.That(actualObj.PropertyAsNullableDecimal, Is.EqualTo(expectObj.PropertyAsNullableDecimal));
            });
        }

        [Test(Description = "测试用例：自定义 Newtosoft.Json.JsonConverter 之 TextualNumberListWithPipeSplitConverter")]
        public void TestNewtosoftJsonConverter()
        {
            var jsonSettings = NewtonsoftJsonSerializer.GetDefaultSerializerSettings();
            jsonSettings.Formatting = Newtonsoft.Json.Formatting.None;
            jsonSettings.FloatFormatHandling = Newtonsoft.Json.FloatFormatHandling.String;

            TestCustomJsonConverter(new NewtonsoftJsonSerializer(jsonSettings));
        }

        [Test(Description = "测试用例：自定义 System.Text.Json.Serialization.JsonConverter 之 TextualNumberListWithPipeSplitConverter")]
        public void TestSystemTextJsonConverter()
        {
            var jsonOptions = SystemTextJsonSerializer.GetDefaultSerializerOptions();
            jsonOptions.WriteIndented = false;
            jsonOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals;

            TestCustomJsonConverter(new SystemTextJsonSerializer(jsonOptions));
        }
    }
}
