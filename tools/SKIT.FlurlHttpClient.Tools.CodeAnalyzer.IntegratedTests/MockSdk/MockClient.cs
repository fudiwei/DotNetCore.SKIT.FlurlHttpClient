using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests.MockSdk
{
    public class MockClient : CommonClientBase
    {
        public IFlurlRequest CreateFlurlRequest(MockRequest request, HttpMethod method, params object[] urlSegments)
        {
            return base.CreateFlurlRequest(request, method, urlSegments);
        }

        public async Task<T> SendFlurlRequestAsync<T>(IFlurlRequest flurlRequest, HttpContent? httpContent = null, CancellationToken cancellationToken = default)
            where T : MockResponse, new()
        {
            if (flurlRequest is null) throw new ArgumentNullException(nameof(flurlRequest));

            using IFlurlResponse flurlResponse = await base.SendFlurlRequestAsync(flurlRequest, httpContent, cancellationToken);
            return await WrapFlurlResponseAsJsonAsync<T>(flurlResponse, cancellationToken);
        }
    }
}
