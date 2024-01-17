using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases
{
    public class TestCase_ConfigureFormUrlEncodedSerializerTest
    {
        private sealed class MockObject
        {
            [Newtonsoft.Json.JsonProperty(Order = 1)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedStringArrayWithCommaSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(1)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.StringifiedStringArrayWithCommaSplitConverter))]
            public string[]? PropertyAsStringArray { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 2)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.BasicDateTimeOffsetConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(2)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.Common.BasicDateTimeOffsetConverter))]
            public DateTimeOffset PropertyAsDateTimeOffset { get; set; }
        }

        private static void TestConfigureFormUrlEncodedSerializer(JsonifiedFormUrlEncodedSerializer formUrlEncodedSerializer)
        {
            using var client = new MockTestClient();
            Assert.That(client.FormUrlEncodedSerializer, Is.Not.Null);

            client.Configure(settings => settings.FormUrlEncodedSerializer = formUrlEncodedSerializer);
            Assert.That(client.FormUrlEncodedSerializer, Is.SameAs(formUrlEncodedSerializer));

            // 模拟请求：复杂字典
            Assert.Multiple(() =>
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
                Assert.That(client.FormUrlEncodedSerializer.Serialize(expectObj), Is.EqualTo(EXPECTED).IgnoreCase);
            });

            // 模拟请求：带有自定义 JsonConverter 的对象
            Assert.Multiple(() =>
            {
                var expectObj = new MockObject()
                {
                    PropertyAsStringArray = new string[] { "你好", "世界" },
                    PropertyAsDateTimeOffset = new DateTimeOffset(2006, 1, 2, 15, 4, 5, TimeSpan.FromHours(8))
                };

                const string EXPECTED = "PropertyAsStringArray=%E4%BD%A0%E5%A5%BD%2C%E4%B8%96%E7%95%8C&PropertyAsDateTimeOffset=2006-01-02+15%3A04%3A05";
                Assert.That(client.FormUrlEncodedSerializer.Serialize(expectObj), Is.EqualTo(EXPECTED).IgnoreCase);
            });
        }

        [Test(Description = "测试用例：配置项之 FormUrlEncoded 序列化器")]
        public void TestClientConfigure_FormUrlEncodedSerializer_JsonifiedFormUrlEncodedSerializer()
        {
            TestConfigureFormUrlEncodedSerializer(new JsonifiedFormUrlEncodedSerializer(new NewtonsoftJsonSerializer()));
            TestConfigureFormUrlEncodedSerializer(new JsonifiedFormUrlEncodedSerializer(new SystemTextJsonSerializer()));
        }
    }
}
