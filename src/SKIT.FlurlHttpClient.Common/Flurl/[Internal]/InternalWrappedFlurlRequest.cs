using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using Flurl.Util;

namespace SKIT.FlurlHttpClient
{
    internal sealed class InternalWrappedFlurlRequest : IFlurlRequest
    {
        private readonly IFlurlRequest _flurlRequest;
        private readonly ICommonRequest _commonRequest;

        public InternalWrappedFlurlRequest(IFlurlRequest flurlRequest, ICommonRequest commonRequest)
        {
            if (flurlRequest is InternalWrappedFlurlRequest wrappedFlurlRequest)
            {
                flurlRequest = wrappedFlurlRequest._flurlRequest;
            }

            _flurlRequest = flurlRequest ?? throw new ArgumentNullException(nameof(flurlRequest));
            _commonRequest = commonRequest ?? throw new ArgumentNullException(nameof(commonRequest));
        }

        public IFlurlClient Client
        {
            get { return _flurlRequest.Client; }
            set { _flurlRequest.Client = value; }
        }

        public ICommonRequest OriginalRequest
        {
            get { return _commonRequest; }
        }

        public HttpMethod Verb
        {
            get { return _flurlRequest.Verb; }
            set { _flurlRequest.Verb = value; }
        }

        public Url Url
        {
            get { return _flurlRequest.Url; }
            set { _flurlRequest.Url = value; }
        }

        public INameValueList<string> Headers
        {
            get { return _flurlRequest.Headers; }
        }

        public HttpContent Content
        {
            get { return _flurlRequest.Content; }
            set { _flurlRequest.Content = value; }
        }

        public IEnumerable<(string Name, string Value)> Cookies
        {
            get { return _flurlRequest.Cookies; }
        }

        public CookieJar CookieJar
        {
            get { return _flurlRequest.CookieJar; }
            set { _flurlRequest.CookieJar = value; }
        }

        public FlurlCall RedirectedFrom
        {
            get { return _flurlRequest.RedirectedFrom; }
            set { _flurlRequest.RedirectedFrom = value; }
        }

        public FlurlHttpSettings Settings
        {
            get { return _flurlRequest.Settings; }
        }

        public IList<(FlurlEventType EventType, IFlurlEventHandler Handler)> EventHandlers
        {
            get { return _flurlRequest.EventHandlers; }
        }

        public IFlurlClient EnsureClient()
        {
            return _flurlRequest.EnsureClient();
        }

        public Task<IFlurlResponse> SendAsync(HttpMethod verb, HttpContent? content = null, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead, CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException("This method should not be called directly. Please use the 'SendAsync' method of the client instead.");
        }
    }
}
