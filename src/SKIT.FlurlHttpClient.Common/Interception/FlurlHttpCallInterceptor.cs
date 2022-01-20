using System.Threading.Tasks;
using Flurl.Http;

namespace SKIT.FlurlHttpClient
{
    public abstract class FlurlHttpCallInterceptor
    {
        public virtual Task BeforeCallAsync(FlurlCall flurlCall)
        {
            return Task.CompletedTask;
        }

        public virtual Task AfterCallAsync(FlurlCall flurlCall)
        {
            return Task.CompletedTask;
        }
    }
}
