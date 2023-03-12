namespace SKIT.FlurlHttpClient
{
    using SKIT.FlurlHttpClient.Configuration;

    /// <summary>
    /// SKIT.FlurlHttpClient 通用响应接口。
    /// </summary>
    public interface ICommonResponse
    {
        /// <summary>
        /// 获取原始的 HTTP 响应状态码。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public int RawStatus { get; }

        /// <summary>
        /// 获取原始的 HTTP 响应表头集合。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public HttpHeaderCollection RawHeaders { get; }

        /// <summary>
        /// 获取原始的 HTTP 响应正文。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public byte[] RawBytes { get; }

        /// <summary>
        /// 获取一个值，该值指示调用 API 是否成功。
        /// </summary>
        /// <returns></returns>
        bool IsSuccessful();
    }
}
