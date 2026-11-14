namespace SKIT.FlurlHttpClient.UnitTests.TestCases
{
    public class TestCase_SendHttpContentTest
    {
        [Fact]
        public async Task TestSendJson_PreservesSerializedBody()
        {
            using var client = new MockTestClient();
            var data = new { text = "request body", enabled = false };
            IFlurlRequest request = client.CreateFlurlRequest(new MockTestRequest(), HttpMethod.Post, "mock_url");

            MockTestResponse response = await client.SendFlurlRequestAsJsonAsync<MockTestResponse>(request, data, TestContext.Current.CancellationToken);

            Assert.True(response.IsSuccessful());
            Assert.Equal(Encoding.UTF8.GetBytes(client.JsonSerializer.Serialize(data, data.GetType())),
                Convert.FromBase64String(response.EncodingRequestData));
        }

        [Fact]
        public async Task TestSendForm_PreservesEncodedBody()
        {
            using var client = new MockTestClient();
            using var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "name", "value with spaces & symbols" }
            });
            byte[] expected = Encoding.UTF8.GetBytes("name=value+with+spaces+%26+symbols");
            IFlurlRequest request = client.CreateFlurlRequest(new MockTestRequest(), HttpMethod.Post, "mock_url");

            MockTestResponse response = await client.SendFlurlRequestAsync<MockTestResponse>(request, content, TestContext.Current.CancellationToken);

            Assert.Equal(expected, Convert.FromBase64String(response.EncodingRequestData));
        }

        [Fact]
        public async Task TestSendStream_PreservesBinaryBody()
        {
            using var client = new MockTestClient();
            byte[] expected = new byte[] { 0, 1, 127, 128, 255 };
            using var stream = new System.IO.MemoryStream(expected);
            using var content = new StreamContent(stream);
            IFlurlRequest request = client.CreateFlurlRequest(new MockTestRequest(), HttpMethod.Put, "mock_url");

            MockTestResponse response = await client.SendFlurlRequestAsync<MockTestResponse>(request, content, TestContext.Current.CancellationToken);

            Assert.Equal(expected, Convert.FromBase64String(response.EncodingRequestData));
        }

        [Fact]
        public async Task TestSendWithoutContent_RemainsEmpty()
        {
            using var client = new MockTestClient();
            IFlurlRequest request = client.CreateFlurlRequest(new MockTestRequest(), HttpMethod.Get, "mock_url");

            MockTestResponse response = await client.SendFlurlRequestAsync<MockTestResponse>(request, cancellationToken: TestContext.Current.CancellationToken);

            Assert.True(response.IsSuccessful());
            Assert.Empty(Convert.FromBase64String(response.EncodingRequestData));
        }
    }
}
