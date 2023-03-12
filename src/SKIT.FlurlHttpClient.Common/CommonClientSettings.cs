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
        ///
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IJsonSerializer JsonSerializer { get; set; } = default!;

        /// <summary>
        ///
        /// </summary>
        public IFormUrlEncodedSerializer FormUrlEncodedSerializer { get; set; } = default!;

        /// <summary>
        ///
        /// </summary>
        public IHttpClientFactory FlurlHttpClientFactory { get; set; } = default!;

        internal CommonClientSettings(ClientFlurlHttpSettings flurlClientSettings)
        {
            Timeout = flurlClientSettings.Timeout;
            JsonSerializer = ((InternalWrappedJsonSerializer)flurlClientSettings.JsonSerializer)!.Serializer;
            FormUrlEncodedSerializer = ((InternalWrappedFormUrlEncodedSerializer)flurlClientSettings.UrlEncodedSerializer)!.Serializer;
            FlurlHttpClientFactory = flurlClientSettings.HttpClientFactory;
        }
    }
}
