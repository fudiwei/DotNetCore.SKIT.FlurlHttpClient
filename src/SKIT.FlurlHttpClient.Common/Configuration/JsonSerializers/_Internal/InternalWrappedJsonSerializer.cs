using System;
using System.IO;
using Flurl.Http.Configuration;

namespace SKIT.FlurlHttpClient
{
    internal sealed class InternalWrappedJsonSerializer : ISerializer
    {
        internal IJsonSerializer Serializer { get; }

        public InternalWrappedJsonSerializer(IJsonSerializer jsonSerializer)
        {
            Serializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
        }

        T ISerializer.Deserialize<T>(Stream stream)
        {
            if (stream is null) throw new ArgumentNullException(nameof(stream));

            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            using TextReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();
            return Serializer.Deserialize<T>(json);
        }

        T ISerializer.Deserialize<T>(string s)
        {
            return Serializer.Deserialize<T>(s);
        }

        string? ISerializer.Serialize(object obj)
        {
            if (obj is null)
                return null;

            return Serializer.Serialize(obj);
        }
    }
}
