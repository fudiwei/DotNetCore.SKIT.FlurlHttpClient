using System;

namespace SKIT.FlurlHttpClient
{
    using SKIT.FlurlHttpClient.Configuration;

    /// <summary>
    /// SKIT.FlurlHttpClient 客户端接口。
    /// </summary>
    public interface ICommonClient : IDisposable
    {
        /// <summary>
        /// 获取当前客户端的拦截器集合。
        /// </summary>
        public FlurlHttpCallInterceptorCollection Interceptors { get; }

        /// <summary>
        /// 获取当前客户端使用的 JSON 序列化器。
        /// </summary>
        public IJsonSerializer JsonSerializer { get; }

        /// <summary>
        /// 配置客户端。
        /// </summary>
        /// <param name="configure"></param>
        public void Configure(Action<CommonClientSettings> configure);
    }
}
