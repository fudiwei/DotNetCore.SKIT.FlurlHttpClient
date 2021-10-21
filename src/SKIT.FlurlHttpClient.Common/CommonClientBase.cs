﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace SKIT.FlurlHttpClient
{
    /// <summary>
    /// SKIT.FlurlHttpClient 客户端基类。
    /// </summary>
    public abstract class CommonClientBase : ICommonClient
    {
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public FlurlHttpCallInterceptorCollection Interceptors { get; }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public ISerializer JsonSerializer
        {
            get { return FlurlClient.Settings?.JsonSerializer ?? FlurlHttp.GlobalSettings.JsonSerializer; }
        }

        /// <summary>
        /// 获取当前客户端使用的 <see cref="IFlurlClient"/> 对象。
        /// </summary>
        protected IFlurlClient FlurlClient { get; }

        /// <summary>
        /// 
        /// </summary>
        protected CommonClientBase()
        {
            Interceptors = new FlurlHttpCallInterceptorCollection();
            FlurlClient = new FlurlClient();
            FlurlClient.Configure(flurlSettings =>
            {
                flurlSettings.JsonSerializer = new FlurlSystemTextJsonSerializer();
                flurlSettings.BeforeCallAsync = async (flurlCall) =>
                {
                    for (int i = 0, len = Interceptors.Count; i < len; i++)
                    {
                        await Interceptors[i].BeforeCallAsync(flurlCall);
                    }
                };
                flurlSettings.AfterCallAsync = async (flurlCall) =>
                {
                    for (int i = Interceptors.Count - 1; i >= 0; i--)
                    {
                        await Interceptors[i].AfterCallAsync(flurlCall);
                    }
                };
            });
        }

        /// <inheritdoc/>
        public void Configure(Action<CommonClientSettings> configure)
        {
            if (configure == null) throw new ArgumentNullException(nameof(configure));

            FlurlClient.Configure(flurlSettings =>
            {
                var settings = new CommonClientSettings();
                settings.ConnectionRequestTimeout = flurlSettings.Timeout;
                settings.ConnectionLeaseTimeout = flurlSettings.ConnectionLeaseTimeout;
                settings.JsonSerializer = flurlSettings.JsonSerializer;
                settings.UrlEncodedSerializer = flurlSettings.UrlEncodedSerializer;
                settings.FlurlHttpClientFactory = flurlSettings.HttpClientFactory;
                configure.Invoke(settings);

                flurlSettings.Timeout = settings.ConnectionRequestTimeout;
                flurlSettings.ConnectionLeaseTimeout = settings.ConnectionLeaseTimeout;
                flurlSettings.JsonSerializer = settings.JsonSerializer;
                flurlSettings.UrlEncodedSerializer = settings.UrlEncodedSerializer;
                flurlSettings.HttpClientFactory = settings.FlurlHttpClientFactory;
            });
        }

        private IFlurlRequest WrapRequest(IFlurlRequest flurlRequest)
        {
            return flurlRequest
                .WithClient(FlurlClient)
                .AllowAnyHttpStatus();
        }

        /// <summary>
        /// 异步发起请求。
        /// </summary>
        /// <param name="flurlRequest"></param>
        /// <param name="httpContent"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected virtual async Task<IFlurlResponse> SendRequestAsync(IFlurlRequest flurlRequest, HttpContent? httpContent = null, CancellationToken cancellationToken = default)
        {
            if (flurlRequest == null) throw new ArgumentNullException(nameof(flurlRequest));

            return await WrapRequest(flurlRequest).SendAsync(flurlRequest.Verb, httpContent, cancellationToken);
        }

        /// <summary>
        /// 异步发起请求。
        /// </summary>
        /// <param name="flurlRequest"></param>
        /// <param name="data"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected virtual async Task<IFlurlResponse> SendRequestWithJsonAsync(IFlurlRequest flurlRequest, object? data = null, CancellationToken cancellationToken = default)
        {
            if (flurlRequest == null) throw new ArgumentNullException(nameof(flurlRequest));

            if (flurlRequest.Verb == HttpMethod.Get ||
                flurlRequest.Verb == HttpMethod.Head ||
                flurlRequest.Verb == HttpMethod.Options)
            {
                return await WrapRequest(flurlRequest).SendAsync(flurlRequest.Verb, cancellationToken: cancellationToken);
            }

            return await WrapRequest(flurlRequest).SendJsonAsync(flurlRequest.Verb, data: data, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public virtual void Dispose()
        {
            FlurlClient?.Dispose();
        }
    }
}
