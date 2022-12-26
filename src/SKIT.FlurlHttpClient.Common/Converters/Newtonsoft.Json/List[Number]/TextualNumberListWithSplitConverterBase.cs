using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Newtonsoft.Json.Converters.Common
{
    using SKIT.FlurlHttpClient.Converters.Internal;

    public abstract partial class TextualNumberListWithSplitConverterBase : JsonConverter
    {
        protected abstract string Separator { get; }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override bool CanConvert(Type objectType)
        {
            if (!objectType.IsGenericType)
                return false;

            if (!typeof(IList<>).IsAssignableFrom(objectType.GetGenericTypeDefinition()))
                return false;

            Type elementType = objectType.GetGenericArguments()[0];
            return TypeHelper.IsNumberType(elementType);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            JsonConverter converter = new InternalTextualNumberListWithSplitConverter(objectType, Separator);
            return converter.ReadJson(reader, objectType, (IList?)existingValue, serializer);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is null)
            {
                writer.WriteNull();
            }
            else
            {
                JsonConverter converter = new InternalTextualNumberListWithSplitConverter(value.GetType(), Separator);
                converter.WriteJson(writer, (IList?)value, serializer);
            }
        }
    }

    partial class TextualNumberListWithSplitConverterBase
    {
        private sealed class InternalTextualNumberArrayWithSplitConverter : TextualNumberArrayWithSplitConverterBase
        {
            protected override string Separator { get; }

            public InternalTextualNumberArrayWithSplitConverter(string separator)
            {
                Separator = separator;
            }
        }

        private sealed class InternalTextualNumberListWithSplitConverter : JsonConverter
        {
            private readonly static IDictionary<Type, MethodInfo> _type2ToArrayMethodMap;
            private readonly static IDictionary<Type, MethodInfo> _type2ToListMethodMap;

            static InternalTextualNumberListWithSplitConverter()
            {
                _type2ToArrayMethodMap = new Dictionary<Type, MethodInfo>(capacity: TypeHelper.NumberTypes.Length + 1);
                _type2ToListMethodMap = new Dictionary<Type, MethodInfo>(capacity: TypeHelper.NumberTypes.Length + 1);

                MethodInfo toListBaseMethodInfo = typeof(Enumerable).GetMethod(nameof(Enumerable.ToList), BindingFlags.Public | BindingFlags.Static)!;
                MethodInfo toArrayBaseMethodInfo = typeof(Enumerable).GetMethod(nameof(Enumerable.ToArray), BindingFlags.Public | BindingFlags.Static)!;

                foreach (Type type in TypeHelper.NumberTypes)
                {
                    _type2ToArrayMethodMap[type] = toArrayBaseMethodInfo.MakeGenericMethod(type);
                    _type2ToListMethodMap[type] = toListBaseMethodInfo.MakeGenericMethod(type);
                }
            }

            private readonly Type _convertType;
            private readonly JsonConverter _converter;

            public InternalTextualNumberListWithSplitConverter(Type convertType, string separator)
            {
                _convertType = convertType;
                _converter = new InternalTextualNumberArrayWithSplitConverter(separator);
            }

            public override bool CanConvert(Type objectType)
            {
                // 实际是否可转换依赖外层转换器，此处不做判断，以提升效率
                return true;
            }

            public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
            {
                Type elementType = _convertType.GetGenericArguments()[0];
                Type arrayType = elementType.MakeArrayType();

                Array? array = (Array?)_converter.ReadJson(reader, arrayType, existingValue, serializer);
                if (array == null)
                    return null;

                return _type2ToListMethodMap[elementType].Invoke(null, new object?[] { array })!;
            }

            public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
            {
                if (value is null)
                {
                    writer.WriteNull();
                }
                else
                {
                    Type elementType = _convertType.GetGenericArguments()[0];

                    Array array = (Array)_type2ToArrayMethodMap[elementType].Invoke(null, new object?[] { value })!;

                    _converter.WriteJson(writer, array, serializer);
                }
            }
        }
    }
}
