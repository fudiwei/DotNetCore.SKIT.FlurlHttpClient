using System;

namespace SKIT.FlurlHttpClient
{
    /// <summary>
    /// SKIT.FlurlHttpClient 引发的异常基类。
    /// </summary>
    public class CommonException : Exception
    {
        /// <inheritdoc/>
        public CommonException()
        {
        }

        /// <inheritdoc/>
        public CommonException(string message)
            : base(message)
        {
        }

        /// <inheritdoc/>
        public CommonException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
