namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    /// <summary>
    /// 静态文件种类。
    /// </summary>
    public enum StaticFileKind
    {
        /// <summary>
        /// 未知。
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 接口请求模型源代码。
        /// </summary>
        RequestModelCode,

        /// <summary>
        /// 接口请求模型样例。
        /// </summary>
        RequestModelFixture,

        /// <summary>
        /// 接口响应模型源代码。
        /// </summary>
        ResponseModelCode,

        /// <summary>
        /// 接口响应模型样例。
        /// </summary>
        ResponseModelFixture,

        /// <summary>
        /// 接口方法源代码。
        /// </summary>
        ExecutingMethodCode,

        /// <summary>
        /// 回调通知事件源代码。
        /// </summary>
        WebhookEventCode,

        /// <summary>
        /// 回调通知事件样例。
        /// </summary>
        WebhookEventFixture,
    }
}
