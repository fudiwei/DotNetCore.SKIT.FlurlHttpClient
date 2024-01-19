using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace SKIT.FlurlHttpClient.Internal
{
    internal static class TypeHelper
    {
        private static readonly Type[] NumberTypes = new Type[]
        {
            typeof(sbyte),
            typeof(byte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(sbyte?),
            typeof(byte?),
            typeof(short?),
            typeof(ushort?),
            typeof(int?),
            typeof(uint?),
            typeof(long?),
            typeof(ulong?),
            typeof(float?),
            typeof(double?),
            typeof(decimal?)
        };

        private static readonly IDictionary<Type, Func<int, Array>> NumberType2ArrayConstructorExpressionMap;
        private static readonly IDictionary<Type, Func<Array, IList>> NumberType2ArrayLinqToListExpressionMap;
        private static readonly IDictionary<Type, Func<IList>> NumberType2ListConstructorExpressionMap;
        private static readonly IDictionary<Type, Func<IList, Array>> NumberType2ListLinqToArrayExpressionMap;

        static TypeHelper()
        {
            NumberType2ArrayConstructorExpressionMap = new Dictionary<Type, Func<int, Array>>(capacity: NumberTypes.Length + 1);
            NumberType2ArrayLinqToListExpressionMap = new Dictionary<Type, Func<Array, IList>>(capacity: NumberTypes.Length + 1);
            NumberType2ListConstructorExpressionMap = new Dictionary<Type, Func<IList>>(capacity: NumberTypes.Length + 1);
            NumberType2ListLinqToArrayExpressionMap = new Dictionary<Type, Func<IList, Array>>(capacity: NumberTypes.Length + 1);

            foreach (Type type in NumberTypes)
            {
                {
                    var initFunc = new Func<int, Array>((length) => Array.CreateInstance(type, length));
                    NumberType2ArrayConstructorExpressionMap[type] = initFunc;
                }

                {
                    var paramExpr = Expression.Parameter(typeof(Array), "source");
                    var unaryExpr = Expression.Convert(paramExpr, type.MakeArrayType());
                    var callExpr = Expression.Call(typeof(Enumerable), nameof(Enumerable.ToList), new[] { type }, unaryExpr);
                    NumberType2ArrayLinqToListExpressionMap[type] = Expression
                        .Lambda<Func<Array, IList>>(callExpr, paramExpr)
                        .Compile();
                }

                {
                    var initExpr = Expression.New(typeof(List<>).MakeGenericType(type));
                    NumberType2ListConstructorExpressionMap[type] = Expression
                        .Lambda<Func<IList>>(initExpr)
                        .Compile();
                }

                {
                    var paramExpr = Expression.Parameter(typeof(IList), "source");
                    var unaryExpr = Expression.Convert(paramExpr, typeof(List<>).MakeGenericType(type));
                    var callExpr = Expression.Call(typeof(Enumerable), nameof(Enumerable.ToArray), new[] { type }, unaryExpr);
                    NumberType2ListLinqToArrayExpressionMap[type] = Expression
                        .Lambda<Func<IList, Array>>(callExpr, paramExpr)
                        .Compile();
                }
            }

#if NET7_0_OR_GREATER
            NumberType2ArrayConstructorExpressionMap = NumberType2ArrayConstructorExpressionMap.AsReadOnly();
            NumberType2ArrayLinqToListExpressionMap = NumberType2ArrayLinqToListExpressionMap.AsReadOnly();
            NumberType2ListConstructorExpressionMap = NumberType2ListConstructorExpressionMap.AsReadOnly();
            NumberType2ListLinqToArrayExpressionMap = NumberType2ListLinqToArrayExpressionMap.AsReadOnly();
#else
            NumberType2ArrayConstructorExpressionMap = new ReadOnlyDictionary<Type, Func<int, Array>>(NumberType2ArrayConstructorExpressionMap);
            NumberType2ArrayLinqToListExpressionMap = new ReadOnlyDictionary<Type, Func<Array, IList>>(NumberType2ArrayLinqToListExpressionMap);
            NumberType2ListConstructorExpressionMap = new ReadOnlyDictionary<Type, Func<IList>>(NumberType2ListConstructorExpressionMap);
            NumberType2ListLinqToArrayExpressionMap = new ReadOnlyDictionary<Type, Func<IList, Array>>(NumberType2ListLinqToArrayExpressionMap);
#endif
        }

        /// <summary>
        /// 验证 <see cref="Type"/> 是否是基元类型中表示数值的类型。
        /// 完整的数值类型清单见 <see cref="NumberTypes"/>。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumberType(Type type)
        {
            return NumberTypes.Contains(type);
        }

        /// <summary>
        /// 将数组（即 <see cref="Array"/>）转换为列表（即 <see cref="IList{T}"/>）结构，数组中的元素是基元类型中表示数值的类型。
        /// </summary>
        /// <param name="array"></param>
        /// <param name="elementType"></param>
        /// <returns></returns>
        public static IList ConvertNumberArrayToList(Array array, Type elementType)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            if (elementType is null)
                throw new ArgumentNullException(nameof(elementType));
            if (!IsNumberType(elementType))
                throw new NotSupportedException();

            return NumberType2ArrayLinqToListExpressionMap[elementType](array);
        }

        /// <summary>
        /// 将列表（即 <see cref="IList{T}"/>）转换为数组（即 <see cref="Array"/>）结构，列表中的元素是基元类型中表示数值的类型。
        /// </summary>
        /// <param name="list"></param>
        /// <param name="elementType"></param>
        /// <returns></returns>
        public static Array ConvertNumberListToArray(IList list, Type elementType)
        {
            if (list is null)
                throw new ArgumentNullException(nameof(list));
            if (elementType is null)
                throw new ArgumentNullException(nameof(elementType));
            if (!IsNumberType(elementType))
                throw new NotSupportedException();

            return NumberType2ListLinqToArrayExpressionMap[elementType](list);
        }

        /// <summary>
        /// 创建一个数组（即 <see cref="Array"/>）结构，数组中的元素是基元类型中表示数值的类型。
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Array CreateNumberArray(Type elementType, int length = 0)
        {
            if (elementType is null)
                throw new ArgumentNullException(nameof(elementType));
            if (!IsNumberType(elementType))
                throw new NotSupportedException();

            return NumberType2ArrayConstructorExpressionMap[elementType](length);
        }

        /// <summary>
        /// 创建一个列表（即 <see cref="IList{T}"/>）结构，列表中的元素是基元类型中表示数值的类型。
        /// </summary>
        /// <param name="elementType"></param>
        /// <returns></returns>
        public static IList CreateNumberList(Type elementType)
        {
            if (elementType is null)
                throw new ArgumentNullException(nameof(elementType));
            if (!IsNumberType(elementType))
                throw new NotSupportedException();

            return NumberType2ListConstructorExpressionMap[elementType]();
        }
    }
}
