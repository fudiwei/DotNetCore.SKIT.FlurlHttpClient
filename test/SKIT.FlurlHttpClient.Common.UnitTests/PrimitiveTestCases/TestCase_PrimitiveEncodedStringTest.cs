namespace SKIT.FlurlHttpClient.UnitTests.TestCases
{
    using SKIT.FlurlHttpClient.Primitives;

    public class TestCase_PrimitiveEncodedStringTest
    {
        [Fact(DisplayName = "测试用例：EncodedString")]
        public void TestSecurityEncodedString()
        {
            {
                const string TEXT = "SKIT.FlurlHttpClient is AWESOME!";

                Assert.Equal(TEXT, (string?)new EncodedString(TEXT));

                Assert.Equal(new EncodedString(TEXT), (EncodedString)TEXT);
            }

            {
                Assert.Equal(new EncodedString(null), new EncodedString(null));
                Assert.Equal(new EncodedString(""), new EncodedString(""));
                Assert.Equal(new EncodedString("Unspecified", EncodingKinds.Unspecified), new EncodedString("Unspecified", EncodingKinds.Unspecified));
                Assert.Equal(new EncodedString("Hex", EncodingKinds.Hex), new EncodedString("Hex", EncodingKinds.Hex));
                Assert.Equal(new EncodedString("Base64", EncodingKinds.Base64), new EncodedString("Base64", EncodingKinds.Base64));

                Assert.NotEqual(new EncodedString(null), new EncodedString(""));
                Assert.NotEqual(new EncodedString("", EncodingKinds.Base64), new EncodedString("", EncodingKinds.Hex));
                Assert.NotEqual(new EncodedString("", EncodingKinds.Unspecified), new EncodedString("Unspecified", EncodingKinds.Unspecified));
                Assert.NotEqual(new EncodedString("Hex", EncodingKinds.Hex), new EncodedString("Unspecified", EncodingKinds.Unspecified));
                Assert.NotEqual(new EncodedString("Base64", EncodingKinds.Base64), new EncodedString("Unspecified", EncodingKinds.Unspecified));
                Assert.NotEqual(new EncodedString("Unspecified", EncodingKinds.Unspecified), new EncodedString("Hex", EncodingKinds.Hex));
                Assert.NotEqual(new EncodedString("", EncodingKinds.Hex), new EncodedString("Hex", EncodingKinds.Hex));
                Assert.NotEqual(new EncodedString("Base64", EncodingKinds.Base64), new EncodedString("Hex", EncodingKinds.Hex));
                Assert.NotEqual(new EncodedString("Unspecified", EncodingKinds.Unspecified), new EncodedString("Base64", EncodingKinds.Base64));
                Assert.NotEqual(new EncodedString("Hex", EncodingKinds.Hex), new EncodedString("Base64", EncodingKinds.Base64));
                Assert.NotEqual(new EncodedString("", EncodingKinds.Base64), new EncodedString("Base64", EncodingKinds.Base64));

                Assert.True(new EncodedString(null) == new EncodedString(null));
                Assert.True(new EncodedString("") == new EncodedString(""));
                Assert.True(new EncodedString("Unspecified", EncodingKinds.Unspecified) == new EncodedString("Unspecified", EncodingKinds.Unspecified));
                Assert.True(new EncodedString("Hex", EncodingKinds.Hex) == new EncodedString("Hex", EncodingKinds.Hex));
                Assert.True(new EncodedString("Base64", EncodingKinds.Base64) == new EncodedString("Base64", EncodingKinds.Base64));

                Assert.True(new EncodedString("") != new EncodedString(null));
                Assert.True(new EncodedString("", EncodingKinds.Hex) != new EncodedString("", EncodingKinds.Base64));
                Assert.True(new EncodedString("Unspecified", EncodingKinds.Unspecified) != new EncodedString("", EncodingKinds.Unspecified));
                Assert.True(new EncodedString("Unspecified", EncodingKinds.Unspecified) != new EncodedString("Hex", EncodingKinds.Hex));
                Assert.True(new EncodedString("Unspecified", EncodingKinds.Unspecified) != new EncodedString("Base64", EncodingKinds.Base64));
                Assert.True(new EncodedString("Hex", EncodingKinds.Hex) != new EncodedString("Unspecified", EncodingKinds.Unspecified));
                Assert.True(new EncodedString("Hex", EncodingKinds.Hex) != new EncodedString("", EncodingKinds.Hex));
                Assert.True(new EncodedString("Hex", EncodingKinds.Hex) != new EncodedString("Base64", EncodingKinds.Base64));
                Assert.True(new EncodedString("Base64", EncodingKinds.Base64) != new EncodedString("Unspecified", EncodingKinds.Unspecified));
                Assert.True(new EncodedString("Base64", EncodingKinds.Base64) != new EncodedString("Hex", EncodingKinds.Hex));
                Assert.True(new EncodedString("Base64", EncodingKinds.Base64) != new EncodedString("", EncodingKinds.Base64));
            }

            {
                const string RAW_TEXT = "SKIT.FlurlHttpClient is AWESOME!";
                const string ENC_TEXT = "534B49542E466C75726C48747470436C69656E7420697320415745534F4D4521";

                Assert.Equal(EncodingKinds.Hex, EncodedString.ToString(Encoding.UTF8.GetBytes(RAW_TEXT), EncodingKinds.Hex).EncodingKind);
                Assert.Equal(ENC_TEXT, EncodedString.ToString(Encoding.UTF8.GetBytes(RAW_TEXT), EncodingKinds.Hex).Value);
                Assert.Equal(EncodingKinds.Hex, EncodedString.ToHexString(Encoding.UTF8.GetBytes(RAW_TEXT)).EncodingKind);
                Assert.Equal(ENC_TEXT, EncodedString.ToHexString(Encoding.UTF8.GetBytes(RAW_TEXT)).Value);

                Assert.Equal(RAW_TEXT, Encoding.UTF8.GetString(EncodedString.FromString(ENC_TEXT, EncodingKinds.Hex)), ignoreCase: true);
                Assert.Equal(RAW_TEXT, Encoding.UTF8.GetString(EncodedString.FromHexString(new EncodedString(ENC_TEXT))), ignoreCase: true);
                Assert.Equal(RAW_TEXT, Encoding.UTF8.GetString(EncodedString.FromHexString(new EncodedString(ENC_TEXT, EncodingKinds.Unspecified))), ignoreCase: true);
                Assert.Equal(RAW_TEXT, Encoding.UTF8.GetString(EncodedString.FromHexString(new EncodedString(ENC_TEXT, EncodingKinds.Hex))), ignoreCase: true);
                Assert.Throws<FormatException>(() => Encoding.UTF8.GetString(EncodedString.FromHexString(new EncodedString(ENC_TEXT, EncodingKinds.Base64))));
            }

            {
                const string RAW_TEXT = "SKIT.FlurlHttpClient is AWESOME!";
                const string ENC_TEXT = "U0tJVC5GbHVybEh0dHBDbGllbnQgaXMgQVdFU09NRSE=";

                Assert.Equal(EncodingKinds.Base64, EncodedString.ToString(Encoding.UTF8.GetBytes(RAW_TEXT), EncodingKinds.Base64).EncodingKind);
                Assert.Equal(ENC_TEXT, EncodedString.ToString(Encoding.UTF8.GetBytes(RAW_TEXT), EncodingKinds.Base64).Value);
                Assert.Equal(EncodingKinds.Base64, EncodedString.ToBase64String(Encoding.UTF8.GetBytes(RAW_TEXT)).EncodingKind);
                Assert.Equal(ENC_TEXT, EncodedString.ToBase64String(Encoding.UTF8.GetBytes(RAW_TEXT)).Value);

                Assert.Equal(RAW_TEXT, Encoding.UTF8.GetString(EncodedString.FromString(ENC_TEXT, EncodingKinds.Base64)));
                Assert.Equal(RAW_TEXT, Encoding.UTF8.GetString(EncodedString.FromBase64String(new EncodedString(ENC_TEXT))));
                Assert.Equal(RAW_TEXT, Encoding.UTF8.GetString(EncodedString.FromBase64String(new EncodedString(ENC_TEXT, EncodingKinds.Unspecified))));
                Assert.Equal(RAW_TEXT, Encoding.UTF8.GetString(EncodedString.FromBase64String(new EncodedString(ENC_TEXT, EncodingKinds.Base64))));
                Assert.Throws<FormatException>(() => Encoding.UTF8.GetString(EncodedString.FromBase64String(new EncodedString(ENC_TEXT, EncodingKinds.Hex))));
            }
        }
    }
}
