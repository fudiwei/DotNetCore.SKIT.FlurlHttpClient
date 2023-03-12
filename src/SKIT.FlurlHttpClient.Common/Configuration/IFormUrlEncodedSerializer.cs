using System;

namespace SKIT.FlurlHttpClient.Configuration
{
    /// <summary>
    /// 用于序列化 "application/x-www-form-urlencoded" 内容的序列化器。
    /// </summary>
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
