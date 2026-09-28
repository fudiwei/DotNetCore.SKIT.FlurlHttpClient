namespace SKIT.FlurlHttpClient.UnitTests.TestCases
{
    public class TestCase_UtilityMimeMappingTest
    {
        [Fact(DisplayName = "测试用例：MIME 映射")]
        public void TestMimeTypesMapping()
        {
            Assert.Equal(MimeTypes.Binary, MimeTypes.GetMimeMapping(default!));
            Assert.Equal(MimeTypes.Binary, MimeTypes.GetMimeMapping(""));
            Assert.Equal(MimeTypes.Binary, MimeTypes.GetMimeMapping("."));

            Assert.Equal("application/octet-stream", MimeTypes.GetMimeMapping("image"));
            Assert.Equal("application/octet-stream", MimeTypes.GetMimeMapping("/path/to/"));
            Assert.Equal("application/octet-stream", MimeTypes.GetMimeMapping("/path/to/png"));

            Assert.Equal("image/png", MimeTypes.GetMimeMapping("image.png"));
            Assert.Equal("image/png", MimeTypes.GetMimeMapping("IMAGE.PNG"));
            Assert.Equal("image/png", MimeTypes.GetMimeMapping("/path/to/image.png"));
            Assert.Equal("image/png", MimeTypes.GetMimeMapping("/PATH/TO/IMAGE.PNG"));
        }
    }
}
