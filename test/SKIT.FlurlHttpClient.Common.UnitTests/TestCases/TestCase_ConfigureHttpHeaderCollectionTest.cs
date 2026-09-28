namespace SKIT.FlurlHttpClient.UnitTests.TestCases
{
    public class TestCase_ConfigureHttpHeaderCollectionTest
    {
        [Fact(DisplayName = "测试用例：HTTP 标头集合读取与枚举")]
        public async Task TestHttpHeaderCollection_ReadAndEnumerate()
        {
            using var client = new MockTestClient();
            IFlurlRequest request = client.CreateFlurlRequest(new MockTestRequest(), HttpMethod.Get, "mock_url");
            MockTestResponse response = await client.SendFlurlRequestAsJsonAsync<MockTestResponse>(request);
            HttpHeaderCollection headers = response.GetRawHeaders();

            Assert.True(headers.ContainsKey("x-test-multi"));
            Assert.True(headers.TryGetValue("X-TEST-MULTI", out IEnumerable<string>? values));
            Assert.Equal(new[] { "value-1", "value-2" }, values);
            Assert.True(headers.TryGetMergedValue("x-test-multi", out string? mergedValue));
            Assert.Equal("value-1, value-2", mergedValue);
            Assert.True(headers.TryGetFirstValue("X-Test-Multi", out string? firstValue));
            Assert.Equal("value-1", firstValue);
            Assert.Equal("value-1, value-2", headers.GetMergedValueOrEmpty("x-test-multi"));
            Assert.Equal("value-1", headers.GetFirstValueOrEmpty("x-test-multi"));

            Assert.False(headers.TryGetValue("X-Missing", out _));
            Assert.False(headers.TryGetMergedValue("X-Missing", out string? missingMergedValue));
            Assert.Null(missingMergedValue);
            Assert.False(headers.TryGetFirstValue("X-Missing", out string? missingFirstValue));
            Assert.Null(missingFirstValue);
            Assert.Empty(headers.GetValueOrEmpty("X-Missing"));
            Assert.Equal(string.Empty, headers.GetMergedValueOrEmpty("X-Missing"));
            Assert.Equal(string.Empty, headers.GetFirstValueOrEmpty("X-Missing"));

            Assert.Contains("x-test-multi", headers.GetAllKeys());
            Assert.Contains(headers, entry => entry.Key == "x-test-multi");
            Assert.Contains("x-test-multi: value-1, value-2\r\n", headers.ToString());

            IDictionary<string, IEnumerable<string>> dictionary = headers;
            Assert.True(((ICollection<KeyValuePair<string, IEnumerable<string>>>)dictionary).IsReadOnly);
            Assert.Throws<NotSupportedException>(() => dictionary.Add("X-New", new[] { "value" }));
            Assert.Throws<NotSupportedException>(() => dictionary.Remove("X-Test-Multi"));
            Assert.Throws<NotSupportedException>(() => dictionary["X-Test-Multi"] = new[] { "changed" });
            Assert.Throws<NotSupportedException>(() => ((ICollection<KeyValuePair<string, IEnumerable<string>>>)dictionary).Clear());
        }

        [Fact(DisplayName = "测试用例：HTTP 标头空集合")]
        public void TestHttpHeaderCollection_Empty()
        {
            HttpHeaderCollection headers = HttpHeaderCollection.Empty;

            Assert.Empty(headers);
            Assert.Empty(headers.GetAllKeys());

            Assert.False(headers.ContainsKey(HttpHeaders.ContentType));
            Assert.Null(headers.ContentType);

            Assert.False(headers.TryGetMergedValue(HttpHeaders.ContentType, out string? mergedValue));
            Assert.Null(mergedValue);

            Assert.False(headers.TryGetFirstValue(HttpHeaders.ContentType, out string? firstValue));
            Assert.Null(firstValue);

            Assert.Equal(string.Empty, headers.ToString());
        }
    }
}
