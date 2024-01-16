using System.Text;
using NUnit.Framework;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases.Utility
{
    using SKIT.FlurlHttpClient.Utilities.Internal;

    public class TestCase_FormatUtilityTest
    {
        [Test(Description = "测试用例：工具类之 `MaybeJson`")]
        public void TestFormatUtility_MaybeJson()
        {
            Assert.True(FormatUtility.MaybeJson("{}"));
            Assert.True(FormatUtility.MaybeJson("[]"));
            Assert.True(FormatUtility.MaybeJson(" { } "));
            Assert.True(FormatUtility.MaybeJson(" [ ] "));
            Assert.True(FormatUtility.MaybeJson("\r\n{\t}\r\n"));
            Assert.True(FormatUtility.MaybeJson("\r\n[\t]\r\n"));

            Assert.True(FormatUtility.MaybeJson(Encoding.UTF8.GetBytes("{}")));
            Assert.True(FormatUtility.MaybeJson(Encoding.UTF8.GetBytes("[]")));
            Assert.True(FormatUtility.MaybeJson(Encoding.UTF8.GetBytes(" { } ")));
            Assert.True(FormatUtility.MaybeJson(Encoding.UTF8.GetBytes(" [ ] ")));
            Assert.True(FormatUtility.MaybeJson(Encoding.UTF8.GetBytes("\r\n{\t}\r\n")));
            Assert.True(FormatUtility.MaybeJson(Encoding.UTF8.GetBytes("\r\n[\t]\r\n")));

            Assert.False(FormatUtility.MaybeJson(default(string)!));
            Assert.False(FormatUtility.MaybeJson(""));
            Assert.False(FormatUtility.MaybeJson(" "));
            Assert.False(FormatUtility.MaybeJson("{"));
            Assert.False(FormatUtility.MaybeJson("}"));
            Assert.False(FormatUtility.MaybeJson("["));
            Assert.False(FormatUtility.MaybeJson("]"));
            Assert.False(FormatUtility.MaybeJson("{]"));
            Assert.False(FormatUtility.MaybeJson("[}"));

            Assert.False(FormatUtility.MaybeJson(default(byte[])!));
            Assert.False(FormatUtility.MaybeJson(Encoding.UTF8.GetBytes("")));
            Assert.False(FormatUtility.MaybeJson(Encoding.UTF8.GetBytes(" ")));
            Assert.False(FormatUtility.MaybeJson(Encoding.UTF8.GetBytes("{")));
            Assert.False(FormatUtility.MaybeJson(Encoding.UTF8.GetBytes("}")));
            Assert.False(FormatUtility.MaybeJson(Encoding.UTF8.GetBytes("[")));
            Assert.False(FormatUtility.MaybeJson(Encoding.UTF8.GetBytes("]")));
            Assert.False(FormatUtility.MaybeJson(Encoding.UTF8.GetBytes("{]")));
            Assert.False(FormatUtility.MaybeJson(Encoding.UTF8.GetBytes("[}")));
        }

        [Test(Description = "测试用例：工具类之 `MaybeXml`")]
        public void TestFormatUtility_MaybeXml()
        {
            Assert.True(FormatUtility.MaybeXml("<xml></xml>"));
            Assert.True(FormatUtility.MaybeXml(" <xml> </xml> "));
            Assert.True(FormatUtility.MaybeXml("\r\n<xml>\t</xml>\r\n"));

            Assert.True(FormatUtility.MaybeXml(Encoding.UTF8.GetBytes("<xml></xml>")));
            Assert.True(FormatUtility.MaybeXml(Encoding.UTF8.GetBytes(" <xml> </xml> ")));
            Assert.True(FormatUtility.MaybeXml(Encoding.UTF8.GetBytes("\r\n<xml>\t</xml>\r\n")));

            Assert.False(FormatUtility.MaybeXml(default(string)!));
            Assert.False(FormatUtility.MaybeXml(""));
            Assert.False(FormatUtility.MaybeXml(" "));
            Assert.False(FormatUtility.MaybeXml("<"));
            Assert.False(FormatUtility.MaybeXml(">"));
            Assert.False(FormatUtility.MaybeXml("<<"));
            Assert.False(FormatUtility.MaybeXml(">>"));
            Assert.False(FormatUtility.MaybeXml("<>"));

            Assert.False(FormatUtility.MaybeXml(default(byte[])!));
            Assert.False(FormatUtility.MaybeXml(Encoding.UTF8.GetBytes("")));
            Assert.False(FormatUtility.MaybeXml(Encoding.UTF8.GetBytes(" ")));
            Assert.False(FormatUtility.MaybeXml(Encoding.UTF8.GetBytes("<")));
            Assert.False(FormatUtility.MaybeXml(Encoding.UTF8.GetBytes(">")));
            Assert.False(FormatUtility.MaybeXml(Encoding.UTF8.GetBytes("<<")));
            Assert.False(FormatUtility.MaybeXml(Encoding.UTF8.GetBytes(">>")));
            Assert.False(FormatUtility.MaybeXml(Encoding.UTF8.GetBytes("<>")));
        }
    }
}
