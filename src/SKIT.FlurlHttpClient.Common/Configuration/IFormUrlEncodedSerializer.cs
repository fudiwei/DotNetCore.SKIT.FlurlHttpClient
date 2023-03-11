using System;

namespace SKIT.FlurlHttpClient.Configuration
{
    public interface IFormUrlEncodedSerializer
    {
        public string Serialize(object? obj, Type type);
    }

    public static class IFormUrlEncodedSerializerExtensions
    {
        public static string Serialize<T>(this IFormUrlEncodedSerializer serializer, T obj)
        {
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            return serializer.Serialize(obj, obj?.GetType() ?? typeof(T))!;
        }
    }
}
