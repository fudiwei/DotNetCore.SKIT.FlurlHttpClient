using System.Text;
using NUnit.Framework;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases
{
    public class TestCase_SecurityEncodedStringTest
    {
        [Test(Description = "测试用例：EncodedString")]
        public void TestSecurityEncodedStringTest()
        {
            Assert.Multiple(() =>
            {
                const string TEXT = "SKIT.FlurlHttpClient is AWESOME!";

                Assert.That((string?)new EncodedString(TEXT), Is.EqualTo(TEXT));

                Assert.That((EncodedString)TEXT, Is.EqualTo(new EncodedString(TEXT)));
            });

            Assert.Multiple(() =>
            {
                Assert.That(new EncodedString(null), Is.EqualTo(new EncodedString(null)));
                Assert.That(new EncodedString(""), Is.EqualTo(new EncodedString("")));
                Assert.That(new EncodedString("Unspecified", EncodingKinds.Unspecified), Is.EqualTo(new EncodedString("Unspecified", EncodingKinds.Unspecified)));
                Assert.That(new EncodedString("Hex", EncodingKinds.Hex), Is.EqualTo(new EncodedString("Hex", EncodingKinds.Hex)));
                Assert.That(new EncodedString("Base64", EncodingKinds.Base64), Is.EqualTo(new EncodedString("Base64", EncodingKinds.Base64)));

                Assert.That(new EncodedString(""), Is.Not.EqualTo(new EncodedString(null)));
                Assert.That(new EncodedString("", EncodingKinds.Hex), Is.Not.EqualTo(new EncodedString("", EncodingKinds.Base64)));
                Assert.That(new EncodedString("Unspecified", EncodingKinds.Unspecified), Is.Not.EqualTo(new EncodedString("", EncodingKinds.Unspecified)));
                Assert.That(new EncodedString("Unspecified", EncodingKinds.Unspecified), Is.Not.EqualTo(new EncodedString("Hex", EncodingKinds.Hex)));
                Assert.That(new EncodedString("Unspecified", EncodingKinds.Unspecified), Is.Not.EqualTo(new EncodedString("Base64", EncodingKinds.Base64)));
                Assert.That(new EncodedString("Hex", EncodingKinds.Hex), Is.Not.EqualTo(new EncodedString("Unspecified", EncodingKinds.Unspecified)));
                Assert.That(new EncodedString("Hex", EncodingKinds.Hex), Is.Not.EqualTo(new EncodedString("", EncodingKinds.Hex)));
                Assert.That(new EncodedString("Hex", EncodingKinds.Hex), Is.Not.EqualTo(new EncodedString("Base64", EncodingKinds.Base64)));
                Assert.That(new EncodedString("Base64", EncodingKinds.Base64), Is.Not.EqualTo(new EncodedString("Unspecified", EncodingKinds.Unspecified)));
                Assert.That(new EncodedString("Base64", EncodingKinds.Base64), Is.Not.EqualTo(new EncodedString("Hex", EncodingKinds.Hex)));
                Assert.That(new EncodedString("Base64", EncodingKinds.Base64), Is.Not.EqualTo(new EncodedString("", EncodingKinds.Base64)));

                Assert.That(new EncodedString(null) == new EncodedString(null), Is.True);
                Assert.That(new EncodedString("") == new EncodedString(""), Is.True);
                Assert.That(new EncodedString("Unspecified", EncodingKinds.Unspecified) == new EncodedString("Unspecified", EncodingKinds.Unspecified), Is.True);
                Assert.That(new EncodedString("Hex", EncodingKinds.Hex) == new EncodedString("Hex", EncodingKinds.Hex), Is.True);
                Assert.That(new EncodedString("Base64", EncodingKinds.Base64) == new EncodedString("Base64", EncodingKinds.Base64), Is.True);

                Assert.That(new EncodedString("") != new EncodedString(null), Is.True);
                Assert.That(new EncodedString("", EncodingKinds.Hex) != new EncodedString("", EncodingKinds.Base64), Is.True);
                Assert.That(new EncodedString("Unspecified", EncodingKinds.Unspecified) != new EncodedString("", EncodingKinds.Unspecified), Is.True);
                Assert.That(new EncodedString("Unspecified", EncodingKinds.Unspecified) != new EncodedString("Hex", EncodingKinds.Hex), Is.True);
                Assert.That(new EncodedString("Unspecified", EncodingKinds.Unspecified) != new EncodedString("Base64", EncodingKinds.Base64), Is.True);
                Assert.That(new EncodedString("Hex", EncodingKinds.Hex) != new EncodedString("Unspecified", EncodingKinds.Unspecified), Is.True);
                Assert.That(new EncodedString("Hex", EncodingKinds.Hex) != new EncodedString("", EncodingKinds.Hex), Is.True);
                Assert.That(new EncodedString("Hex", EncodingKinds.Hex) != new EncodedString("Base64", EncodingKinds.Base64), Is.True);
                Assert.That(new EncodedString("Base64", EncodingKinds.Base64) != new EncodedString("Unspecified", EncodingKinds.Unspecified), Is.True);
                Assert.That(new EncodedString("Base64", EncodingKinds.Base64) != new EncodedString("Hex", EncodingKinds.Hex), Is.True);
                Assert.That(new EncodedString("Base64", EncodingKinds.Base64) != new EncodedString("", EncodingKinds.Base64), Is.True);
            });

            Assert.Multiple(() =>
            {
                const string RAW_TEXT = "SKIT.FlurlHttpClient is AWESOME!";
                const string ENC_TEXT = "534B49542E466C75726C48747470436C69656E7420697320415745534F4D4521";

                Assert.That(EncodedString.ToEncodedString(Encoding.UTF8.GetBytes(RAW_TEXT), EncodingKinds.Hex).EncodingKind, Is.EqualTo(EncodingKinds.Hex));
                Assert.That(EncodedString.ToEncodedString(Encoding.UTF8.GetBytes(RAW_TEXT), EncodingKinds.Hex).Value, Is.EqualTo(ENC_TEXT));
                Assert.That(EncodedString.ToHexString(Encoding.UTF8.GetBytes(RAW_TEXT)).EncodingKind, Is.EqualTo(EncodingKinds.Hex));
                Assert.That(EncodedString.ToHexString(Encoding.UTF8.GetBytes(RAW_TEXT)).Value, Is.EqualTo(ENC_TEXT));

                Assert.That(Encoding.UTF8.GetString(EncodedString.FromEncodedString(ENC_TEXT, EncodingKinds.Hex)), Is.EqualTo(RAW_TEXT).IgnoreCase);
                Assert.That(Encoding.UTF8.GetString(EncodedString.FromHexString(new EncodedString(ENC_TEXT))), Is.EqualTo(RAW_TEXT).IgnoreCase);
                Assert.That(Encoding.UTF8.GetString(EncodedString.FromHexString(new EncodedString(ENC_TEXT, EncodingKinds.Unspecified))), Is.EqualTo(RAW_TEXT).IgnoreCase);
                Assert.That(Encoding.UTF8.GetString(EncodedString.FromHexString(new EncodedString(ENC_TEXT, EncodingKinds.Hex))), Is.EqualTo(RAW_TEXT).IgnoreCase);
                Assert.Catch(() => Encoding.UTF8.GetString(EncodedString.FromHexString(new EncodedString(ENC_TEXT, EncodingKinds.Base64))));
            });

            Assert.Multiple(() =>
            {
                const string RAW_TEXT = "SKIT.FlurlHttpClient is AWESOME!";
                const string ENC_TEXT = "U0tJVC5GbHVybEh0dHBDbGllbnQgaXMgQVdFU09NRSE=";

                Assert.That(EncodedString.ToEncodedString(Encoding.UTF8.GetBytes(RAW_TEXT), EncodingKinds.Base64).EncodingKind, Is.EqualTo(EncodingKinds.Base64));
                Assert.That(EncodedString.ToEncodedString(Encoding.UTF8.GetBytes(RAW_TEXT), EncodingKinds.Base64).Value, Is.EqualTo(ENC_TEXT));
                Assert.That(EncodedString.ToBase64String(Encoding.UTF8.GetBytes(RAW_TEXT)).EncodingKind, Is.EqualTo(EncodingKinds.Base64));
                Assert.That(EncodedString.ToBase64String(Encoding.UTF8.GetBytes(RAW_TEXT)).Value, Is.EqualTo(ENC_TEXT));

                Assert.That(Encoding.UTF8.GetString(EncodedString.FromEncodedString(ENC_TEXT, EncodingKinds.Base64)), Is.EqualTo(RAW_TEXT));
                Assert.That(Encoding.UTF8.GetString(EncodedString.FromBase64String(new EncodedString(ENC_TEXT))), Is.EqualTo(RAW_TEXT));
                Assert.That(Encoding.UTF8.GetString(EncodedString.FromBase64String(new EncodedString(ENC_TEXT, EncodingKinds.Unspecified))), Is.EqualTo(RAW_TEXT));
                Assert.That(Encoding.UTF8.GetString(EncodedString.FromBase64String(new EncodedString(ENC_TEXT, EncodingKinds.Base64))), Is.EqualTo(RAW_TEXT));
                Assert.Catch(() => Encoding.UTF8.GetString(EncodedString.FromBase64String(new EncodedString(ENC_TEXT, EncodingKinds.Hex))));
            });
        }
    }
}
