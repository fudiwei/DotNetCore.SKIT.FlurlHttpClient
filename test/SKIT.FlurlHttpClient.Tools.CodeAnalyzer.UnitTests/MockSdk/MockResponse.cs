namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests.MockSdk
{
    public abstract class MockResponse : CommonResponseBase
    {
        public override bool IsSuccessful()
        {
            return true;
        }
    }
}
