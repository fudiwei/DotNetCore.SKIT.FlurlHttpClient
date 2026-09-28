namespace SKIT.FlurlHttpClient.UnitTests.TestCases
{
    public class TestCase_ConfigureFormUrlEncodedSerializerTest
    {
        private sealed class MockObject
        {
            [Newtonsoft.Json.JsonProperty(Order = 1)]
            [System.Text.Json.Serialization.JsonPropertyOrder(1)]
            public string? PropertyAsString { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 2)]
            [System.Text.Json.Serialization.JsonPropertyOrder(2)]
            public int PropertyAsDateInt32 { get; set; }
        }

        private static void TestConfigureFormUrlEncodedSerializer(JsonifiedFormUrlEncodedSerializer formUrlEncodedSerializer)
        {
            using var client = new MockTestClient();
            Assert.NotNull(client.FormUrlEncodedSerializer);

            client.Configure(settings => settings.FormUrlEncodedSerializer = formUrlEncodedSerializer);
            Assert.Same(formUrlEncodedSerializer, client.FormUrlEncodedSerializer);

            // 模拟请求：复杂字典
            {
                var expectObj = new SortedDictionary<string, object?>(new Dictionary<string, object?>()
                {
                    { "integer", 12345 },
                    { "string", "abcdef"  },
                    { "boolean", true  },
                    { "guid", Guid.Parse("12345678-ffff-ffff-ffff-123456789abc")  },
                    { "array", new string[] { "你好", "世界" } },
                    { "object", new { key = "value" } }
                }, StringComparer.Ordinal);

                const string EXPECTED = "array[0]=%E4%BD%A0%E5%A5%BD&array[1]=%E4%B8%96%E7%95%8C&boolean=true&guid=12345678-ffff-ffff-ffff-123456789abc&integer=12345&object.key=value&string=abcdef";
                Assert.Equal(EXPECTED, client.FormUrlEncodedSerializer.Serialize(expectObj), ignoreCase: true);
            }

            // 模拟请求：带有自定义 JsonConverter 的对象
            {
                var expectObj = new MockObject()
                {
                    PropertyAsString = "你好，世界",
                    PropertyAsDateInt32 = 12345
                };

                const string EXPECTED = "PropertyAsString=%E4%BD%A0%E5%A5%BD%EF%BC%8C%E4%B8%96%E7%95%8C&PropertyAsDateInt32=12345";
                Assert.Equal(EXPECTED, client.FormUrlEncodedSerializer.Serialize(expectObj), ignoreCase: true);
            }
        }

        [Fact(DisplayName = "测试用例：配置项之 FormUrlEncoded 序列化器")]
        public void TestClientConfigure_FormUrlEncodedSerializer_JsonifiedFormUrlEncodedSerializer()
        {
            TestConfigureFormUrlEncodedSerializer(new JsonifiedFormUrlEncodedSerializer(new NewtonsoftJsonSerializer()));
            TestConfigureFormUrlEncodedSerializer(new JsonifiedFormUrlEncodedSerializer(new SystemTextJsonSerializer()));
        }
    }
}
