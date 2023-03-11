using System;

namespace SKIT.FlurlHttpClient
{
    /// <summary>
    /// SKIT.FlurlHttpClient 请求接口。
    /// </summary>
    public interface ICommonRequest
    {
        /// <summary>
        /// 设置当前请求的超时时间。
        /// <para>如果设置为 null，将使用客户端默认的全局超时时间。</para>
        /// </summary>
        /// <param name="timeout">超时时间。</param>
        public void WithTimeout(TimeSpan? timeout);
    }
}
