using System;

namespace SKIT.FlurlHttpClient
{
    /// <summary>
    /// SKIT.FlurlHttpClient 通用请求基类。
    /// </summary>
    public abstract class CommonRequestBase : ICommonRequest
    {
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        internal TimeSpan? _InternalTimeout;

        /// <inheritdoc/>
        public void WithTimeout(TimeSpan? timeout)
        {
            _InternalTimeout = timeout;
        }
    }
}
