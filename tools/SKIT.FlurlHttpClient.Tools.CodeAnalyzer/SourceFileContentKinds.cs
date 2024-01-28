namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    /// <summary>
    /// 源码内容类型。
    /// </summary>
    public enum SourceFileContentKinds
    {
        /// <summary>
        /// 未知。
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// API 请求模型类的代码。
        /// </summary>
        RequestModelClassCode = 11,

        /// <summary>
        /// API 响应模型类的代码。
        /// </summary>
        ResponseModelClassCode = 12,

        /// <summary>
        /// API 接口方法类的代码。
        /// </summary>
        ExecutingExtensionClassCode = 13,

        /// <summary>
        /// 回调通知事件模型类的代码。
        /// </summary>
        WebhookEventClassCode = 21,

        /// <summary>
        /// API 请求模型的序列化后的样例。
        /// </summary>
        RequestModelSerializationSample = 31,

        /// <summary>
        /// API 响应模型的序列化后的样例。
        /// </summary>
        ResponseModelSerializationSample = 32,

        /// <summary>
        /// 回调通知事件模型的序列化后的样例。
        /// </summary>
        WebhookEventSerializationSample = 41
    }
}
