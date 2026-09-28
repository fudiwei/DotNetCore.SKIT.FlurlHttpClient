using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using NUnit.Framework;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases
{
    public class TestCase_ConfigureHttpInterceptorTest
    {
        private class MockInterceptor1 : HttpInterceptor
        {
            public override Task BeforeCallAsync(HttpInterceptorContext context, CancellationToken cancellationToken = default)
            {
                Assert.That(context.Items.ContainsKey("TEST_KEY"), Is.False);

                context.Items.Add("TEST_KEY", "TEST_VALUE_1");

                return base.BeforeCallAsync(context, cancellationToken);
            }

            public override Task AfterCallAsync(HttpInterceptorContext context, CancellationToken cancellationToken = default)
            {
                Assert.That(context.Items["TEST_KEY"], Is.EqualTo("TEST_VALUE_2"));

                return base.AfterCallAsync(context, cancellationToken);
            }
        }

        private class MockInterceptor2 : HttpInterceptor
        {
            public override Task BeforeCallAsync(HttpInterceptorContext context, CancellationToken cancellationToken = default)
            {
                Assert.That(context.Items["TEST_KEY"], Is.EqualTo("TEST_VALUE_1"));

                return base.BeforeCallAsync(context, cancellationToken);
            }

            public override Task AfterCallAsync(HttpInterceptorContext context, CancellationToken cancellationToken = default)
            {
                Assert.That(context.Items["TEST_KEY"], Is.EqualTo("TEST_VALUE_1"));

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
                    Assert.That(mockClient.Interceptors, Is.Not.Null);
                }
                else
                {
                    Assert.Fail("The original client is not of type 'MockTestClient'.");
                }

                var request = context.GetOriginalRequest();
                if (request is MockTestRequest mockRequest)
                {
                    Assert.That(mockRequest.Data, Is.EqualTo("MockInterceptor"));
                }
                else
                {
                    Assert.Fail("The original request is not of type 'MockTestRequest'.");
                }

                return base.BeforeCallAsync(context, cancellationToken);
            }
        }

        [Test(Description = "测试用例：配置项之拦截器")]
        public async Task TestClientConfigure_HttpInterceptor()
        {
            using var client = new MockTestClient();
            client.Interceptors.Add(new MockInterceptor1());
            client.Interceptors.Add(new MockInterceptor2());
            client.Interceptors.Add(new MockInterceptor3());
            Assert.That(client.Interceptors, Is.Not.Null);

            await Assert.MultipleAsync(async () =>
            {
                var request = new MockTestRequest();
                request.Data = "MockInterceptor";

                IFlurlRequest flurlRequest = client.CreateFlurlRequest(request, HttpMethod.Post, "mock_url");

                var response = await client.SendFlurlRequestAsJsonAsync<MockTestResponse>(flurlRequest);

                Assert.That(response.IsSuccessful(), Is.True);
                Assert.That(response.GetRawStatus(), Is.Not.Null);
                Assert.That(response.GetRawBytes(), Is.Not.Null);
                Assert.That(response.GetRawHeaders(), Is.Not.Null);
            });
        }
    }
}
