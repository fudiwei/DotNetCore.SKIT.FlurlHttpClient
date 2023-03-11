using System;

namespace SKIT.FlurlHttpClient.Exceptions
{
    /// <summary>
    /// 表示 SKIT.FlurlHttpClient 超时引发的异常。
    /// </summary>
    public class CommonTimeoutException : CommonException
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public CommonTimeoutException()
        {
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="message"></param>
        public CommonTimeoutException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public CommonTimeoutException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
