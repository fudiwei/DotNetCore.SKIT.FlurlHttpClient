namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests.MockSdk.Models
{
    /// <summary>
    /// <para>表示 [POST] /foo 接口的响应。</para>
    /// </summary>
    public class PostFooResponse : MockResponse
    {
        [Newtonsoft.Json.JsonProperty("id")]
        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public string Id { get; set; } = default!;
    }
}
