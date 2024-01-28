namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    /// <summary>
    /// 类型声明成员类型。
    /// </summary>
    public enum TypeDeclarationMemberKinds
    {
        /// <summary>
        /// 未知。
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// API 请求模型类。
        /// </summary>
        RequestModelClass = 11,

        /// <summary>
        /// API 响应默认类。
        /// </summary>
        ResponseModelClass = 12,

        /// <summary>
        /// API 接口方法类。
        /// </summary>
        ExecutingExtensionClass = 13,

        /// <summary>
        /// API 接口方法函数。
        /// </summary>
        ExecutingExtensionMethod = 14,

        /// <summary>
        /// 回调通知事件模型类。
        /// </summary>
        WebhookEventClass = 21
    }
}
