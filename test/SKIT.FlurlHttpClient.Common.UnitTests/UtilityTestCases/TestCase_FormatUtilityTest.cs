using System.Text;
using NUnit.Framework;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases
{
    using SKIT.FlurlHttpClient.Utilities.Internal;

    public class TestCase_FormatUtilityTest
    {
        [Test(Description = "测试用例：工具类之 `MaybeJson`")]
        public void TestFormatUtility_MaybeJson()
        {
            Assert.That(FormatUtility.MaybeJson("{}"), Is.True);
            Assert.That(FormatUtility.MaybeJson("[]"), Is.True);
            Assert.That(FormatUtility.MaybeJson(" { } "), Is.True);
            Assert.That(FormatUtility.MaybeJson(" [ ] "), Is.True);
            Assert.That(FormatUtility.MaybeJson("\r\n{\t}\r\n"), Is.True);
            Assert.That(FormatUtility.MaybeJson("\r\n[\t]\r\n"), Is.True);

            Assert.That(FormatUtility.MaybeJson(Encoding.UTF8.GetBytes("{}")), Is.True);
            Assert.That(FormatUtility.MaybeJson(Encoding.UTF8.GetBytes("[]")), Is.True);
            Assert.That(FormatUtility.MaybeJson(Encoding.UTF8.GetBytes(" { } ")), Is.True);
            Assert.That(FormatUtility.MaybeJson(Encoding.UTF8.GetBytes(" [ ] ")), Is.True);
            Assert.That(FormatUtility.MaybeJson(Encoding.UTF8.GetBytes("\r\n{\t}\r\n")), Is.True);
            Assert.That(FormatUtility.MaybeJson(Encoding.UTF8.GetBytes("\r\n[\t]\r\n")), Is.True);

            Assert.That(FormatUtility.MaybeJson(default(string)!), Is.False);
            Assert.That(FormatUtility.MaybeJson(""), Is.False);
            Assert.That(FormatUtility.MaybeJson(" "), Is.False);
            Assert.That(FormatUtility.MaybeJson("{"), Is.False);
            Assert.That(FormatUtility.MaybeJson("}"), Is.False);
            Assert.That(FormatUtility.MaybeJson("["), Is.False);
            Assert.That(FormatUtility.MaybeJson("]"), Is.False);
            Assert.That(FormatUtility.MaybeJson("{]"), Is.False);
            Assert.That(FormatUtility.MaybeJson("[}"), Is.False);

            Assert.That(FormatUtility.MaybeJson(default(byte[])!), Is.False);
            Assert.That(FormatUtility.MaybeJson(Encoding.UTF8.GetBytes("")), Is.False);
            Assert.That(FormatUtility.MaybeJson(Encoding.UTF8.GetBytes(" ")), Is.False);
            Assert.That(FormatUtility.MaybeJson(Encoding.UTF8.GetBytes("{")), Is.False);
            Assert.That(FormatUtility.MaybeJson(Encoding.UTF8.GetBytes("}")), Is.False);
            Assert.That(FormatUtility.MaybeJson(Encoding.UTF8.GetBytes("[")), Is.False);
            Assert.That(FormatUtility.MaybeJson(Encoding.UTF8.GetBytes("]")), Is.False);
            Assert.That(FormatUtility.MaybeJson(Encoding.UTF8.GetBytes("{]")), Is.False);
            Assert.That(FormatUtility.MaybeJson(Encoding.UTF8.GetBytes("[}")), Is.False);
        }

        [Test(Description = "测试用例：工具类之 `MaybeXml`")]
        public void TestFormatUtility_MaybeXml()
        {
            Assert.That(FormatUtility.MaybeXml("<xml></xml>"));
            Assert.That(FormatUtility.MaybeXml(" <xml> </xml> "));
            Assert.That(FormatUtility.MaybeXml("\r\n<xml>\t</xml>\r\n"));

            Assert.That(FormatUtility.MaybeXml(Encoding.UTF8.GetBytes("<xml></xml>")));
            Assert.That(FormatUtility.MaybeXml(Encoding.UTF8.GetBytes(" <xml> </xml> ")));
            Assert.That(FormatUtility.MaybeXml(Encoding.UTF8.GetBytes("\r\n<xml>\t</xml>\r\n")));

            Assert.That(FormatUtility.MaybeXml(default(string)!), Is.False);
            Assert.That(FormatUtility.MaybeXml(""), Is.False);
            Assert.That(FormatUtility.MaybeXml(" "), Is.False);
            Assert.That(FormatUtility.MaybeXml("<"), Is.False);
            Assert.That(FormatUtility.MaybeXml(">"), Is.False);
            Assert.That(FormatUtility.MaybeXml("<<"), Is.False);
            Assert.That(FormatUtility.MaybeXml(">>"), Is.False);
            Assert.That(FormatUtility.MaybeXml("<>"), Is.False);

            Assert.That(FormatUtility.MaybeXml(default(byte[])!), Is.False);
            Assert.That(FormatUtility.MaybeXml(Encoding.UTF8.GetBytes("")), Is.False);
            Assert.That(FormatUtility.MaybeXml(Encoding.UTF8.GetBytes(" ")), Is.False);
            Assert.That(FormatUtility.MaybeXml(Encoding.UTF8.GetBytes("<")), Is.False);
            Assert.That(FormatUtility.MaybeXml(Encoding.UTF8.GetBytes(">")), Is.False);
            Assert.That(FormatUtility.MaybeXml(Encoding.UTF8.GetBytes("<<")), Is.False);
            Assert.That(FormatUtility.MaybeXml(Encoding.UTF8.GetBytes(">>")), Is.False);
            Assert.That(FormatUtility.MaybeXml(Encoding.UTF8.GetBytes("<>")), Is.False);
        }
    }
}
