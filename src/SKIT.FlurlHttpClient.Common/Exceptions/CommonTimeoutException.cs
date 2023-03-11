using System;

namespace SKIT.FlurlHttpClient.Exceptions
{
    /// <summary>
    /// 表示 SKIT.FlurlHttpClient 超时引发的异常。
    /// </summary>
    public class CommonTimeoutException : CommonException
    {
        /// <inheritdoc/>
        public CommonTimeoutException()
        {
        }

        /// <inheritdoc/>
        public CommonTimeoutException(string message)
            : base(message)
        {
        }

        /// <inheritdoc/>
        public CommonTimeoutException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
