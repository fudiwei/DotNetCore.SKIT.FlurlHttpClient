using System;
using Flurl.Http.Configuration;

namespace SKIT.FlurlHttpClient.Configuration
{
    public class DefaultFormUrlEncodedSerializer : IFormUrlEncodedSerializer
    {
        private readonly DefaultUrlEncodedSerializer _flurlUrlEncodedSerializer = new DefaultUrlEncodedSerializer();

        public virtual string Serialize(object? obj, Type type)
        {
            if (obj is null)
                return string.Empty;

            return _flurlUrlEncodedSerializer.Serialize(obj);
        }
    }
}
