using System;

namespace SKIT.FlurlHttpClient
{
    /// <summary>
    /// SKIT.FlurlHttpClient 请求基类。
    /// </summary>
    public abstract class CommonRequestBase : ICommonRequest
    {
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        internal TimeSpan? Timeout { get; private set; }

        /// <inheritdoc/>
        public void WithTimeout(TimeSpan? timeout)
        {
            Timeout = timeout;
        }
    }
}
