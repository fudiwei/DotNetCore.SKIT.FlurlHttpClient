using System;

namespace SKIT.FlurlHttpClient.Internal
{
#pragma warning disable IDE1006
    public static class _UnsafeAccessor
#pragma warning restore IDE1006
    {
        public sealed class _UnsafeAccessorRequest<TRequest> where TRequest : CommonRequestBase
        {
            private readonly TRequest _request;

            public TimeSpan? Timeout
            {
                get { return _request._InternalTimeout; }
                set { _request._InternalTimeout = value; }
            }

            internal _UnsafeAccessorRequest(TRequest request)
            {
                _request = request ?? throw new ArgumentNullException(nameof(request));
            }
        }

        public sealed class _UnsafeAccessorResponse<TResponse> where TResponse : CommonResponseBase
        {
            private readonly TResponse _response;

            public int RawStatus
            {
                get { return _response._InternalRawStatus; }
                set { _response._InternalRawStatus = value; }
            }

            public HttpHeaderCollection RawHeaders
            {
                get { return _response._InternalRawHeaders; }
                set { _response._InternalRawHeaders = value ?? throw new ArgumentNullException(nameof(value)); }
            }

            public byte[] RawBytes
            {
                get { return _response._InternalRawBytes; }
                set { _response._InternalRawBytes = value ?? throw new ArgumentNullException(nameof(value)); }
            }

            internal _UnsafeAccessorResponse(TResponse request)
            {
                _response = request ?? throw new ArgumentNullException(nameof(request));
            }
        }

        public static _UnsafeAccessorRequest<TRequest> VisitCommonRequest<TRequest>(TRequest request) where TRequest : CommonRequestBase
        {
            return new _UnsafeAccessorRequest<TRequest>(request);
        }

        public static _UnsafeAccessorResponse<TResponse> VisitCommonResponse<TResponse>(TResponse response) where TResponse : CommonResponseBase
        {
            return new _UnsafeAccessorResponse<TResponse>(response);
        }
    }
}
