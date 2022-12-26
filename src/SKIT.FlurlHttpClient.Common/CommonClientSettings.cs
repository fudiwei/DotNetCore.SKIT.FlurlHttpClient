using System;
using Flurl.Http.Configuration;

namespace SKIT.FlurlHttpClient
{
    using SKIT.FlurlHttpClient.Configuration;
    using SKIT.FlurlHttpClient.Configuration.Internal;

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
        public IJsonSerializer JsonSerializer { get; set; } = default!;

        /// <summary>
        ///
        /// </summary>
        public ISerializer UrlEncodedSerializer { get; set; } = default!;

        /// <summary>
        ///
        /// </summary>
        public IHttpClientFactory FlurlHttpClientFactory { get; set; } = default!;

        /// <summary>
        ///
        /// </summary>
        public bool ThrowOnSerializationError { get; set; } = true;

        internal CommonClientSettings(ClientFlurlHttpSettings flurlClientSettings)
        {
            ConnectionRequestTimeout = flurlClientSettings.Timeout;
            ConnectionLeaseTimeout = flurlClientSettings.ConnectionLeaseTimeout;
            JsonSerializer = ((InternalWrappedJsonSerializer)flurlClientSettings.JsonSerializer)!.Serializer;
            UrlEncodedSerializer = flurlClientSettings.UrlEncodedSerializer;
            FlurlHttpClientFactory = flurlClientSettings.HttpClientFactory;
        }
    }
}
