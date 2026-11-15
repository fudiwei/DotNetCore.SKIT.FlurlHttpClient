using System.IO;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases
{
    public class TestCase_ClientSendRequestTest
    {
        [Fact(DisplayName = "测试用例：发送请求之 JSON 请求体")]
        public async Task TestClientSendRequest_JsonPayload()
        {
            using var client = new MockTestClient();

            var payload = new { text = "request body", enabled = false };
            var request = client.CreateFlurlRequest(new MockTestRequest(), HttpMethod.Post, "mock_url");
            var response = await client.SendFlurlRequestAsJsonAsync<MockTestResponse>(request, payload, TestContext.Current.CancellationToken);

            byte[] expected = Encoding.UTF8.GetBytes(client.JsonSerializer.Serialize(payload, payload.GetType()));
            Assert.Equal(expected, Convert.FromBase64String(response.EncodingRequestData));
        }

        [Fact(DisplayName = "测试用例：发送请求之表单请求体")]
        public async Task TestClientSendRequest_FormUrlEncodedPayload()
        {
            using var client = new MockTestClient();

            using var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "name", "value with spaces & symbols" }
            });
            var request = client.CreateFlurlRequest(new MockTestRequest(), HttpMethod.Post, "mock_url");
            var response = await client.SendFlurlRequestAsync<MockTestResponse>(request, content, TestContext.Current.CancellationToken);

            byte[] expected = Encoding.UTF8.GetBytes("name=value+with+spaces+%26+symbols");
            Assert.Equal(expected, Convert.FromBase64String(response.EncodingRequestData));
        }

        [Fact(DisplayName = "测试用例：发送请求之二进制流请求体")]
        public async Task TestClientSendRequest_BinaryPayload()
        {
            using var client = new MockTestClient();

            using var stream = new MemoryStream(new byte[] { 0, 1, 127, 128, 255 });
            using var content = new StreamContent(stream);
            var request = client.CreateFlurlRequest(new MockTestRequest(), HttpMethod.Post, "mock_url");
            var response = await client.SendFlurlRequestAsync<MockTestResponse>(request, content, TestContext.Current.CancellationToken);

            byte[] expected = new byte[] { 0, 1, 127, 128, 255 };
            Assert.Equal(expected, Convert.FromBase64String(response.EncodingRequestData));
        }

        [Fact(DisplayName = "测试用例：发送请求之空请求体")]
        public async Task TestSendWithoutContent_EmptyPayload()
        {
            using var client = new MockTestClient();

            var request = client.CreateFlurlRequest(new MockTestRequest(), HttpMethod.Get, "mock_url");
            var response = await client.SendFlurlRequestAsync<MockTestResponse>(request, cancellationToken: TestContext.Current.CancellationToken);

            Assert.Empty(Convert.FromBase64String(response.EncodingRequestData));
        }
    }
}
