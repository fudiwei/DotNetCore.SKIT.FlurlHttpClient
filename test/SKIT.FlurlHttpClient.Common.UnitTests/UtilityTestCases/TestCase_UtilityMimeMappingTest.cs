using NUnit.Framework;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases
{
    public class TestCase_UtilityMimeMappingTest
    {
        [Test(Description = "测试用例：MIME 映射")]
        public void TestMimeTypesMapping()
        {
            Assert.That(MimeTypes.GetMimeMapping(default!), Is.EqualTo(MimeTypes.Binary));
            Assert.That(MimeTypes.GetMimeMapping(""), Is.EqualTo(MimeTypes.Binary));
            Assert.That(MimeTypes.GetMimeMapping("."), Is.EqualTo(MimeTypes.Binary));
            Assert.That(MimeTypes.GetMimeMapping("image"), Is.EqualTo("application/octet-stream"));
            Assert.That(MimeTypes.GetMimeMapping("/path/to/"), Is.EqualTo("application/octet-stream"));
            Assert.That(MimeTypes.GetMimeMapping("/path/to/image"), Is.EqualTo("application/octet-stream"));
            Assert.That(MimeTypes.GetMimeMapping("image.png"), Is.EqualTo("image/png"));
            Assert.That(MimeTypes.GetMimeMapping("IMAGE.PNG"), Is.EqualTo("image/png"));
            Assert.That(MimeTypes.GetMimeMapping("/path/to/image.png"), Is.EqualTo("image/png"));
            Assert.That(MimeTypes.GetMimeMapping("/PATH/TO/IMAGE.PNG"), Is.EqualTo("image/png"));
            Assert.That(MimeTypes.GetMimeMapping("/PATH/TO/IMAGE.PNG"), Is.EqualTo("image/png"));
        }
    }
}
