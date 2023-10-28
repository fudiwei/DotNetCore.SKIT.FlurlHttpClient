using System;
using Flurl.Http.Configuration;

namespace SKIT.FlurlHttpClient
{
    using SKIT.FlurlHttpClient.Configuration;
    using SKIT.FlurlHttpClient.Configuration.Internal;

    /// <summary>
    /// SKIT.FlurlHttpClient 通用客户端配置项。
    /// </summary>
    public sealed class CommonClientSettings
    {
        /// <summary>
        /// 获取或设置客户端默认请求超时时间间隔。
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        /// 获取或设置客户端默认的 HTTP 协议版本号。
        /// </summary>
        public Version HttpVersion { get; set; } = default!;

        /// <summary>
        /// 获取或设置客户端用于序列化 "application/json" 内容的序列化器。
        /// </summary>
        public IJsonSerializer JsonSerializer { get; set; } = default!;

        /// <summary>
        /// 获取或设置客户端用于序列化 "application/x-www-form-urlencoded" 内容的序列化器。
        /// </summary>
        public IFormUrlEncodedSerializer FormUrlEncodedSerializer { get; set; } = default!;

        // TODO: Migrate to Flurl.Http v4.x.
        /// <summary>
        /// 
        /// </summary>
        public IHttpClientFactory FlurlHttpClientFactory { get; set; } = default!;

        internal CommonClientSettings(FlurlHttpSettings flurlSettings)
        {
            Timeout = flurlSettings.Timeout;
            HttpVersion = Version.Parse(flurlSettings.HttpVersion);
            JsonSerializer = ((InternalWrappedJsonSerializer)flurlSettings.JsonSerializer)!.Serializer;
            FormUrlEncodedSerializer = ((InternalWrappedFormUrlEncodedSerializer)flurlSettings.UrlEncodedSerializer)!.Serializer;
            FlurlHttpClientFactory = flurlSettings.HttpClientFactory;
        }
    }
}
