using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SKIT.FlurlHttpClient
{
    public class MockTestHttpMessageHandler : DelegatingHandler
    {
        public MockTestHttpMessageHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            byte[] reqBytes = Array.Empty<byte>();
            if (request.Content is not null)
                reqBytes = await request.Content.ReadAsByteArrayAsync();

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(new Dictionary<string, object?>()
                {
                    { "ret", true },
                    { "req_data", reqBytes }
                }))
            };
            return httpResponseMessage;
        }
    }
}
