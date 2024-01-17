namespace SKIT.FlurlHttpClient
{
    public class MockTestResponse : CommonResponseBase
    {
        [Newtonsoft.Json.JsonProperty("ret")]
        [System.Text.Json.Serialization.JsonPropertyName("ret")]
        public bool ReturnValue { get; set; }

        [Newtonsoft.Json.JsonProperty("req_data")]
        [System.Text.Json.Serialization.JsonPropertyName("req_data")]
        public string EncodingRequestData { get; set; } = default!;

        public MockTestResponse()
        {
        }

        public override bool IsSuccessful()
        {
            return GetRawStatus() >= 200 && GetRawStatus() < 300 && ReturnValue;
        }
    }
}
