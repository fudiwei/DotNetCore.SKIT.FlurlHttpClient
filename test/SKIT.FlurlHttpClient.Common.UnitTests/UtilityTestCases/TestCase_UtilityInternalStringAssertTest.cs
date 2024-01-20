using System.Text;
using NUnit.Framework;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases
{
    using SKIT.FlurlHttpClient.Internal;

    public class TestCase_UtilityInternalStringAssertTest
    {
        [Test(Description = "测试用例：工具类之 `MaybeJson`")]
        public void TestUtilityInternalStringAssert_MaybeJson()
        {
            Assert.That(_StringAssert.MaybeJson("{}"), Is.True);
            Assert.That(_StringAssert.MaybeJson("[]"), Is.True);
            Assert.That(_StringAssert.MaybeJson(" { } "), Is.True);
            Assert.That(_StringAssert.MaybeJson(" [ ] "), Is.True);
            Assert.That(_StringAssert.MaybeJson("\r\n{\t}\r\n"), Is.True);
            Assert.That(_StringAssert.MaybeJson("\r\n[\t]\r\n"), Is.True);

            Assert.That(_StringAssert.MaybeJson(Encoding.UTF8.GetBytes("{}")), Is.True);
            Assert.That(_StringAssert.MaybeJson(Encoding.UTF8.GetBytes("[]")), Is.True);
            Assert.That(_StringAssert.MaybeJson(Encoding.UTF8.GetBytes(" { } ")), Is.True);
            Assert.That(_StringAssert.MaybeJson(Encoding.UTF8.GetBytes(" [ ] ")), Is.True);
            Assert.That(_StringAssert.MaybeJson(Encoding.UTF8.GetBytes("\r\n{\t}\r\n")), Is.True);
            Assert.That(_StringAssert.MaybeJson(Encoding.UTF8.GetBytes("\r\n[\t]\r\n")), Is.True);

            Assert.That(_StringAssert.MaybeJson(default(string)!), Is.False);
            Assert.That(_StringAssert.MaybeJson(""), Is.False);
            Assert.That(_StringAssert.MaybeJson(" "), Is.False);
            Assert.That(_StringAssert.MaybeJson("{"), Is.False);
            Assert.That(_StringAssert.MaybeJson("}"), Is.False);
            Assert.That(_StringAssert.MaybeJson("["), Is.False);
            Assert.That(_StringAssert.MaybeJson("]"), Is.False);
            Assert.That(_StringAssert.MaybeJson("{]"), Is.False);
            Assert.That(_StringAssert.MaybeJson("[}"), Is.False);

            Assert.That(_StringAssert.MaybeJson(default(byte[])!), Is.False);
            Assert.That(_StringAssert.MaybeJson(Encoding.UTF8.GetBytes("")), Is.False);
            Assert.That(_StringAssert.MaybeJson(Encoding.UTF8.GetBytes(" ")), Is.False);
            Assert.That(_StringAssert.MaybeJson(Encoding.UTF8.GetBytes("{")), Is.False);
            Assert.That(_StringAssert.MaybeJson(Encoding.UTF8.GetBytes("}")), Is.False);
            Assert.That(_StringAssert.MaybeJson(Encoding.UTF8.GetBytes("[")), Is.False);
            Assert.That(_StringAssert.MaybeJson(Encoding.UTF8.GetBytes("]")), Is.False);
            Assert.That(_StringAssert.MaybeJson(Encoding.UTF8.GetBytes("{]")), Is.False);
            Assert.That(_StringAssert.MaybeJson(Encoding.UTF8.GetBytes("[}")), Is.False);
        }

        [Test(Description = "测试用例：工具类之 `MaybeXml`")]
        public void TestUtilityInternalStringAssert_MaybeXml()
        {
            Assert.That(_StringAssert.MaybeXml("<xml></xml>"));
            Assert.That(_StringAssert.MaybeXml(" <xml> </xml> "));
            Assert.That(_StringAssert.MaybeXml("\r\n<xml>\t</xml>\r\n"));

            Assert.That(_StringAssert.MaybeXml(Encoding.UTF8.GetBytes("<xml></xml>")));
            Assert.That(_StringAssert.MaybeXml(Encoding.UTF8.GetBytes(" <xml> </xml> ")));
            Assert.That(_StringAssert.MaybeXml(Encoding.UTF8.GetBytes("\r\n<xml>\t</xml>\r\n")));

            Assert.That(_StringAssert.MaybeXml(default(string)!), Is.False);
            Assert.That(_StringAssert.MaybeXml(""), Is.False);
            Assert.That(_StringAssert.MaybeXml(" "), Is.False);
            Assert.That(_StringAssert.MaybeXml("<"), Is.False);
            Assert.That(_StringAssert.MaybeXml(">"), Is.False);
            Assert.That(_StringAssert.MaybeXml("<<"), Is.False);
            Assert.That(_StringAssert.MaybeXml(">>"), Is.False);
            Assert.That(_StringAssert.MaybeXml("<>"), Is.False);

            Assert.That(_StringAssert.MaybeXml(default(byte[])!), Is.False);
            Assert.That(_StringAssert.MaybeXml(Encoding.UTF8.GetBytes("")), Is.False);
            Assert.That(_StringAssert.MaybeXml(Encoding.UTF8.GetBytes(" ")), Is.False);
            Assert.That(_StringAssert.MaybeXml(Encoding.UTF8.GetBytes("<")), Is.False);
            Assert.That(_StringAssert.MaybeXml(Encoding.UTF8.GetBytes(">")), Is.False);
            Assert.That(_StringAssert.MaybeXml(Encoding.UTF8.GetBytes("<<")), Is.False);
            Assert.That(_StringAssert.MaybeXml(Encoding.UTF8.GetBytes(">>")), Is.False);
            Assert.That(_StringAssert.MaybeXml(Encoding.UTF8.GetBytes("<>")), Is.False);
        }
    }
}
