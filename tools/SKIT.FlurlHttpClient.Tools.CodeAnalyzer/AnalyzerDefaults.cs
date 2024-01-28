namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    internal static class AnalyzerDefaults
    {
        public const string NAMING_REQUEST_MODEL_SUFFIX = "Request";
        public const string NAMING_RESPONSE_MODEL_SUFFIX = "Response";
        public const string NAMING_EXECUTING_METHOD_PREFIX = "Execute";
        public const string NAMING_EXECUTING_METHOD_SUFFIX = "Async";
        public const string NAMING_WEBHOOK_EVENT_SUFFIX = "Event";

        public const string DEFAULT_EXECUTING_EXTENSION_NAME_REGEX = "[a-zA-Z0-9]+ClientExecute[a-zA-Z0-9]+Extensions$";
    }
}
