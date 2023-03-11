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
        /// 获取当前客户端使用的拦截器集合。
        /// </summary>
        public HttpInterceptorCollection Interceptors { get; }

        /// <summary>
        /// 获取当前客户端使用的针对 "application/json" 内容请求的序列化器。
        /// </summary>
        public IJsonSerializer JsonSerializer { get; }

        /// <summary>
        /// 获取当前客户端使用的针对 "application/x-www-form-urlencoded" 内容请求的序列化器。
        /// </summary>
        public IFormUrlEncodedSerializer FormUrlEncodedSerializer { get; }

        /// <summary>
        /// 配置客户端。
        /// </summary>
        /// <param name="configure"></param>
        public void Configure(Action<CommonClientSettings> configure);
    }
}
