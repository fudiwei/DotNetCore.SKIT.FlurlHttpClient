namespace SKIT.FlurlHttpClient.UnitTests.TestCases
{
    public class TestCase_ConfigureJsonSerializerTest
    {
        [Fact(DisplayName = "测试用例：JSON 序列化器")]
        public void TestClientConfigure_JsonSerializer()
        {
            using var client = new MockTestClient();
            Assert.NotNull(client.JsonSerializer);

            var newtonsoftJsonSerializer = new NewtonsoftJsonSerializer();
            client.Configure(settings =>
            {
                settings.JsonSerializer = newtonsoftJsonSerializer;
            });
            Assert.Same(newtonsoftJsonSerializer, client.JsonSerializer);

            var systemTextJsonSerializer = new SystemTextJsonSerializer();
            client.Configure(settings =>
            {
                settings.JsonSerializer = systemTextJsonSerializer;
            });
            Assert.Same(systemTextJsonSerializer, client.JsonSerializer);
        }
    }
}
