using System;

namespace SKIT.FlurlHttpClient
{
    /// <summary>
    /// 用于序列化 "application/x-www-form-urlencoded" 内容的序列化器。
    /// </summary>
    public interface IFormUrlEncodedSerializer
    {
        /// <summary>
        /// 将指定类型的实例序列化为表示 HTML 表单 URL 编码的字符串。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string Serialize(object? obj, Type type);
    }

    public static class IFormUrlEncodedSerializerExtensions
    {
        /// <summary>
        /// 将指定类型的实例序列化为表示 HTML 表单 URL 编码的字符串。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize<T>(this IFormUrlEncodedSerializer serializer, T obj)
        {
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            return serializer.Serialize(obj, obj?.GetType() ?? typeof(T))!;
        }
    }
}
