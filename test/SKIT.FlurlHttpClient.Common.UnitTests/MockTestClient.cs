using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;

namespace SKIT.FlurlHttpClient
{
    public class MockTestClient : CommonClientBase
    {
        public MockTestClient()
#if NETFRAMEWORK
            : base(new HttpClient(new MockTestHttpMessageHandler(new HttpClientHandler())))
#else
            : base(new HttpClient(new MockTestHttpMessageHandler(new SocketsHttpHandler())))
#endif
        {
            FlurlClient.BaseUrl = "http://localhost:5050/";
        }

        public IFlurlRequest CreateFlurlRequest(MockTestRequest request, HttpMethod method, params object[] urlSegments)
        {
            return base.CreateFlurlRequest(request, method, urlSegments);
        }

        public async Task<T> SendFlurlRequestAsync<T>(IFlurlRequest flurlRequest, HttpContent? httpContent = null, CancellationToken cancellationToken = default)
            where T : MockTestResponse, new()
        {
            if (flurlRequest is null) throw new ArgumentNullException(nameof(flurlRequest));

            using IFlurlResponse flurlResponse = await base.SendFlurlRequestAsync(flurlRequest, httpContent, cancellationToken);
            return await WrapFlurlResponseAsJsonAsync<T>(flurlResponse, cancellationToken);
        }

        public async Task<T> SendFlurlRequestAsJsonAsync<T>(IFlurlRequest flurlRequest, object? data = null, CancellationToken cancellationToken = default)
            where T : MockTestResponse, new()
        {
            if (flurlRequest is null) throw new ArgumentNullException(nameof(flurlRequest));

            bool isSimpleRequest = data == null ||
                flurlRequest.Verb == HttpMethod.Get ||
                flurlRequest.Verb == HttpMethod.Head ||
                flurlRequest.Verb == HttpMethod.Options;
            using IFlurlResponse flurlResponse = isSimpleRequest ?
                await base.SendFlurlRequestAsync(flurlRequest, null, cancellationToken) :
                await base.SendFlurlRequestAsJsonAsync(flurlRequest, data, cancellationToken);
            return await WrapFlurlResponseAsJsonAsync<T>(flurlResponse, cancellationToken);
        }
    }
}
