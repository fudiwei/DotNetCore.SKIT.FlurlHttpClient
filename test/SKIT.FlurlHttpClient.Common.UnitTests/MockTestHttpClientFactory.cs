using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http.Configuration;

namespace SKIT.FlurlHttpClient
{
    public class MockTestHttpClientFactory : DefaultHttpClientFactory
    {
        public override HttpMessageHandler CreateMessageHandler()
        {
            return new MockTestHttpMessageHandler(base.CreateMessageHandler());
        }
    }

    public class MockTestHttpMessageHandler : DelegatingHandler
    {
        public MockTestHttpMessageHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var resp = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"ret\":true}")
            };
            return Task.FromResult(resp);
        }
    }
}
