namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests.MockSdk.Models
{
    /// <summary>
    /// 表示 [GET] /foo/{id} 接口的请求。
    /// </summary>
    public class GetFooRequest : MockRequest
    {
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public string Id { get; set; } = string.Empty;
    }
}
