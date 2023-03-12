using System;

namespace SKIT.FlurlHttpClient
{
    using SKIT.FlurlHttpClient.Configuration;

    /// <summary>
    /// SKIT.FlurlHttpClient 通用响应基类。
    /// </summary>
    public abstract class CommonResponseBase : ICommonResponse
    {
        internal protected CommonResponseBase()
        {
            RawHeaders = HttpHeaderCollection.Empty;
            RawBytes = Array.Empty<byte>();
        }

        /// <inheritdoc/>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public int RawStatus { get; internal set; }

        /// <inheritdoc/>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public HttpHeaderCollection RawHeaders { get; internal set; }

        /// <inheritdoc/>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public byte[] RawBytes { get; internal set; }

        /// <inheritdoc/>
        public abstract bool IsSuccessful();
    }
}
