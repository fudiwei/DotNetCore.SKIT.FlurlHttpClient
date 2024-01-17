using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;

namespace SKIT.FlurlHttpClient
{
    /// <summary>
    /// SKIT.FlurlHttpClient HTTP 拦截器基类。
    /// </summary>
    public abstract class HttpInterceptor
    {
        /// <summary>
        /// 在 <see cref="FlurlCall"/> 调用前触发的函数。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task BeforeCallAsync(HttpInterceptorContext context, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 在 <see cref="FlurlCall"/> 调用后触发的函数。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task AfterCallAsync(HttpInterceptorContext context, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
