using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using NUnit.Framework;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases.Configuration
{
    using SKIT.FlurlHttpClient.Configuration;

    public class TestCase_ConfigureHttpInterceptorTest
    {
        private class MockInterceptor1 : HttpInterceptor
        {
            public override Task BeforeCallAsync(HttpInterceptorContext context, CancellationToken cancellationToken = default)
            {
                Assert.IsFalse(context.Items.ContainsKey("TEST_KEY"));
                context.Items.Add("TEST_KEY", "TEST_VALUE_1");

                return base.BeforeCallAsync(context, cancellationToken);
            }

            public override Task AfterCallAsync(HttpInterceptorContext context, CancellationToken cancellationToken = default)
            {
                Assert.AreEqual(context.Items["TEST_KEY"], "TEST_VALUE_2");

                return base.AfterCallAsync(context, cancellationToken);
            }
        }

        private class MockInterceptor2 : HttpInterceptor
        {
            public override Task BeforeCallAsync(HttpInterceptorContext context, CancellationToken cancellationToken = default)
            {
                Assert.AreEqual(context.Items["TEST_KEY"], "TEST_VALUE_1");

                return base.BeforeCallAsync(context, cancellationToken);
            }

            public override Task AfterCallAsync(HttpInterceptorContext context, CancellationToken cancellationToken = default)
            {
                Assert.AreEqual(context.Items["TEST_KEY"], "TEST_VALUE_1");
                context.Items["TEST_KEY"] = "TEST_VALUE_2";

                return base.AfterCallAsync(context, cancellationToken);
            }
        }

        [Test(Description = "测试用例：配置项之拦截器")]
        public async Task TestClientConfigureJsonSerializer()
        {
            using var client = new MockTestClient();
            client.Configure(settings => settings.FlurlHttpClientFactory = new MockTestHttpClientFactory());
            client.Interceptors.Add(new MockInterceptor1());
            client.Interceptors.Add(new MockInterceptor2());
            Assert.IsNotNull(client.Interceptors);

            IFlurlRequest flurlRequest = client.BuildFlurlRequest(new MockTestRequest(), HttpMethod.Post, "mock_url");
            var response = await client.SendFlurlRequestAsJsonAsync<MockTestResponse>(flurlRequest);
            Assert.IsTrue(response.IsSuccessful());
            Assert.NotNull(response.RawStatus);
            Assert.NotNull(response.RawBytes);
            Assert.NotNull(response.RawHeaders);
        }
    }
}
