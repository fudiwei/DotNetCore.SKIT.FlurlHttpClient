using System.Text;
using NUnit.Framework;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases
{
    using SKIT.FlurlHttpClient.Internal;

    public class TestCase_UtilityInternalStringSyntaxAssertTest
    {
        [Test(Description = "测试用例：_StringSyntaxAssert 工具类之 `MaybeJson`")]
        public void TestUtilityInternalStringSyntaxAssert_MaybeJson()
        {
            Assert.That(_StringSyntaxAssert.MaybeJson("{}"), Is.True);
            Assert.That(_StringSyntaxAssert.MaybeJson("[]"), Is.True);
            Assert.That(_StringSyntaxAssert.MaybeJson(" { } "), Is.True);
            Assert.That(_StringSyntaxAssert.MaybeJson(" [ ] "), Is.True);
            Assert.That(_StringSyntaxAssert.MaybeJson("\r\n{\t}\r\n"), Is.True);
            Assert.That(_StringSyntaxAssert.MaybeJson("\r\n[\t]\r\n"), Is.True);

            Assert.That(_StringSyntaxAssert.MaybeJson(Encoding.UTF8.GetBytes("{}")), Is.True);
            Assert.That(_StringSyntaxAssert.MaybeJson(Encoding.UTF8.GetBytes("[]")), Is.True);
            Assert.That(_StringSyntaxAssert.MaybeJson(Encoding.UTF8.GetBytes(" { } ")), Is.True);
            Assert.That(_StringSyntaxAssert.MaybeJson(Encoding.UTF8.GetBytes(" [ ] ")), Is.True);
            Assert.That(_StringSyntaxAssert.MaybeJson(Encoding.UTF8.GetBytes("\r\n{\t}\r\n")), Is.True);
            Assert.That(_StringSyntaxAssert.MaybeJson(Encoding.UTF8.GetBytes("\r\n[\t]\r\n")), Is.True);

            Assert.That(_StringSyntaxAssert.MaybeJson(default(string)!), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeJson(""), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeJson(" "), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeJson("{"), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeJson("}"), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeJson("["), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeJson("]"), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeJson("{]"), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeJson("[}"), Is.False);

            Assert.That(_StringSyntaxAssert.MaybeJson(default(byte[])!), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeJson(Encoding.UTF8.GetBytes("")), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeJson(Encoding.UTF8.GetBytes(" ")), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeJson(Encoding.UTF8.GetBytes("{")), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeJson(Encoding.UTF8.GetBytes("}")), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeJson(Encoding.UTF8.GetBytes("[")), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeJson(Encoding.UTF8.GetBytes("]")), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeJson(Encoding.UTF8.GetBytes("{]")), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeJson(Encoding.UTF8.GetBytes("[}")), Is.False);
        }

        [Test(Description = "测试用例：_StringSyntaxAssert 工具类之 `MaybeXml`")]
        public void TestUtilityInternalStringAssert_MaybeXml()
        {
            Assert.That(_StringSyntaxAssert.MaybeXml("<xml></xml>"));
            Assert.That(_StringSyntaxAssert.MaybeXml(" <xml> </xml> "));
            Assert.That(_StringSyntaxAssert.MaybeXml("\r\n<xml>\t</xml>\r\n"));

            Assert.That(_StringSyntaxAssert.MaybeXml(Encoding.UTF8.GetBytes("<xml></xml>")));
            Assert.That(_StringSyntaxAssert.MaybeXml(Encoding.UTF8.GetBytes(" <xml> </xml> ")));
            Assert.That(_StringSyntaxAssert.MaybeXml(Encoding.UTF8.GetBytes("\r\n<xml>\t</xml>\r\n")));

            Assert.That(_StringSyntaxAssert.MaybeXml(default(string)!), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeXml(""), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeXml(" "), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeXml("<"), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeXml(">"), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeXml("<<"), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeXml(">>"), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeXml("<>"), Is.False);

            Assert.That(_StringSyntaxAssert.MaybeXml(default(byte[])!), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeXml(Encoding.UTF8.GetBytes("")), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeXml(Encoding.UTF8.GetBytes(" ")), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeXml(Encoding.UTF8.GetBytes("<")), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeXml(Encoding.UTF8.GetBytes(">")), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeXml(Encoding.UTF8.GetBytes("<<")), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeXml(Encoding.UTF8.GetBytes(">>")), Is.False);
            Assert.That(_StringSyntaxAssert.MaybeXml(Encoding.UTF8.GetBytes("<>")), Is.False);
        }
    }
}
