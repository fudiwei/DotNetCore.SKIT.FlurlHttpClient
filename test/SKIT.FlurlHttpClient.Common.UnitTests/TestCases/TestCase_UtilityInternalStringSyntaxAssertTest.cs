namespace SKIT.FlurlHttpClient.UnitTests.TestCases
{
    using SKIT.FlurlHttpClient.Internal;

    public class TestCase_UtilityInternalStringSyntaxAssertTest
    {
        [Fact(DisplayName = "测试用例：工具类 `_StringSyntaxAssert.MaybeJson`")]
        public void TestUtilityInternalStringSyntaxAssert_MaybeJson()
        {
            Assert.True(_StringSyntaxAssert.MaybeJson("{}"));
            Assert.True(_StringSyntaxAssert.MaybeJson("[]"));
            Assert.True(_StringSyntaxAssert.MaybeJson(" { } "));
            Assert.True(_StringSyntaxAssert.MaybeJson(" [ ] "));
            Assert.True(_StringSyntaxAssert.MaybeJson("\r\n{\t}\r\n"));
            Assert.True(_StringSyntaxAssert.MaybeJson("\r\n[\t]\r\n"));

            Assert.True(_StringSyntaxAssert.MaybeJson(Encoding.UTF8.GetBytes("{}")));
            Assert.True(_StringSyntaxAssert.MaybeJson(Encoding.UTF8.GetBytes("[]")));
            Assert.True(_StringSyntaxAssert.MaybeJson(Encoding.UTF8.GetBytes(" { } ")));
            Assert.True(_StringSyntaxAssert.MaybeJson(Encoding.UTF8.GetBytes(" [ ] ")));
            Assert.True(_StringSyntaxAssert.MaybeJson(Encoding.UTF8.GetBytes("\r\n{\t}\r\n")));
            Assert.True(_StringSyntaxAssert.MaybeJson(Encoding.UTF8.GetBytes("\r\n[\t]\r\n")));

            Assert.False(_StringSyntaxAssert.MaybeJson(default(string)!));
            Assert.False(_StringSyntaxAssert.MaybeJson(""));
            Assert.False(_StringSyntaxAssert.MaybeJson(" "));
            Assert.False(_StringSyntaxAssert.MaybeJson("{"));
            Assert.False(_StringSyntaxAssert.MaybeJson("}"));
            Assert.False(_StringSyntaxAssert.MaybeJson("["));
            Assert.False(_StringSyntaxAssert.MaybeJson("]"));
            Assert.False(_StringSyntaxAssert.MaybeJson("{]"));
            Assert.False(_StringSyntaxAssert.MaybeJson("[}"));

            Assert.False(_StringSyntaxAssert.MaybeJson(default(byte[])!));
            Assert.False(_StringSyntaxAssert.MaybeJson(Encoding.UTF8.GetBytes("")));
            Assert.False(_StringSyntaxAssert.MaybeJson(Encoding.UTF8.GetBytes(" ")));
            Assert.False(_StringSyntaxAssert.MaybeJson(Encoding.UTF8.GetBytes("{")));
            Assert.False(_StringSyntaxAssert.MaybeJson(Encoding.UTF8.GetBytes("}")));
            Assert.False(_StringSyntaxAssert.MaybeJson(Encoding.UTF8.GetBytes("[")));
            Assert.False(_StringSyntaxAssert.MaybeJson(Encoding.UTF8.GetBytes("]")));
            Assert.False(_StringSyntaxAssert.MaybeJson(Encoding.UTF8.GetBytes("{]")));
            Assert.False(_StringSyntaxAssert.MaybeJson(Encoding.UTF8.GetBytes("[}")));
        }

        [Fact(DisplayName = "测试用例：工具类 `_StringSyntaxAssert.MaybeXml`")]
        public void TestUtilityInternalStringAssert_MaybeXml()
        {
            Assert.True(_StringSyntaxAssert.MaybeXml("<xml></xml>"));
            Assert.True(_StringSyntaxAssert.MaybeXml(" <xml> </xml> "));
            Assert.True(_StringSyntaxAssert.MaybeXml("\r\n<xml>\t</xml>\r\n"));

            Assert.True(_StringSyntaxAssert.MaybeXml(Encoding.UTF8.GetBytes("<xml></xml>")));
            Assert.True(_StringSyntaxAssert.MaybeXml(Encoding.UTF8.GetBytes(" <xml> </xml> ")));
            Assert.True(_StringSyntaxAssert.MaybeXml(Encoding.UTF8.GetBytes("\r\n<xml>\t</xml>\r\n")));

            Assert.False(_StringSyntaxAssert.MaybeXml(default(string)!));
            Assert.False(_StringSyntaxAssert.MaybeXml(""));
            Assert.False(_StringSyntaxAssert.MaybeXml(" "));
            Assert.False(_StringSyntaxAssert.MaybeXml("<"));
            Assert.False(_StringSyntaxAssert.MaybeXml(">"));
            Assert.False(_StringSyntaxAssert.MaybeXml("<<"));
            Assert.False(_StringSyntaxAssert.MaybeXml(">>"));
            Assert.False(_StringSyntaxAssert.MaybeXml("<>"));

            Assert.False(_StringSyntaxAssert.MaybeXml(default(byte[])!));
            Assert.False(_StringSyntaxAssert.MaybeXml(Encoding.UTF8.GetBytes("")));
            Assert.False(_StringSyntaxAssert.MaybeXml(Encoding.UTF8.GetBytes(" ")));
            Assert.False(_StringSyntaxAssert.MaybeXml(Encoding.UTF8.GetBytes("<")));
            Assert.False(_StringSyntaxAssert.MaybeXml(Encoding.UTF8.GetBytes(">")));
            Assert.False(_StringSyntaxAssert.MaybeXml(Encoding.UTF8.GetBytes("<<")));
            Assert.False(_StringSyntaxAssert.MaybeXml(Encoding.UTF8.GetBytes(">>")));
            Assert.False(_StringSyntaxAssert.MaybeXml(Encoding.UTF8.GetBytes("<>")));
        }
    }
}
