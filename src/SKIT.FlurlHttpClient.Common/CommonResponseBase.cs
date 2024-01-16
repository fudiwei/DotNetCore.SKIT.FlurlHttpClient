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
            _InternalRawHeaders = HttpHeaderCollection.Empty;
            _InternalRawBytes = Array.Empty<byte>();
        }

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        internal int _InternalRawStatus;

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        internal HttpHeaderCollection _InternalRawHeaders;

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        internal byte[] _InternalRawBytes;

        /// <inheritdoc/>
        public int GetRawStatus()
        {
            return _InternalRawStatus;
        }

        /// <inheritdoc/>
        public HttpHeaderCollection GetRawHeaders()
        {
            return _InternalRawHeaders;
        }

        /// <inheritdoc/>
        public byte[] GetRawBytes()
        {
            return _InternalRawBytes;
        }

        /// <inheritdoc/>
        public abstract bool IsSuccessful();
    }
}
