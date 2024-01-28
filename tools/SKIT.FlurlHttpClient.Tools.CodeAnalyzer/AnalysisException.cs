using System;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    /// <summary>
    /// 代码质量分析时引发的异常。
    /// </summary>
    public class AnalysisException : Exception
    {
        private const string PREFIX_MESSAGE = "[ISSUE] ";

        /// <inheritdoc/>
        public AnalysisException(string issue)
            : base(PREFIX_MESSAGE + issue)
        {
        }

        /// <inheritdoc/>
        public AnalysisException(string issue, Exception innerException)
            : base(PREFIX_MESSAGE + issue, innerException)
        {
        }

        /// <inheritdoc/>
        public string ToFullString()
        {
            if (InnerException is null)
            {
                return Message;
            }

            Exception baseException = InnerException.GetBaseException() ?? InnerException;
            return $"{Message} (Fatal: {baseException.Message}, Stack: {baseException.StackTrace})";
        }
    }
}
