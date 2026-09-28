namespace SKIT.FlurlHttpClient.UnitTests.TestCases
{
    using SKIT.FlurlHttpClient.Internal;

    public class TestCase_UtilityInternalXmlSimpleSerializerTest
    {
        public sealed class MockClass
        {
            [System.Xml.Serialization.XmlElement(nameof(PropertyAsString), Order = 1, IsNullable = true)]
            public string? PropertyAsString { get; set; }

            [System.Xml.Serialization.XmlElement(nameof(PropertyAsInt32), Order = 2)]
            public int PropertyAsInt32 { get; set; }

            [System.Xml.Serialization.XmlElement(nameof(PropertyAsObject), Order = 3, IsNullable = true)]
            public MockClass? PropertyAsObject { get; set; }
        }

        [Fact(DisplayName = "测试用例：工具类 `_XmlSimpleSerializer.Serialize`")]
        public void TestUtilityInternalSimpleXmlSerializer_Serialize()
        {
            var mockObj = new MockClass()
            {
                PropertyAsString = "hello world",
                PropertyAsInt32 = 2147483647,
                PropertyAsObject = new MockClass()
                {
                    PropertyAsInt32 = -2147483648
                }
            };

            var actualXml = _XmlSimpleSerializer.Serialize(mockObj, typeof(MockClass));

            Assert.Equal("<xml><PropertyAsString>hello world</PropertyAsString><PropertyAsInt32>2147483647</PropertyAsInt32><PropertyAsObject><PropertyAsInt32>-2147483648</PropertyAsInt32></PropertyAsObject></xml>", actualXml);
        }

        [Fact(DisplayName = "测试用例：工具类 `_XmlSimpleSerializer.Deserialize`")]
        public void TestUtilityInternalSimpleXmlSerializer_Deserialize()
        {
            var mockXml = "<xml><PropertyAsString>hello world</PropertyAsString><PropertyAsInt32>2147483647</PropertyAsInt32><PropertyAsObject><PropertyAsInt32>-2147483648</PropertyAsInt32></PropertyAsObject></xml>";

            var actualObj = (MockClass)_XmlSimpleSerializer.Deserialize(mockXml, typeof(MockClass));

            Assert.Equal("hello world", actualObj.PropertyAsString);
            Assert.Equal(2147483647, actualObj.PropertyAsInt32);
            Assert.Equal(null!, actualObj.PropertyAsObject?.PropertyAsString);
            Assert.Equal(-2147483648, actualObj.PropertyAsObject?.PropertyAsInt32);
        }
    }
}
