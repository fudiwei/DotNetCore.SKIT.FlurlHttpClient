namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    /// <summary>
    /// 类型声明种类。
    /// </summary>
    public enum TypeDeclarationKind
    {
        /// <summary>
        /// 未知。
        /// </summary>
        Unkown = 0,

        /// <summary>
        /// 接口请求模型。
        /// </summary>
        RequestModel,

        /// <summary>
        /// 接口响应模型。
        /// </summary>
        ResponseModel,

        /// <summary>
        /// 接口方法。
        /// </summary>
        ExecutingMethod,

        /// <summary>
        /// 回调通知事件。
        /// </summary>
        WebhookEvent
    }
}
