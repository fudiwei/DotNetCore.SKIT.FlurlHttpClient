using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases.Configuration
{
    using SKIT.FlurlHttpClient.Configuration;

    public class TestCase_ConfigureFormUrlEncodedSerializerTest
    {
        private sealed class MockObject
        {
            [Newtonsoft.Json.JsonProperty(Order = 1)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.StringifiedStringArrayWithCommaSplitConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(1)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.StringifiedStringArrayWithCommaSplitConverter))]
            public string[]? PropertyAsStringArray { get; set; }

            [Newtonsoft.Json.JsonProperty(Order = 2)]
            [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.Common.BasicDateTimeOffsetConverter))]
            [System.Text.Json.Serialization.JsonPropertyOrder(2)]
            [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Converters.Common.BasicDateTimeOffsetConverter))]
            public DateTimeOffset PropertyAsDateTimeOffset { get; set; }
        }

        [Test(Description = "测试用例：配置项之 FormUrlEncoded 序列化器")]
        public void TestClientConfigure_FormUrlEncodedSerializer()
        {
            using var client = new MockTestClient();
            Assert.IsNotNull(client.FormUrlEncodedSerializer);

            var newtonsoftJsonFormUrlEncodedSerializer = new JsonifiedFormUrlEncodedSerializer(new NewtonsoftJsonSerializer());
            client.Configure(settings =>
            {
                settings.FormUrlEncodedSerializer = newtonsoftJsonFormUrlEncodedSerializer;
            });
            Assert.AreSame(client.FormUrlEncodedSerializer, newtonsoftJsonFormUrlEncodedSerializer);

            var systemTextJsonFormUrlEncodedSerializer = new JsonifiedFormUrlEncodedSerializer(new SystemTextJsonSerializer());
            client.Configure(settings =>
            {
                settings.FormUrlEncodedSerializer = systemTextJsonFormUrlEncodedSerializer;
            });
            Assert.AreSame(client.FormUrlEncodedSerializer, systemTextJsonFormUrlEncodedSerializer);
        }

        [Test(Description = "测试用例：配置项之 FormUrlEncoded 序列化器序列化")]
        public void TestClientConfigure_FormUrlEncodedSerializer_Serialize()
        {
            var mockObj = new SortedDictionary<string, object?>(new Dictionary<string, object?>()
            {
                { "integer", 12345 },
                { "string", "abcdef"  },
                { "boolean", true  },
                { "guid", Guid.Parse("12345678-ffff-ffff-ffff-123456789abc")  },
                { "array", new string[] { "你好", "世界" } },
                { "object", new { key = "value" } }
            }, StringComparer.Ordinal);

            const string expected = "array[0]=%E4%BD%A0%E5%A5%BD&array[1]=%E4%B8%96%E7%95%8C&boolean=true&guid=12345678-ffff-ffff-ffff-123456789abc&integer=12345&object.key=value&string=abcdef";
            StringAssert.AreEqualIgnoringCase(expected, new JsonifiedFormUrlEncodedSerializer(new NewtonsoftJsonSerializer()).Serialize(mockObj));
            StringAssert.AreEqualIgnoringCase(expected, new JsonifiedFormUrlEncodedSerializer(new SystemTextJsonSerializer()).Serialize(mockObj));



            var mockObjWithCustomConverters = new MockObject()
            {
                PropertyAsStringArray = new string[] { "你好", "世界" },
                PropertyAsDateTimeOffset = new DateTimeOffset(2006, 1, 2, 15, 4, 5, TimeSpan.FromHours(8))
            };

            const string expectedWithCustomConverters = "PropertyAsStringArray=%E4%BD%A0%E5%A5%BD%2C%E4%B8%96%E7%95%8C&PropertyAsDateTimeOffset=2006-01-02+15%3A04%3A05";
            StringAssert.AreEqualIgnoringCase(expectedWithCustomConverters, new JsonifiedFormUrlEncodedSerializer(new NewtonsoftJsonSerializer()).Serialize(mockObjWithCustomConverters));
            StringAssert.AreEqualIgnoringCase(expectedWithCustomConverters, new JsonifiedFormUrlEncodedSerializer(new SystemTextJsonSerializer()).Serialize(mockObjWithCustomConverters));
        }
    }
}
