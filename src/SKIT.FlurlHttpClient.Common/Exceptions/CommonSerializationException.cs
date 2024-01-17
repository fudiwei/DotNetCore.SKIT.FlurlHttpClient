using System;

namespace SKIT.FlurlHttpClient
{
    /// <summary>
    /// 表示 SKIT.FlurlHttpClient 数据序列化或反序列化失败引发的异常。
    /// </summary>
    public class CommonSerializationException : CommonException
    {
        /// <inheritdoc/>
        public CommonSerializationException()
        {
        }

        /// <inheritdoc/>
        public CommonSerializationException(string message)
            : base(message)
        {
        }

        /// <inheritdoc/>
        public CommonSerializationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
