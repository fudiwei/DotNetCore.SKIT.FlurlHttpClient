using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests.MockSdk
{
    public static class MockClientExecuteFooExtensions
    {
        /// <summary>
        /// 异步调用 [GET] /foo/{id} 接口。
        /// </summary>
        /// <param name="client"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<Models.GetFooResponse<T>> ExecuteGetFooAsync<T>(this MockClient client, Models.GetFooRequest request, CancellationToken cancellationToken = default)
        {
            IFlurlRequest flurlReq = client.CreateFlurlRequest(request, HttpMethod.Get, "foo", request.Id);
            return await client.SendFlurlRequestAsync<Models.GetFooResponse<T>>(flurlReq, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// <para>异步调用 [POST] /foo 接口。</para>
        /// <para>REF: <![CDATA[https://github.com/fudiwei/SKIT.FlurlHttpClient]]></para>
        /// </summary>
        /// <param name="client"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<Models.PostFooResponse> ExecutePostFooAsync(this MockClient client, Models.PostFooRequest request, CancellationToken cancellationToken = default)
        {
            IFlurlRequest flurlReq = client.CreateFlurlRequest(request, new HttpMethod("POST"), "foo");
            return await client.SendFlurlRequestAsync<Models.PostFooResponse>(flurlReq, cancellationToken: cancellationToken);
        }
    }
}
