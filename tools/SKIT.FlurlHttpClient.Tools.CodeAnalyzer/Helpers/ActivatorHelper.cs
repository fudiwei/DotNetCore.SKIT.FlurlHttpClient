using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer.Helpers
{
    internal static class ActivatorHelper
    {
        public static T CreateInitializedInstance<T>()
        {
            return (T)CreateInitializedInstance(typeof(T));
        }

        public static object CreateInitializedInstance(Type type)
        {
            return InnerCreateInitializedInstance(type, 0);
        }

        private static object InnerCreateInitializedInstance(Type type, int depth)
        {
            const int MAX_DEPTH = 8;

            depth++;
            if (depth > MAX_DEPTH)
                return default!;

            /* 基元类型 */
            if (type.IsPrimitive)
            {
                return Activator.CreateInstance(type);
            }

            /* 字符串类型 */
            if (type == typeof(string))
            {
                return string.Empty;
            }

            /* Array<T> 类型 */
            if (type.IsArray)
            {
                Type elementType = type.GetElementType();
                Array array = Array.CreateInstance(elementType, 1);
                array.SetValue(InnerCreateInitializedInstance(elementType, depth), 0);
                return array;
            }

            /* Nullable<T> 类型 */
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                Type genericType = type.GetGenericArguments()[0];
                return InnerCreateInitializedInstance(genericType, depth);
            }

            /* IDictionary<K, V> 类型 */
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>))
            {
                Type genericKType = type.GetGenericArguments()[0];
                Type genericVType = type.GetGenericArguments()[1];

                IDictionary dict;
                if (type.IsInterface || type.IsAbstract)
                {
                    dict = (IDictionary)Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(genericKType, genericVType));
                }
                else
                {
                    try
                    {
                        dict = (IDictionary)Activator.CreateInstance(type);
                    }
                    catch (MissingMethodException)
                    {
                        return default!;
                    }
                }

                if (!dict.IsReadOnly)
                {
                    dict.Add(InnerCreateInitializedInstance(genericKType, depth), InnerCreateInitializedInstance(genericVType, depth));
                }

                return dict;
            }

            /* IList<T> 类型 */
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>))
            {
                Type genericType = type.GetGenericArguments()[0];

                IList list;
                if (type.IsInterface || type.IsAbstract)
                {
                    list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(genericType));
                }
                else
                {
                    try
                    {
                        list = (IList)Activator.CreateInstance(type);
                    }
                    catch (MissingMethodException)
                    {
                        return default!;
                    }
                }

                if (!list.IsReadOnly)
                {
                    list.Add(InnerCreateInitializedInstance(genericType, depth));
                }

                return list;
            }

            /* object */
            try
            {
                object obj = Activator.CreateInstance(type);

                PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo property in properties)
                {
                    if (property.SetMethod == null || !property.SetMethod.IsPublic)
                        continue;

                    property.SetValue(obj, InnerCreateInitializedInstance(property.PropertyType, depth));
                }

                return obj;
            }
            catch (MissingMethodException)
            {
                return default!;
            }
        }
    }
}
