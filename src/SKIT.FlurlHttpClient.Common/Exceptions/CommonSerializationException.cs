using System;

namespace SKIT.FlurlHttpClient.Exceptions
{
    /// <summary>
    /// 表示 SKIT.FlurlHttpClient 数据序列化或反序列化失败引发的异常。
    /// </summary>
    public class CommonSerializationException : CommonException
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public CommonSerializationException()
        {
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="message"></param>
        public CommonSerializationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public CommonSerializationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
