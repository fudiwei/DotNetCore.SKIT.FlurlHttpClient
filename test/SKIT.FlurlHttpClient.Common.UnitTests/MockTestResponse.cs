namespace SKIT.FlurlHttpClient
{
    public class MockTestResponse : CommonResponseBase
    {
        [Newtonsoft.Json.JsonProperty("ret")]
        [System.Text.Json.Serialization.JsonPropertyName("ret")]
        public bool ReturnValue { get; set; }

        public MockTestResponse()
        {
        }

        public override bool IsSuccessful()
        {
            return RawStatus >= 200 && RawStatus < 300 && ReturnValue;
        }
    }
}
