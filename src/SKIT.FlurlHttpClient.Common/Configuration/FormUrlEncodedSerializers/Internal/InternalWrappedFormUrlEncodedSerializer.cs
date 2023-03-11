using System;
using System.IO;
using Flurl.Http.Configuration;

namespace SKIT.FlurlHttpClient.Configuration.Internal
{
    internal sealed class InternalWrappedFormUrlEncodedSerializer : ISerializer
    {
        internal IFormUrlEncodedSerializer Serializer { get; }

        public InternalWrappedFormUrlEncodedSerializer(IFormUrlEncodedSerializer jsonSerializer)
        {
            if (jsonSerializer == null) throw new ArgumentNullException(nameof(jsonSerializer));

            Serializer = jsonSerializer;
        }

        T ISerializer.Deserialize<T>(Stream stream)
        {
            throw new NotSupportedException();
        }

        T ISerializer.Deserialize<T>(string s)
        {
            throw new NotSupportedException();
        }

        string? ISerializer.Serialize(object obj)
        {
            if (obj == null)
                return null;

            return Serializer.Serialize(obj);
        }
    }
}
