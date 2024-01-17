using NUnit.Framework;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases.JsonConverter
{
    using SKIT.FlurlHttpClient.Configuration;

    public class TestCase_JsonConverterOfTextualNumberTest
    {
        private sealed class MockObject
        {
            [Newtonsoft.Json.JsonProperty(Order = 101)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(101)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.TextualNumberConverter))]
            public byte PropertyAsByte { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 102)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(102)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.TextualNumberConverter))]
            public sbyte PropertyAsSByte { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 103)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(103)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.TextualNumberConverter))]
            public short PropertyAsInt16 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 104)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(104)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.TextualNumberConverter))]
            public ushort PropertyAsUInt16 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 105)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(105)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.TextualNumberConverter))]
            public int PropertyAsInt32 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 106)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(106)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.TextualNumberConverter))]
            public uint PropertyAsUInt32 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 107)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(107)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.TextualNumberConverter))]
            public long PropertyAsInt64 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 108)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(108)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.TextualNumberConverter))]
            public ulong PropertyAsUInt64 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 109)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(109)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.TextualNumberConverter))]
            public float PropertyAsFloat { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 110)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(110)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.TextualNumberConverter))]
            public double PropertyAsDouble { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 111)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(111)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.TextualNumberConverter))]
            public decimal PropertyAsDecimal { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 201)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(201)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.TextualNumberConverter))]
            public byte? PropertyAsNullableByte { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 202)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(202)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.TextualNumberConverter))]
            public sbyte? PropertyAsNullableSByte { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 203)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(203)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.TextualNumberConverter))]
            public short? PropertyAsNullableInt16 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 204)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(204)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.TextualNumberConverter))]
            public ushort? PropertyAsNullableUInt16 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 205)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(205)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.TextualNumberConverter))]
            public int? PropertyAsNullableInt32 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 206)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(206)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.TextualNumberConverter))]
            public uint? PropertyAsNullableUInt32 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 207)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(207)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.TextualNumberConverter))]
            public long? PropertyAsNullableInt64 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 208)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(208)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.TextualNumberConverter))]
            public ulong? PropertyAsNullableUInt64 { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 209)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(209)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.TextualNumberConverter))]
            public float? PropertyAsNullableFloat { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 210)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(210)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.TextualNumberConverter))]
            public double? PropertyAsNullableDouble { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 211)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.TextualNumberConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(211)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.TextualNumberConverter))]
            public decimal? PropertyAsNullableDecimal { get; set; }
        }

        private static void TestCustomJsonConverter(IJsonSerializer jsonSerializer)
        {
            Assert.Multiple(() =>
            {
                var expectObj = new MockObject()
                {
                    PropertyAsByte = byte.MaxValue,
                    PropertyAsSByte = sbyte.MaxValue,
                    PropertyAsInt16 = short.MaxValue,
                    PropertyAsUInt16 = ushort.MaxValue,
                    PropertyAsInt32 = int.MaxValue,
                    PropertyAsUInt32 = uint.MaxValue,
                    PropertyAsInt64 = long.MaxValue,
                    PropertyAsUInt64 = ulong.MaxValue,
                    PropertyAsFloat = 1.23F,
                    PropertyAsDouble = 1.23D,
                    PropertyAsDecimal = 1.23M
                };
                var actualJson = jsonSerializer.Serialize(expectObj);
                var actualObj = jsonSerializer.Deserialize<MockObject>(actualJson);

                Assert.That(actualJson, Does.Contain("\"PropertyAsByte\":\"255\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsSByte\":\"127\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsInt16\":\"32767\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsUInt16\":\"65535\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsInt32\":\"2147483647\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsUInt32\":\"4294967295\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsInt64\":\"9223372036854775807\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsUInt64\":\"18446744073709551615\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsDecimal\":\"1.23\""));

                Assert.That(actualObj.PropertyAsByte, Is.EqualTo(expectObj.PropertyAsByte));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsByte\":255}").PropertyAsByte, Is.EqualTo(expectObj.PropertyAsByte));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsByte\":\"255\"}").PropertyAsByte, Is.EqualTo(expectObj.PropertyAsByte));

                Assert.That(actualObj.PropertyAsSByte, Is.EqualTo(expectObj.PropertyAsSByte));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsSByte\":127}").PropertyAsSByte, Is.EqualTo(expectObj.PropertyAsSByte));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsSByte\":\"127\"}").PropertyAsSByte, Is.EqualTo(expectObj.PropertyAsSByte));

                Assert.That(actualObj.PropertyAsInt16, Is.EqualTo(expectObj.PropertyAsInt16));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsInt16\":32767}").PropertyAsInt16, Is.EqualTo(expectObj.PropertyAsInt16));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsInt16\":\"32767\"}").PropertyAsInt16, Is.EqualTo(expectObj.PropertyAsInt16));

                Assert.That(actualObj.PropertyAsUInt16, Is.EqualTo(expectObj.PropertyAsUInt16));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsUInt16\":65535}").PropertyAsUInt16, Is.EqualTo(expectObj.PropertyAsUInt16));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsUInt16\":\"65535\"}").PropertyAsUInt16, Is.EqualTo(expectObj.PropertyAsUInt16));

                Assert.That(actualObj.PropertyAsInt32, Is.EqualTo(expectObj.PropertyAsInt32));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsInt32\":2147483647}").PropertyAsInt32, Is.EqualTo(expectObj.PropertyAsInt32));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsInt32\":\"2147483647\"}").PropertyAsInt32, Is.EqualTo(expectObj.PropertyAsInt32));

                Assert.That(actualObj.PropertyAsUInt32, Is.EqualTo(expectObj.PropertyAsUInt32));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsUInt32\":4294967295}").PropertyAsUInt32, Is.EqualTo(expectObj.PropertyAsUInt32));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsUInt32\":\"4294967295\"}").PropertyAsUInt32, Is.EqualTo(expectObj.PropertyAsUInt32));

                Assert.That(actualObj.PropertyAsInt64, Is.EqualTo(expectObj.PropertyAsInt64));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsInt64\":9223372036854775807}").PropertyAsInt64, Is.EqualTo(expectObj.PropertyAsInt64));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsInt64\":\"9223372036854775807\"}").PropertyAsInt64, Is.EqualTo(expectObj.PropertyAsInt64));

                Assert.That(actualObj.PropertyAsUInt64, Is.EqualTo(expectObj.PropertyAsUInt64));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsUInt64\":18446744073709551615}").PropertyAsUInt64, Is.EqualTo(expectObj.PropertyAsUInt64));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsUInt64\":\"18446744073709551615\"}").PropertyAsUInt64, Is.EqualTo(expectObj.PropertyAsUInt64));

                Assert.That(actualObj.PropertyAsFloat, Is.EqualTo(expectObj.PropertyAsFloat));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsFloat\":1.23}").PropertyAsFloat, Is.EqualTo(expectObj.PropertyAsFloat));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsFloat\":\"1.23\"}").PropertyAsFloat, Is.EqualTo(expectObj.PropertyAsFloat));

                Assert.That(actualObj.PropertyAsDouble, Is.EqualTo(expectObj.PropertyAsDouble));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsDouble\":1.23}").PropertyAsDouble, Is.EqualTo(expectObj.PropertyAsDouble));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsDouble\":\"1.23\"}").PropertyAsDouble, Is.EqualTo(expectObj.PropertyAsDouble));

                Assert.That(actualObj.PropertyAsDecimal, Is.EqualTo(expectObj.PropertyAsDecimal));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsDecimal\":1.23}").PropertyAsDecimal, Is.EqualTo(expectObj.PropertyAsDecimal));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsDecimal\":\"1.23\"}").PropertyAsDecimal, Is.EqualTo(expectObj.PropertyAsDecimal));

                Assert.That(actualObj.PropertyAsNullableByte, Is.EqualTo(expectObj.PropertyAsNullableByte));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableByte\":null}").PropertyAsNullableByte, Is.EqualTo(expectObj.PropertyAsNullableByte));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableByte\":\"\"}").PropertyAsNullableByte, Is.EqualTo(expectObj.PropertyAsNullableByte));

                Assert.That(actualObj.PropertyAsNullableSByte, Is.EqualTo(expectObj.PropertyAsNullableSByte));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableSByte\":null}").PropertyAsNullableSByte, Is.EqualTo(expectObj.PropertyAsNullableSByte));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableSByte\":\"\"}").PropertyAsNullableSByte, Is.EqualTo(expectObj.PropertyAsNullableSByte));

                Assert.That(actualObj.PropertyAsNullableInt16, Is.EqualTo(expectObj.PropertyAsNullableInt16));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableInt16\":null}").PropertyAsNullableInt16, Is.EqualTo(expectObj.PropertyAsNullableInt16));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableInt16\":\"\"}").PropertyAsNullableInt16, Is.EqualTo(expectObj.PropertyAsNullableInt16));

                Assert.That(actualObj.PropertyAsNullableUInt16, Is.EqualTo(expectObj.PropertyAsNullableUInt16));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableUInt16\":null}").PropertyAsNullableUInt16, Is.EqualTo(expectObj.PropertyAsNullableUInt16));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableUInt16\":\"\"}").PropertyAsNullableUInt16, Is.EqualTo(expectObj.PropertyAsNullableUInt16));

                Assert.That(actualObj.PropertyAsNullableInt32, Is.EqualTo(expectObj.PropertyAsNullableInt32));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableInt32\":null}").PropertyAsNullableInt32, Is.EqualTo(expectObj.PropertyAsNullableInt32));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableInt32\":\"\"}").PropertyAsNullableInt32, Is.EqualTo(expectObj.PropertyAsNullableInt32));

                Assert.That(actualObj.PropertyAsNullableUInt32, Is.EqualTo(expectObj.PropertyAsNullableUInt32));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableUInt32\":null}").PropertyAsNullableUInt32, Is.EqualTo(expectObj.PropertyAsNullableUInt32));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableUInt32\":\"\"}").PropertyAsNullableUInt32, Is.EqualTo(expectObj.PropertyAsNullableUInt32));

                Assert.That(actualObj.PropertyAsNullableInt64, Is.EqualTo(expectObj.PropertyAsNullableInt64));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableInt64\":null}").PropertyAsNullableInt64, Is.EqualTo(expectObj.PropertyAsNullableInt64));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableInt64\":\"\"}").PropertyAsNullableInt64, Is.EqualTo(expectObj.PropertyAsNullableInt64));

                Assert.That(actualObj.PropertyAsNullableUInt64, Is.EqualTo(expectObj.PropertyAsNullableUInt64));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableUInt64\":null}").PropertyAsNullableUInt64, Is.EqualTo(expectObj.PropertyAsNullableUInt64));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableUInt64\":\"\"}").PropertyAsNullableUInt64, Is.EqualTo(expectObj.PropertyAsNullableUInt64));

                Assert.That(actualObj.PropertyAsNullableFloat, Is.EqualTo(expectObj.PropertyAsNullableFloat));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableFloat\":null}").PropertyAsNullableFloat, Is.EqualTo(expectObj.PropertyAsNullableFloat));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableFloat\":\"\"}").PropertyAsNullableFloat, Is.EqualTo(expectObj.PropertyAsNullableFloat));

                Assert.That(actualObj.PropertyAsNullableDouble, Is.EqualTo(expectObj.PropertyAsNullableDouble));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableDouble\":null}").PropertyAsNullableDouble, Is.EqualTo(expectObj.PropertyAsNullableDouble));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableDouble\":\"\"}").PropertyAsNullableDouble, Is.EqualTo(expectObj.PropertyAsNullableDouble));

                Assert.That(actualObj.PropertyAsNullableDecimal, Is.EqualTo(expectObj.PropertyAsNullableDecimal));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableDecimal\":null}").PropertyAsNullableDecimal, Is.EqualTo(expectObj.PropertyAsNullableDecimal));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableDecimal\":\"\"}").PropertyAsNullableDecimal, Is.EqualTo(expectObj.PropertyAsNullableDecimal));
            });

            Assert.Multiple(() =>
            {
                var expectObj = new MockObject()
                {
                    PropertyAsNullableByte = byte.MaxValue,
                    PropertyAsNullableSByte = sbyte.MaxValue,
                    PropertyAsNullableInt16 = short.MaxValue,
                    PropertyAsNullableUInt16 = ushort.MaxValue,
                    PropertyAsNullableInt32 = int.MaxValue,
                    PropertyAsNullableUInt32 = uint.MaxValue,
                    PropertyAsNullableInt64 = long.MaxValue,
                    PropertyAsNullableUInt64 = ulong.MaxValue,
                    PropertyAsNullableFloat = 1.23F,
                    PropertyAsNullableDouble = 1.23D,
                    PropertyAsNullableDecimal = 1.23M
                };
                var actualJson = jsonSerializer.Serialize(expectObj);
                var actualObj = jsonSerializer.Deserialize<MockObject>(actualJson);

                Assert.That(actualJson, Does.Contain("\"PropertyAsNullableByte\":\"255\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsNullableSByte\":\"127\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsNullableInt16\":\"32767\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsNullableUInt16\":\"65535\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsNullableInt32\":\"2147483647\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsNullableUInt32\":\"4294967295\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsNullableInt64\":\"9223372036854775807\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsNullableUInt64\":\"18446744073709551615\""));
                Assert.That(actualJson, Does.Contain("\"PropertyAsNullableDecimal\":\"1.23\""));

                Assert.That(actualObj.PropertyAsByte, Is.EqualTo(expectObj.PropertyAsByte));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsByte\":null}").PropertyAsByte, Is.EqualTo(expectObj.PropertyAsByte));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsByte\":\"\"}").PropertyAsByte, Is.EqualTo(expectObj.PropertyAsByte));

                Assert.That(actualObj.PropertyAsSByte, Is.EqualTo(expectObj.PropertyAsSByte));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsSByte\":null}").PropertyAsSByte, Is.EqualTo(expectObj.PropertyAsSByte));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsSByte\":\"\"}").PropertyAsSByte, Is.EqualTo(expectObj.PropertyAsSByte));

                Assert.That(actualObj.PropertyAsInt16, Is.EqualTo(expectObj.PropertyAsInt16));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsInt16\":null}").PropertyAsInt16, Is.EqualTo(expectObj.PropertyAsInt16));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsInt16\":\"\"}").PropertyAsInt16, Is.EqualTo(expectObj.PropertyAsInt16));

                Assert.That(actualObj.PropertyAsUInt16, Is.EqualTo(expectObj.PropertyAsUInt16));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsUInt16\":null}").PropertyAsUInt16, Is.EqualTo(expectObj.PropertyAsUInt16));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsUInt16\":\"\"}").PropertyAsUInt16, Is.EqualTo(expectObj.PropertyAsUInt16));

                Assert.That(actualObj.PropertyAsInt32, Is.EqualTo(expectObj.PropertyAsInt32));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsInt32\":null}").PropertyAsInt32, Is.EqualTo(expectObj.PropertyAsInt32));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsInt32\":\"\"}").PropertyAsInt32, Is.EqualTo(expectObj.PropertyAsInt32));

                Assert.That(actualObj.PropertyAsUInt32, Is.EqualTo(expectObj.PropertyAsUInt32));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsUInt32\":null}").PropertyAsUInt32, Is.EqualTo(expectObj.PropertyAsUInt32));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsUInt32\":\"\"}").PropertyAsUInt32, Is.EqualTo(expectObj.PropertyAsUInt32));

                Assert.That(actualObj.PropertyAsInt64, Is.EqualTo(expectObj.PropertyAsInt64));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsInt64\":null}").PropertyAsInt64, Is.EqualTo(expectObj.PropertyAsInt64));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsInt64\":\"\"}").PropertyAsInt64, Is.EqualTo(expectObj.PropertyAsInt64));

                Assert.That(actualObj.PropertyAsUInt64, Is.EqualTo(expectObj.PropertyAsUInt64));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsUInt64\":null}").PropertyAsUInt64, Is.EqualTo(expectObj.PropertyAsUInt64));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsUInt64\":\"\"}").PropertyAsUInt64, Is.EqualTo(expectObj.PropertyAsUInt64));

                Assert.That(actualObj.PropertyAsFloat, Is.EqualTo(expectObj.PropertyAsFloat));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsFloat\":null}").PropertyAsFloat, Is.EqualTo(expectObj.PropertyAsFloat));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsFloat\":\"\"}").PropertyAsFloat, Is.EqualTo(expectObj.PropertyAsFloat));

                Assert.That(actualObj.PropertyAsDouble, Is.EqualTo(expectObj.PropertyAsDouble));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsDouble\":null}").PropertyAsDouble, Is.EqualTo(expectObj.PropertyAsDouble));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsDouble\":\"\"}").PropertyAsDouble, Is.EqualTo(expectObj.PropertyAsDouble));

                Assert.That(actualObj.PropertyAsDecimal, Is.EqualTo(expectObj.PropertyAsDecimal));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsDecimal\":null}").PropertyAsDecimal, Is.EqualTo(expectObj.PropertyAsDecimal));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsDecimal\":\"\"}").PropertyAsDecimal, Is.EqualTo(expectObj.PropertyAsDecimal));

                Assert.That(actualObj.PropertyAsNullableByte, Is.EqualTo(expectObj.PropertyAsNullableByte));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableByte\":255}").PropertyAsNullableByte, Is.EqualTo(expectObj.PropertyAsNullableByte));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableByte\":\"255\"}").PropertyAsNullableByte, Is.EqualTo(expectObj.PropertyAsNullableByte));

                Assert.That(actualObj.PropertyAsNullableSByte, Is.EqualTo(expectObj.PropertyAsNullableSByte));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableSByte\":127}").PropertyAsNullableSByte, Is.EqualTo(expectObj.PropertyAsNullableSByte));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableSByte\":\"127\"}").PropertyAsNullableSByte, Is.EqualTo(expectObj.PropertyAsNullableSByte));

                Assert.That(actualObj.PropertyAsNullableInt16, Is.EqualTo(expectObj.PropertyAsNullableInt16));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableInt16\":32767}").PropertyAsNullableInt16, Is.EqualTo(expectObj.PropertyAsNullableInt16));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableInt16\":\"32767\"}").PropertyAsNullableInt16, Is.EqualTo(expectObj.PropertyAsNullableInt16));

                Assert.That(actualObj.PropertyAsNullableUInt16, Is.EqualTo(expectObj.PropertyAsNullableUInt16));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableUInt16\":65535}").PropertyAsNullableUInt16, Is.EqualTo(expectObj.PropertyAsNullableUInt16));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableUInt16\":\"65535\"}").PropertyAsNullableUInt16, Is.EqualTo(expectObj.PropertyAsNullableUInt16));

                Assert.That(actualObj.PropertyAsNullableInt32, Is.EqualTo(expectObj.PropertyAsNullableInt32));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableInt32\":2147483647}").PropertyAsNullableInt32, Is.EqualTo(expectObj.PropertyAsNullableInt32));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableInt32\":\"2147483647\"}").PropertyAsNullableInt32, Is.EqualTo(expectObj.PropertyAsNullableInt32));

                Assert.That(actualObj.PropertyAsNullableUInt32, Is.EqualTo(expectObj.PropertyAsNullableUInt32));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableUInt32\":4294967295}").PropertyAsNullableUInt32, Is.EqualTo(expectObj.PropertyAsNullableUInt32));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableUInt32\":\"4294967295\"}").PropertyAsNullableUInt32, Is.EqualTo(expectObj.PropertyAsNullableUInt32));

                Assert.That(actualObj.PropertyAsNullableInt64, Is.EqualTo(expectObj.PropertyAsNullableInt64));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableInt64\":9223372036854775807}").PropertyAsNullableInt64, Is.EqualTo(expectObj.PropertyAsNullableInt64));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableInt64\":\"9223372036854775807\"}").PropertyAsNullableInt64, Is.EqualTo(expectObj.PropertyAsNullableInt64));

                Assert.That(actualObj.PropertyAsNullableUInt64, Is.EqualTo(expectObj.PropertyAsNullableUInt64));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableUInt64\":18446744073709551615}").PropertyAsNullableUInt64, Is.EqualTo(expectObj.PropertyAsNullableUInt64));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableUInt64\":\"18446744073709551615\"}").PropertyAsNullableUInt64, Is.EqualTo(expectObj.PropertyAsNullableUInt64));

                Assert.That(actualObj.PropertyAsNullableFloat, Is.EqualTo(expectObj.PropertyAsNullableFloat));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableFloat\":1.23}").PropertyAsNullableFloat, Is.EqualTo(expectObj.PropertyAsNullableFloat));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableFloat\":\"1.23\"}").PropertyAsNullableFloat, Is.EqualTo(expectObj.PropertyAsNullableFloat));

                Assert.That(actualObj.PropertyAsNullableDouble, Is.EqualTo(expectObj.PropertyAsNullableDouble));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableDouble\":1.23}").PropertyAsNullableDouble, Is.EqualTo(expectObj.PropertyAsNullableDouble));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableDouble\":\"1.23\"}").PropertyAsNullableDouble, Is.EqualTo(expectObj.PropertyAsNullableDouble));

                Assert.That(actualObj.PropertyAsNullableDecimal, Is.EqualTo(expectObj.PropertyAsNullableDecimal));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableDecimal\":1.23}").PropertyAsNullableDecimal, Is.EqualTo(expectObj.PropertyAsNullableDecimal));
                Assert.That(jsonSerializer.Deserialize<MockObject>("{\"PropertyAsNullableDecimal\":\"1.23\"}").PropertyAsNullableDecimal, Is.EqualTo(expectObj.PropertyAsNullableDecimal));
            });
        }

        [Test(Description = "测试用例：自定义 Newtosoft.Json.JsonConverter 之 TextualNumberConverter")]
        public void TestNewtosoftJsonConverter()
        {
            var jsonSettings = NewtonsoftJsonSerializer.GetDefaultSerializerSettings();
            jsonSettings.Formatting = Newtonsoft.Json.Formatting.None;
            jsonSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

            TestCustomJsonConverter(new NewtonsoftJsonSerializer(jsonSettings));
        }

        [Test(Description = "测试用例：自定义 System.Text.Json.JsonConverter 之 TextualNumberConverter")]
        public void TestSystemTextJsonConverter()
        {
            var jsonOptions = SystemTextJsonSerializer.GetDefaultSerializerOptions();
            jsonOptions.WriteIndented = false;
            jsonOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;

            TestCustomJsonConverter(new SystemTextJsonSerializer(jsonOptions));
        }
    }
}
