using System.Text.Json;

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
                RequestMessage = request,
                StatusCode = HttpStatusCode.OK,
                ReasonPhrase = "OK",
                Content = new StringContent(JsonSerializer.Serialize(new Dictionary<string, object?>()
                {
                    { "ret", true },
                    { "req_data", reqBytes }
                })),
                Version = request.Version
            };
            httpResponseMessage.Headers.TryAddWithoutValidation("X-Test-Multi", new[] { "value-1", "value-2" });
            return httpResponseMessage;
        }
    }
}
