using NUnit.Framework;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases.Configuration
{
    using SKIT.FlurlHttpClient.Configuration;

    public class TestCase_ConfigureJsonSerializerTest
    {
        [Test(Description = "测试用例：配置项之 JSON 序列化器")]
        public void TestClientConfigureJsonSerializer()
        {
            using var client = new MockTestClient();
            Assert.IsNotNull(client.JsonSerializer);

            var newtonsoftJsonSerializer = new NewtonsoftJsonSerializer();
            client.Configure(settings =>
            {
                settings.JsonSerializer = newtonsoftJsonSerializer;
            });
            Assert.AreSame(client.JsonSerializer, newtonsoftJsonSerializer);

            var systemTextJsonSerializer = new SystemTextJsonSerializer();
            client.Configure(settings =>
            {
                settings.JsonSerializer = systemTextJsonSerializer;
            });
            Assert.AreSame(client.JsonSerializer, systemTextJsonSerializer);
        }
    }
}
