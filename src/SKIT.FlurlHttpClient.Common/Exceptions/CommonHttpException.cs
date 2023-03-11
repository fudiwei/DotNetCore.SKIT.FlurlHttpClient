using System;

namespace SKIT.FlurlHttpClient.Exceptions
{
    /// <summary>
    /// 表示 SKIT.FlurlHttpClient 执行 HTTP 请求时引发的异常。
    /// </summary>
    public class CommonHttpException : CommonException
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public CommonHttpException()
        {
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="message"></param>
        public CommonHttpException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public CommonHttpException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
