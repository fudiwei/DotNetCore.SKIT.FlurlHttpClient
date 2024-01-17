using NUnit.Framework;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases.Configuration
{
    using SKIT.FlurlHttpClient.Configuration;

    public class TestCase_ConfigureJsonSerializerTest
    {
        [Test(Description = "测试用例：配置项之 JSON 序列化器")]
        public void TestClientConfigure_JsonSerializer()
        {
            using var client = new MockTestClient();
            Assert.That(client.JsonSerializer, Is.Not.Null);

            var newtonsoftJsonSerializer = new NewtonsoftJsonSerializer();
            client.Configure(settings =>
            {
                settings.JsonSerializer = newtonsoftJsonSerializer;
            });
            Assert.That(client.JsonSerializer, Is.SameAs(newtonsoftJsonSerializer));

            var systemTextJsonSerializer = new SystemTextJsonSerializer();
            client.Configure(settings =>
            {
                settings.JsonSerializer = systemTextJsonSerializer;
            });
            Assert.That(client.JsonSerializer, Is.SameAs(systemTextJsonSerializer));
        }
    }
}
