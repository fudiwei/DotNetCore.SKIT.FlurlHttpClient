using System;
using Flurl.Http.Configuration;

namespace SKIT.FlurlHttpClient.Configuration
{
    /// <summary>
    /// <para>用于序列化 "application/x-www-form-urlencoded" 内容的序列化器。</para>
    /// <para>基于 Flurl.Http.Configuration.<see cref="DefaultUrlEncodedSerializer"/> 实现。</para>
    /// </summary>
    public class FlurlFormUrlEncodedSerializer : IFormUrlEncodedSerializer
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
