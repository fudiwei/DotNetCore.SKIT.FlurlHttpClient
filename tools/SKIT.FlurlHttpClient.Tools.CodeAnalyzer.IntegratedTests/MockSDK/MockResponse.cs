namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests.MockSDK
{
    public abstract class MockResponse : CommonResponseBase
    {
        public override bool IsSuccessful()
        {
            return true;
        }
    }
}
