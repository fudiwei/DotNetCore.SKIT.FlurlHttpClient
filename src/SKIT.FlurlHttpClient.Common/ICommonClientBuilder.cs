using System;
using System.Net.Http;

namespace SKIT.FlurlHttpClient
{
    /// <summary>
    /// SKIT.FlurlHttpClient 通用客户端建造者接口。
    /// </summary>
    public interface ICommonClientBuilder<TClient>
        where TClient : class, ICommonClient
    {
        /// <summary>
        /// 配置客户端。
        /// </summary>
        /// <param name="configure"></param>
        /// <returns></returns>
        ICommonClientBuilder<TClient> ConfigureSettings(Action<CommonClientSettings> configure);

        /// <summary>
        /// 添加并使用指定的拦截器。
        /// </summary>
        /// <param name="interceptor">拦截器。</param>
        /// <returns></returns>
        ICommonClientBuilder<TClient> UseInterceptor(HttpInterceptor interceptor);

        /// <summary>
        /// 添加并使用指定的 <see cref="HttpClient"/>。
        /// </summary>
        /// <param name="httpClient">指定的 <see cref="HttpClient"/>。</param>
        /// <param name="disposeClient">是否在释放此客户端时释放该 <see cref="HttpClient"/>。</param>
        /// <returns></returns>
        ICommonClientBuilder<TClient> UseHttpClient(HttpClient httpClient, bool disposeClient = true);

        /// <summary>
        /// 构建客户端。
        /// </summary>
        /// <returns></returns>
        TClient Build();
    }
}
