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
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Boolean:
                        return true;

                    case TypeCode.Char:
                        return ' ';

                    case TypeCode.SByte:
                        return sbyte.MaxValue;

                    case TypeCode.Byte:
                        return byte.MaxValue;

                    case TypeCode.Int16:
                        return short.MaxValue;

                    case TypeCode.UInt16:
                        return ushort.MaxValue;

                    case TypeCode.Int32:
                        return int.MaxValue;

                    case TypeCode.UInt32:
                        return uint.MaxValue;

                    case TypeCode.Int64:
                        return long.MaxValue;

                    case TypeCode.UInt64:
                        return ulong.MaxValue;

                    case TypeCode.Single:
                        return float.MaxValue;

                    case TypeCode.Double:
                        return double.MaxValue;

                    default:
                        return Activator.CreateInstance(type)!;
                }
            }

            /* Decimal 类型 */
            if (type == typeof(decimal))
            {
                return 60118.1332M;
            }

            /* String 类型 */
            if (type == typeof(string))
            {
                return "SKIT.FlurlHttpClient";
            }

            /* DateTimeOffset 类型 */
            if (type == typeof(DateTimeOffset))
            {
                return DateTimeOffset.Parse("2006/01/02 15:04:05");
            }

            /* Array<T> 类型 */
            if (type.IsArray)
            {
                Array array = Array.CreateInstance(type.GetElementType()!, 1);
                object element = InnerCreateInitializedInstance(type.GetElementType()!, depth);
                array.SetValue(element, 0);
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
                    dict = (IDictionary)Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(genericKType, genericVType))!;
                }
                else
                {
                    try
                    {
                        dict = (IDictionary)Activator.CreateInstance(type)!;
                    }
                    catch (MissingMethodException)
                    {
                        return default!;
                    }
                }

                if (!dict.IsReadOnly)
                {
                    object k = InnerCreateInitializedInstance(genericKType, depth);
                    object v = InnerCreateInitializedInstance(genericVType, depth);
                    if (!(depth == MAX_DEPTH && k is null))
                        dict.Add(k, v);
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
                    list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(genericType))!;
                }
                else
                {
                    try
                    {
                        list = (IList)Activator.CreateInstance(type)!;
                    }
                    catch (MissingMethodException)
                    {
                        return default!;
                    }
                }

                if (!list.IsReadOnly)
                {
                    var element = InnerCreateInitializedInstance(genericType, depth);
                    if (!(depth == MAX_DEPTH && element is null))
                        list.Add(element);
                }

                return list;
            }

            /* Object */
            try
            {
                object instance = Activator.CreateInstance(type)!;

                PropertyInfo[] properties = instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo property in properties)
                {
                    if (property.SetMethod is null || !property.SetMethod.IsPublic)
                        continue;

                    property.SetValue(instance, InnerCreateInitializedInstance(property.PropertyType, depth));
                }

                return instance;
            }
            catch (MissingMethodException)
            {
                return default!;
            }
        }
    }
}
