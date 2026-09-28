namespace SKIT.FlurlHttpClient.UnitTests.TestCases
{
    using SKIT.FlurlHttpClient.Internal;

    public class TestCase_UtilityInternalXmlSimpleSerializerTest
    {
        public sealed class MockObject
        {
            [System.Xml.Serialization.XmlElement(nameof(PropertyAsString), Order = 1, IsNullable = true)]
            public string? PropertyAsString { get; set; }

            [System.Xml.Serialization.XmlElement(nameof(PropertyAsInt32), Order = 2)]
            public int PropertyAsInt32 { get; set; }

            [System.Xml.Serialization.XmlElement(nameof(PropertyAsObject), Order = 3, IsNullable = true)]
            public MockObject? PropertyAsObject { get; set; }
        }

        [Fact(DisplayName = "测试用例：_XmlSimpleSerializer 工具类")]
        public void TestUtilityInternalSimpleXmlSerializer()
        {
            var expectObj = new MockObject()
            {
                PropertyAsString = "hello world",
                PropertyAsInt32 = 2147483647,
                PropertyAsObject = new MockObject()
                {
                    PropertyAsInt32 = -2147483648
                }
            };
            var actualXml = _XmlSimpleSerializer.Serialize(expectObj, typeof(MockObject));
            var actualObj = (MockObject)_XmlSimpleSerializer.Deserialize(actualXml, typeof(MockObject));

            Assert.Equal("<xml><PropertyAsString>hello world</PropertyAsString><PropertyAsInt32>2147483647</PropertyAsInt32><PropertyAsObject><PropertyAsInt32>-2147483648</PropertyAsInt32></PropertyAsObject></xml>", actualXml);

            Assert.Equal(expectObj.PropertyAsString, actualObj.PropertyAsString);
            Assert.Equal(expectObj.PropertyAsInt32, actualObj.PropertyAsInt32);
            Assert.Equal(expectObj.PropertyAsObject.PropertyAsString, actualObj.PropertyAsObject?.PropertyAsString);
            Assert.Equal(expectObj.PropertyAsObject.PropertyAsInt32, actualObj.PropertyAsObject?.PropertyAsInt32);
        }
    }
}
