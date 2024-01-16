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
        public int GetRawStatus();

        /// <summary>
        /// 获取原始的 HTTP 响应标头集合。
        /// </summary>
        public HttpHeaderCollection GetRawHeaders();

        /// <summary>
        /// 获取原始的 HTTP 响应主体字节数组。
        /// </summary>
        public byte[] GetRawBytes();

        /// <summary>
        /// 获取一个值，该值指示调用 API 是否成功。
        /// </summary>
        /// <returns></returns>
        bool IsSuccessful();
    }
}
