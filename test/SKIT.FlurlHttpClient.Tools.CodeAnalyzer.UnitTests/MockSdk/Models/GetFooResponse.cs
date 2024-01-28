namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests.MockSdk.Models
{
    /// <summary>
    /// 表示 [GET] /foo/{id} 接口的响应。
    /// </summary>
    public class GetFooResponse : MockResponse
    {
        [Newtonsoft.Json.JsonProperty("res")]
        [System.Text.Json.Serialization.JsonPropertyName("res")]
        public int Result { get; set; }
    }

    public class GetFooResponse<T> : GetFooResponse
    {
        [Newtonsoft.Json.JsonProperty("data")]
        [System.Text.Json.Serialization.JsonPropertyName("data")]
        public T? Data { get; set; }
    }
}
