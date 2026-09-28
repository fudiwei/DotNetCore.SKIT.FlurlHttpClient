using System;

namespace SKIT.FlurlHttpClient
{
    /// <summary>
    /// SKIT.FlurlHttpClient 通用客户端接口。
    /// </summary>
    public interface ICommonClient : IDisposable
    {
        /// <summary>
        /// 获取当前客户端使用的拦截器集合。
        /// </summary>
        public HttpInterceptorCollection Interceptors { get; }

        /// <summary>
        /// 获取当前客户端使用的针对 "application/json" 请求载荷的序列化器。
        /// </summary>
        public IJsonSerializer JsonSerializer { get; }

        /// <summary>
        /// 获取当前客户端使用的针对 "application/x-www-form-urlencoded" 请求载荷的序列化器。
        /// </summary>
        public IFormUrlEncodedSerializer FormUrlEncodedSerializer { get; }

        /// <summary>
        /// 配置客户端。
        /// </summary>
        /// <param name="configure"></param>
        public void Configure(Action<CommonClientSettings> configure);
    }
}
