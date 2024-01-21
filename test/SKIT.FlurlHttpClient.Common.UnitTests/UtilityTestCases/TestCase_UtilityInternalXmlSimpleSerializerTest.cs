using NUnit.Framework;

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

        [Test(Description = "测试用例：_XmlSimpleSerializer 工具类")]
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

            Assert.That(actualXml, Is.EqualTo("<xml><PropertyAsString>hello world</PropertyAsString><PropertyAsInt32>2147483647</PropertyAsInt32><PropertyAsObject><PropertyAsInt32>-2147483648</PropertyAsInt32></PropertyAsObject></xml>"));

            Assert.That(actualObj.PropertyAsString, Is.EqualTo(expectObj.PropertyAsString));
            Assert.That(actualObj.PropertyAsInt32, Is.EqualTo(expectObj.PropertyAsInt32));
            Assert.That(actualObj.PropertyAsObject?.PropertyAsString, Is.EqualTo(expectObj.PropertyAsObject.PropertyAsString));
            Assert.That(actualObj.PropertyAsObject?.PropertyAsInt32, Is.EqualTo(expectObj.PropertyAsObject.PropertyAsInt32));
        }
    }
}
