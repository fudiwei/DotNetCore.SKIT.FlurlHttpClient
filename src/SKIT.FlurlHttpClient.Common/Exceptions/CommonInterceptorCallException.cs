using System;
using Flurl.Http;

namespace SKIT.FlurlHttpClient.Exceptions
{
    /// <summary>
    /// 表示 SKIT.FlurlHttpClient 拦截器执行时引发的异常。
    /// </summary>
    public class CommonInterceptorCallException : CommonException
    {
        /// <summary>
        /// 
        /// </summary>
        public FlurlCall FlurlCall { get; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="flurlCall"></param>
        public CommonInterceptorCallException(FlurlCall flurlCall)
        {
            FlurlCall = flurlCall;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="flurlCall"></param>
        /// <param name="message"></param>
        public CommonInterceptorCallException(FlurlCall flurlCall, string message)
            : base(message)
        {
            FlurlCall = flurlCall;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="flurlCall"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public CommonInterceptorCallException(FlurlCall flurlCall, string message, Exception innerException)
            : base(message, innerException)
        {
            FlurlCall = flurlCall;
        }
    }
}
