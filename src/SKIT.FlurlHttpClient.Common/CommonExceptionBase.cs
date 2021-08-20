using System;

namespace SKIT.FlurlHttpClient
{
    /// <summary>
    /// SKIT.FlurlHttpClient 引发的异常基类。
    /// </summary>
    public abstract class CommonExceptionBase : Exception
    {
        /// <inheritdoc/>
        public CommonExceptionBase()
        {
        }

        /// <inheritdoc/>
        public CommonExceptionBase(string message) 
            : base(message)
        {
        }

        /// <inheritdoc/>
        public CommonExceptionBase(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
