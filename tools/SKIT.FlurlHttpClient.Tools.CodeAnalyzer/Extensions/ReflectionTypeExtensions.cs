using System;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    public static class ReflectionTypeExtensions
    {
        /// <summary>
        /// 获取指定类型的不包含泛型信息的名称。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetNameWithoutGenerics(this Type type)
        {
            if (type.IsGenericType)
            {
                return type.Name.Remove(type.Name.IndexOf('`'));
            }

            return type.Name;
        }

        /// <summary>
        /// 判断指定类型是否表示一个静态类。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsStaticClass(this Type type)
        {
            return type.IsClass && type.IsAbstract && type.IsSealed;
        }

        /// <summary>
        /// 判断指定类型是否表示一个非抽象类。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNonAbstractClass(this Type type)
        {
            return type.IsClass && !type.IsAbstract && !type.IsInterface;
        }

        /// <summary>
        /// 判断指定类型是否表示一个非内嵌类。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNonNestedClass(this Type type)
        {
            return type.IsClass && !type.IsNested;
        }
    }
}
