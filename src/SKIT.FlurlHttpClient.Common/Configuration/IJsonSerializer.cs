using System;

namespace SKIT.FlurlHttpClient
{
    /// <summary>
    /// 用于序列化 "application/json" 内容的序列化器。
    /// </summary>
    public interface IJsonSerializer
    {
        /// <summary>
        /// 将指定类型的实例序列化为表示 JSON 的字符串。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string Serialize(object? obj, Type type);

        /// <summary>
        /// 将表示 JSON 的字符串反序列化为指定类型的实例。
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public object? Deserialize(string json, Type type);
    }

    public static class IJsonSerializerExtensions
    {
        /// <summary>
        /// 将指定类型的实例序列化为表示 JSON 的字符串。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize<T>(this IJsonSerializer serializer, T obj)
        {
            if (serializer is null) throw new ArgumentNullException(nameof(serializer));

            return serializer.Serialize(obj, obj?.GetType() ?? typeof(T))!;
        }

        /// <summary>
        /// 将表示 JSON 的字符串反序列化为指定类型的实例。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(this IJsonSerializer serializer, string json)
        {
            if (serializer is null) throw new ArgumentNullException(nameof(serializer));

            return (T)serializer.Deserialize(json, typeof(T))!;
        }
    }
}
