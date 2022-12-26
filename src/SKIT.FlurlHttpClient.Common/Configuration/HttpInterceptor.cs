using System.Threading;
using System.Threading.Tasks;

namespace SKIT.FlurlHttpClient.Configuration
{
    /// <summary>
    /// SKIT.FlurlHttpClient HTTP 拦截器基类。
    /// </summary>
    public abstract class HttpInterceptor
    {
        public virtual Task BeforeCallAsync(HttpInterceptorContext context, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public virtual Task AfterCallAsync(HttpInterceptorContext context, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
