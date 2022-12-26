using System;

namespace SKIT.FlurlHttpClient.Configuration
{
    public interface IJsonSerializer
    {
        public string Serialize(object? obj, Type type);

        public object? Deserialize(string json, Type type);
    }

    public static class IJsonSerializerExtensions
    {
        public static string Serialize<T>(this IJsonSerializer serializer, T obj)
        {
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            return serializer.Serialize(obj, obj?.GetType() ?? typeof(T))!;
        }

        public static T Deserialize<T>(this IJsonSerializer serializer, string json)
        {
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            return (T)serializer.Deserialize(json, typeof(T))!;
        }
    }
}
