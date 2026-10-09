namespace SKIT.FlurlHttpClient.UnitTests.TestCases
{
    public class TestCase_ConfigureHttpInterceptorTest
    {
        private class MockInterceptor1 : HttpInterceptor
        {
            public override Task BeforeCallAsync(HttpInterceptorContext context, CancellationToken cancellationToken = default)
            {
                Assert.False(context.Items.ContainsKey("TEST_KEY"));

                context.Items.Add("TEST_KEY", "TEST_VALUE_1");

                return base.BeforeCallAsync(context, cancellationToken);
            }

            public override Task AfterCallAsync(HttpInterceptorContext context, CancellationToken cancellationToken = default)
            {
                Assert.Equal("TEST_VALUE_2", context.Items["TEST_KEY"]);

                return base.AfterCallAsync(context, cancellationToken);
            }
        }

        private class MockInterceptor2 : HttpInterceptor
        {
            public override Task BeforeCallAsync(HttpInterceptorContext context, CancellationToken cancellationToken = default)
            {
                Assert.Equal("TEST_VALUE_1", context.Items["TEST_KEY"]);

                return base.BeforeCallAsync(context, cancellationToken);
            }

            public override Task AfterCallAsync(HttpInterceptorContext context, CancellationToken cancellationToken = default)
            {
                Assert.Equal("TEST_VALUE_1", context.Items["TEST_KEY"]);

                context.Items["TEST_KEY"] = "TEST_VALUE_2";

                return base.AfterCallAsync(context, cancellationToken);
            }
        }

        private class MockInterceptor3 : HttpInterceptor
        {
            public override Task BeforeCallAsync(HttpInterceptorContext context, CancellationToken cancellationToken = default)
            {
                var client = context.GetOriginalClient();
                if (client is MockTestClient mockClient)
                {
                    Assert.NotNull(mockClient.Interceptors);
                }
                else
                {
                    Assert.Fail("The original client is not of type 'MockTestClient'.");
                }

                var request = context.GetOriginalRequest();
                if (request is MockTestRequest mockRequest)
                {
                    Assert.Equal("MockInterceptor", mockRequest.Data);
                }
                else
                {
                    Assert.Fail("The original request is not of type 'MockTestRequest'.");
                }

                return base.BeforeCallAsync(context, cancellationToken);
            }
        }

        [Fact(DisplayName = "测试用例：拦截器")]
        public async Task TestClientConfigure_HttpInterceptor()
        {
            using var client = new MockTestClient();
            client.Interceptors.Add(new MockInterceptor1());
            client.Interceptors.Add(new MockInterceptor2());
            client.Interceptors.Add(new MockInterceptor3());
            Assert.NotNull(client.Interceptors);

            var request = new MockTestRequest();
            request.Data = "MockInterceptor";

            IFlurlRequest flurlRequest = client.CreateFlurlRequest(request, HttpMethod.Post, "mock_url");

            var response = await client.SendFlurlRequestAsJsonAsync<MockTestResponse>(flurlRequest, request, TestContext.Current.CancellationToken);

            Assert.True(response.IsSuccessful());
            Assert.NotEqual(0, response.GetRawStatus());
            Assert.NotNull(response.GetRawBytes());
            Assert.NotNull(response.GetRawHeaders());
        }
    }
}
