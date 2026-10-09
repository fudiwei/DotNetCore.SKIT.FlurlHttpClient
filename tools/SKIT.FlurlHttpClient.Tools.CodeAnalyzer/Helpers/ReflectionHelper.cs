using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer.Helpers
{
    public static class ReflectionHelper
    {
        /// <summary>
        /// 获取指定程序集下的可导出类型。
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetExportedTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetExportedTypes();
            }
            catch (ReflectionTypeLoadException exception)
            {
                return exception.Types.Where(type => type is not null).Cast<Type>();
            }
        }

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
        /// 获取指定类型的不包含泛型信息的完全名称。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetFullNameWithoutGenerics(this Type type)
        {
            if (type.FullName is null)
                return type.Namespace is null ? GetNameWithoutGenerics(type) : $"{type.Namespace}.{GetNameWithoutGenerics(type)}";

            if (type.IsGenericType)
                return type.FullName!.Remove(type.FullName!.IndexOf('`'));

            return type.FullName!;
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
