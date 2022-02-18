using System;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    public class CodeAnalyzerException : Exception
    {
        private const string PREFIX_MESSAGE = "[ISSUE] ";

        public CodeAnalyzerException(string issue)
            : base(PREFIX_MESSAGE + issue)
        { 
        }

        public CodeAnalyzerException(string issue, Exception innerException)
            : base(PREFIX_MESSAGE + issue, innerException)
        {
        }

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
