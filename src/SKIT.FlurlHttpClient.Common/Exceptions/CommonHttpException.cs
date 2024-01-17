using System;

namespace SKIT.FlurlHttpClient
{
    /// <summary>
    /// 表示 SKIT.FlurlHttpClient 执行 HTTP 请求时引发的异常。
    /// </summary>
    public class CommonHttpException : CommonException
    {
        /// <inheritdoc/>
        public CommonHttpException()
        {
        }

        /// <inheritdoc/>
        public CommonHttpException(string message)
            : base(message)
        {
        }

        /// <inheritdoc/>
        public CommonHttpException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
