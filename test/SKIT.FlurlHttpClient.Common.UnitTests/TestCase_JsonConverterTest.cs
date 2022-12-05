using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SKIT.FlurlHttpClient.UnitTests
{
    public class TestCase_JsonConverterTest
    {
        private sealed class InnerFakeClass
        {
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.NumericalBooleanConverter))]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.NumericalBooleanConverter))]
            public bool NumericalBooleanProperty { get; set; } = true;

            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.NumericalNullableBooleanConverter))]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.NumericalNullableBooleanConverter))]
            public bool? NumericalNullableBooleanProperty { get; set; } = true;

            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.TextualBooleanConverter))]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.TextualBooleanConverter))]
            public bool TextualBooleanProperty { get; set; } = true;

            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.TextualNullableBooleanConverter))]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.TextualNullableBooleanConverter))]
            public bool? TextualNullableBooleanProperty { get; set; } = true;

            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.NumericalStringConverter))]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.NumericalStringConverter))]
            public string NumericalStringProperty { get; set; } = "a";

            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.TextualDoubleConverter))]
            [System.Text.Json.Serialization.JsonNumberHandling(System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString | System.Text.Json.Serialization.JsonNumberHandling.WriteAsString)]
            public double TextualDoubleProperty { get; set; } = 1.1;

            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.TextualNullableDoubleConverter))]
            [System.Text.Json.Serialization.JsonNumberHandling(System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString | System.Text.Json.Serialization.JsonNumberHandling.WriteAsString)]
            public double? TextualNullableDoubleProperty { get; set; } = 1.1;

            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.TextualIntegerConverter))]
            [System.Text.Json.Serialization.JsonNumberHandling(System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString | System.Text.Json.Serialization.JsonNumberHandling.WriteAsString)]
            public int TextualIntegerProperty { get; set; } = 1;

            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.TextualNullableIntegerConverter))]
            [System.Text.Json.Serialization.JsonNumberHandling(System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString | System.Text.Json.Serialization.JsonNumberHandling.WriteAsString)]
            public int? TextualNullableIntegerProperty { get; set; } = 1;

            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.TextualLongConverter))]
            [System.Text.Json.Serialization.JsonNumberHandling(System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString | System.Text.Json.Serialization.JsonNumberHandling.WriteAsString)]
            public long TextualLongProperty { get; set; } = 1;

            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.TextualNullableLongConverter))]
            [System.Text.Json.Serialization.JsonNumberHandling(System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString | System.Text.Json.Serialization.JsonNumberHandling.WriteAsString)]
            public long? TextualNullableLongProperty { get; set; } = 1;

            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.RegularDateTimeOffsetConverter))]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.RegularDateTimeOffsetConverter))]
            public DateTimeOffset RegularDateTimeOffsetProperty { get; set; } = DateTimeOffset.Parse("2022-01-01");

            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.RegularNullableDateTimeOffsetConverter))]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.RegularNullableDateTimeOffsetConverter))]
            public DateTimeOffset? RegularNullableDateTimeOffsetProperty { get; set; } = DateTimeOffset.Parse("2022-01-01");

            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.RFC3339DateTimeOffsetConverter))]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.RFC3339DateTimeOffsetConverter))]
            public DateTimeOffset RFC3339DateTimeOffsetProperty { get; set; } = DateTimeOffset.Parse("2022-01-01");

            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.RFC3339NullableDateTimeOffsetConverter))]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.RFC3339NullableDateTimeOffsetConverter))]
            public DateTimeOffset? RFC3339NullableDateTimeOffsetProperty { get; set; } = DateTimeOffset.Parse("2022-01-01");

            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.UnixMillisecondsDateTimeOffsetConverter))]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.UnixMillisecondsDateTimeOffsetConverter))]
            public DateTimeOffset UnixMillisecondsDateTimeOffsetProperty { get; set; } = DateTimeOffset.Parse("2022-01-01");

            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.UnixMillisecondsNullableDateTimeOffsetConverter))]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.UnixMillisecondsNullableDateTimeOffsetConverter))]
            public DateTimeOffset? UnixMillisecondsNullableDateTimeOffsetProperty { get; set; } = DateTimeOffset.Parse("2022-01-01");

            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.UnixTimestampDateTimeOffsetConverter))]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.UnixTimestampDateTimeOffsetConverter))]
            public DateTimeOffset UnixTimestampDateTimeOffsetProperty { get; set; } = DateTimeOffset.Parse("2022-01-01");

            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.UnixTimestampNullableDateTimeOffsetConverter))]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.UnixTimestampNullableDateTimeOffsetConverter))]
            public DateTimeOffset? UnixTimestampNullableDateTimeOffsetProperty { get; set; } = DateTimeOffset.Parse("2022-01-01");

            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.TextualDoubleArrayWithCommaSplitConverter))]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.TextualDoubleArrayWithCommaSplitConverter))]
            public double[] TextualDoubleArrayWithCommaSplitProperty { get; set; } = new double[] { 1.1, 2.2, 3.3 };

            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.TextualIntegerArrayWithCommaSplitConverter))]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.TextualIntegerArrayWithCommaSplitConverter))]
            public int[] TextualIntegerArrayWithCommaSplitProperty { get; set; } = new int[] { 1, 2, 3 };

            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.TextualLongArrayWithCommaSplitConverter))]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.TextualLongArrayWithCommaSplitConverter))]
            public long[] TextualLongArrayWithCommaSplitProperty { get; set; } = new long[] { 1, 2, 3 };

            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.TextualStringArrayWithCommaSplitConverter))]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.TextualStringArrayWithCommaSplitConverter))]
            public string[] TextualStringArrayWithCommaSplitProperty { get; set; } = new string[] { "a", "b", "c" };

            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.TextualDoubleListWithCommaSplitConverter))]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.TextualDoubleListWithCommaSplitConverter))]
            public IList<double> TextualDoubleListWithCommaSplitProperty { get; set; } = new List<double>() { 1.1, 2.2, 3.3 };

            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.TextualIntegerListWithCommaSplitConverter))]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.TextualIntegerListWithCommaSplitConverter))]
            public IList<int> TextualIntegerListWithCommaSplitProperty { get; set; } = new List<int>() { 1, 2, 3 };

            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.TextualLongListWithCommaSplitConverter))]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.TextualLongListWithCommaSplitConverter))]
            public IList<long> TextualLongListWithCommaSplitProperty { get; set; } = new List<long>() { 1, 2, 3 };

            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.TextualStringListWithCommaSplitConverter))]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.TextualStringListWithCommaSplitConverter))]
            public IList<string> TextualStringListWithCommaSplitProperty { get; set; } = new List<string>() { "a", "b", "c" };

            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.DynamicObjectConverter))]
            public dynamic DynamicObjectConverter_Boolean { get; set; } = true;

            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.DynamicObjectConverter))]
            public dynamic DynamicObjectConverter_Number { get; set; } = 1;

            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.DynamicObjectConverter))]
            public dynamic DynamicObjectConverter_String { get; set; } = "a";

            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.DynamicObjectConverter))]
            public dynamic DynamicObjectConverter_Guid { get; set; } = Guid.Parse("11112222-3333-4444-5555-666677778888");

            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.DynamicObjectConverter))]
            public dynamic DynamicObjectConverter_Array { get; set; } = new object[] { true, 1, "a" };

            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.DynamicObjectConverter))]
            public dynamic DynamicObjectConverter_List { get; set; } = new List<object>() { true, 1, "a" };

            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.DynamicObjectConverter))]
            public dynamic DynamicObjectConverter_Dictionary { get; set; } = new Dictionary<string, object>() { { "k1", true }, { "k2", 1 }, { "k3", "a" } };

            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.DynamicObjectConverter))]
            public dynamic DynamicObjectConverter_AnonymousObject { get; set; } = new { k1 = true, k2 = 1, k3 = "a" };
        }

        [Test(Description = "??????????????? Newtosoft.Json ???л??????")]
        public void TestCustomNewtosoftJsonConverters()
        {
            var serializerSettings = FlurlNewtonsoftJsonSerializer.GetDefaultSerializerSettings();
            serializerSettings.Formatting = Newtonsoft.Json.Formatting.None;
            serializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            var serializer = new FlurlNewtonsoftJsonSerializer(serializerSettings);

            var rawObj = new InnerFakeClass();
            var rawJson = serializer.Serialize(rawObj);
            var parsedObj = serializer.Deserialize<InnerFakeClass>(rawJson);

            Assert.AreEqual(rawObj.NumericalBooleanProperty, parsedObj.NumericalBooleanProperty);
            Assert.AreEqual(rawObj.NumericalNullableBooleanProperty, parsedObj.NumericalNullableBooleanProperty);
            Assert.AreEqual(rawObj.TextualBooleanProperty, parsedObj.TextualBooleanProperty);
            Assert.AreEqual(rawObj.TextualNullableBooleanProperty, parsedObj.TextualNullableBooleanProperty);
            Assert.AreEqual(rawObj.NumericalStringProperty, parsedObj.NumericalStringProperty);
            Assert.AreEqual(rawObj.TextualDoubleProperty, parsedObj.TextualDoubleProperty);
            Assert.AreEqual(rawObj.TextualNullableDoubleProperty, parsedObj.TextualNullableDoubleProperty);
            Assert.AreEqual(rawObj.TextualIntegerProperty, parsedObj.TextualIntegerProperty);
            Assert.AreEqual(rawObj.TextualNullableIntegerProperty, parsedObj.TextualNullableIntegerProperty);
            Assert.AreEqual(rawObj.TextualLongProperty, parsedObj.TextualLongProperty);
            Assert.AreEqual(rawObj.TextualNullableLongProperty, parsedObj.TextualNullableLongProperty);
            Assert.AreEqual(rawObj.RegularDateTimeOffsetProperty, parsedObj.RegularDateTimeOffsetProperty);
            Assert.AreEqual(rawObj.RegularNullableDateTimeOffsetProperty, parsedObj.RegularNullableDateTimeOffsetProperty);
            Assert.AreEqual(rawObj.RFC3339DateTimeOffsetProperty, parsedObj.RFC3339DateTimeOffsetProperty);
            Assert.AreEqual(rawObj.RFC3339NullableDateTimeOffsetProperty, parsedObj.RFC3339NullableDateTimeOffsetProperty);
            Assert.AreEqual(rawObj.UnixMillisecondsDateTimeOffsetProperty, parsedObj.UnixMillisecondsDateTimeOffsetProperty);
            Assert.AreEqual(rawObj.UnixMillisecondsNullableDateTimeOffsetProperty, parsedObj.UnixMillisecondsNullableDateTimeOffsetProperty);
            Assert.AreEqual(rawObj.UnixTimestampDateTimeOffsetProperty, parsedObj.UnixTimestampDateTimeOffsetProperty);
            Assert.AreEqual(rawObj.UnixTimestampNullableDateTimeOffsetProperty, parsedObj.UnixTimestampNullableDateTimeOffsetProperty);
            CollectionAssert.AreEqual(rawObj.TextualDoubleArrayWithCommaSplitProperty, parsedObj.TextualDoubleArrayWithCommaSplitProperty);
            CollectionAssert.AreEqual(rawObj.TextualIntegerArrayWithCommaSplitProperty, parsedObj.TextualIntegerArrayWithCommaSplitProperty);
            CollectionAssert.AreEqual(rawObj.TextualLongArrayWithCommaSplitProperty, parsedObj.TextualLongArrayWithCommaSplitProperty);
            CollectionAssert.AreEqual(rawObj.TextualStringArrayWithCommaSplitProperty, parsedObj.TextualStringArrayWithCommaSplitProperty);
            CollectionAssert.AreEqual(rawObj.TextualDoubleListWithCommaSplitProperty, parsedObj.TextualDoubleListWithCommaSplitProperty);
            CollectionAssert.AreEqual(rawObj.TextualIntegerListWithCommaSplitProperty, parsedObj.TextualIntegerListWithCommaSplitProperty);
            CollectionAssert.AreEqual(rawObj.TextualLongListWithCommaSplitProperty, parsedObj.TextualLongListWithCommaSplitProperty);
            CollectionAssert.AreEqual(rawObj.TextualStringListWithCommaSplitProperty, parsedObj.TextualStringListWithCommaSplitProperty);
        }

        [Test(Description = "??????????????? System.Text.Json ???л??????")]
        public void TestCustomSystemTextJsonConverters()
        {
            var serializerOptions = FlurlSystemTextJsonSerializer.GetDefaultSerializerOptions();
            serializerOptions.WriteIndented = true;
            serializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            var serializer = new FlurlSystemTextJsonSerializer(serializerOptions);

            var rawObj = new InnerFakeClass();
            var rawJson = serializer.Serialize(rawObj);
            var parsedObj = serializer.Deserialize<InnerFakeClass>(rawJson);

            Assert.AreEqual(rawObj.NumericalBooleanProperty, parsedObj.NumericalBooleanProperty);
            Assert.AreEqual(rawObj.NumericalNullableBooleanProperty, parsedObj.NumericalNullableBooleanProperty);
            Assert.AreEqual(rawObj.TextualBooleanProperty, parsedObj.TextualBooleanProperty);
            Assert.AreEqual(rawObj.TextualNullableBooleanProperty, parsedObj.TextualNullableBooleanProperty);
            Assert.AreEqual(rawObj.NumericalStringProperty, parsedObj.NumericalStringProperty);
            Assert.AreEqual(rawObj.TextualDoubleProperty, parsedObj.TextualDoubleProperty);
            Assert.AreEqual(rawObj.TextualNullableDoubleProperty, parsedObj.TextualNullableDoubleProperty);
            Assert.AreEqual(rawObj.TextualIntegerProperty, parsedObj.TextualIntegerProperty);
            Assert.AreEqual(rawObj.TextualNullableIntegerProperty, parsedObj.TextualNullableIntegerProperty);
            Assert.AreEqual(rawObj.TextualLongProperty, parsedObj.TextualLongProperty);
            Assert.AreEqual(rawObj.TextualNullableLongProperty, parsedObj.TextualNullableLongProperty);
            Assert.AreEqual(rawObj.RegularDateTimeOffsetProperty, parsedObj.RegularDateTimeOffsetProperty);
            Assert.AreEqual(rawObj.RegularNullableDateTimeOffsetProperty, parsedObj.RegularNullableDateTimeOffsetProperty);
            Assert.AreEqual(rawObj.RFC3339DateTimeOffsetProperty, parsedObj.RFC3339DateTimeOffsetProperty);
            Assert.AreEqual(rawObj.RFC3339NullableDateTimeOffsetProperty, parsedObj.RFC3339NullableDateTimeOffsetProperty);
            Assert.AreEqual(rawObj.UnixMillisecondsDateTimeOffsetProperty, parsedObj.UnixMillisecondsDateTimeOffsetProperty);
            Assert.AreEqual(rawObj.UnixMillisecondsNullableDateTimeOffsetProperty, parsedObj.UnixMillisecondsNullableDateTimeOffsetProperty);
            Assert.AreEqual(rawObj.UnixTimestampDateTimeOffsetProperty, parsedObj.UnixTimestampDateTimeOffsetProperty);
            Assert.AreEqual(rawObj.UnixTimestampNullableDateTimeOffsetProperty, parsedObj.UnixTimestampNullableDateTimeOffsetProperty);
            CollectionAssert.AreEqual(rawObj.TextualDoubleArrayWithCommaSplitProperty, parsedObj.TextualDoubleArrayWithCommaSplitProperty);
            CollectionAssert.AreEqual(rawObj.TextualIntegerArrayWithCommaSplitProperty, parsedObj.TextualIntegerArrayWithCommaSplitProperty);
            CollectionAssert.AreEqual(rawObj.TextualLongArrayWithCommaSplitProperty, parsedObj.TextualLongArrayWithCommaSplitProperty);
            CollectionAssert.AreEqual(rawObj.TextualStringArrayWithCommaSplitProperty, parsedObj.TextualStringArrayWithCommaSplitProperty);
            CollectionAssert.AreEqual(rawObj.TextualDoubleListWithCommaSplitProperty, parsedObj.TextualDoubleListWithCommaSplitProperty);
            CollectionAssert.AreEqual(rawObj.TextualIntegerListWithCommaSplitProperty, parsedObj.TextualIntegerListWithCommaSplitProperty);
            CollectionAssert.AreEqual(rawObj.TextualLongListWithCommaSplitProperty, parsedObj.TextualLongListWithCommaSplitProperty);
            CollectionAssert.AreEqual(rawObj.TextualStringListWithCommaSplitProperty, parsedObj.TextualStringListWithCommaSplitProperty);
            Assert.NotNull(parsedObj.DynamicObjectConverter_Boolean);
            Assert.NotNull(parsedObj.DynamicObjectConverter_Number);
            Assert.NotNull(parsedObj.DynamicObjectConverter_String);
            Assert.NotNull(parsedObj.DynamicObjectConverter_Guid);
            Assert.NotNull(parsedObj.DynamicObjectConverter_Array);
            Assert.NotNull(parsedObj.DynamicObjectConverter_List);
            Assert.NotNull(parsedObj.DynamicObjectConverter_Dictionary);
            Assert.NotNull(parsedObj.DynamicObjectConverter_AnonymousObject);
        }
    }
}
