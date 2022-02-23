using System;
using Flurl.Http.Configuration;

namespace SKIT.FlurlHttpClient
{
    /// <summary>
    /// SKIT.FlurlHttpClient 客户端配置项。
    /// </summary>
    public sealed class CommonClientSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public TimeSpan? ConnectionRequestTimeout { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan? ConnectionLeaseTimeout { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ISerializer JsonSerializer { get; set; } = default!;

        /// <summary>
        /// 
        /// </summary>
        public ISerializer UrlEncodedSerializer { get; set; } = default!;

        /// <summary>
        /// 
        /// </summary>
        public IHttpClientFactory FlurlHttpClientFactory { get; set; } = default!;

        internal CommonClientSettings()
        {
        }

        internal CommonClientSettings(ClientFlurlHttpSettings flurlClientSettings)
            : this()
        {
            ConnectionRequestTimeout = flurlClientSettings.Timeout;
            ConnectionLeaseTimeout = flurlClientSettings.ConnectionLeaseTimeout;
            JsonSerializer = flurlClientSettings.JsonSerializer;
            UrlEncodedSerializer = flurlClientSettings.UrlEncodedSerializer;
            FlurlHttpClientFactory = flurlClientSettings.HttpClientFactory;
        }
    }
}
