using System;
using Flurl.Http;

namespace SKIT.FlurlHttpClient.Exceptions
{
    /// <summary>
    /// 表示 SKIT.FlurlHttpClient 执行拦截器时引发的异常。
    /// </summary>
    public class CommonInterceptorCallException : CommonException
    {
        /// <summary>
        /// 获取本次请求的上下文。
        /// </summary>
        public FlurlCall FlurlCall { get; }

        /// <inheritdoc/>
        public CommonInterceptorCallException(FlurlCall flurlCall)
        {
            FlurlCall = flurlCall;
        }

        /// <inheritdoc/>
        public CommonInterceptorCallException(FlurlCall flurlCall, string message)
            : base(message)
        {
            FlurlCall = flurlCall;
        }

        /// <inheritdoc/>
        public CommonInterceptorCallException(FlurlCall flurlCall, string message, Exception innerException)
            : base(message, innerException)
        {
            FlurlCall = flurlCall;
        }
    }
}
