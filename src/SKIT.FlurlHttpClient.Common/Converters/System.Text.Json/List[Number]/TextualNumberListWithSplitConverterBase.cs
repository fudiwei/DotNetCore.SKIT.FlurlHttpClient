using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace System.Text.Json.Converters.Common
{
    using SKIT.FlurlHttpClient.Converters.Internal;

    public abstract partial class TextualNumberListWithSplitConverterBase : JsonConverterFactory
    {
        protected abstract string Separator { get; }

        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType)
                return false;

            if (!typeof(IList<>).IsAssignableFrom(typeToConvert.GetGenericTypeDefinition()))
                return false;

            Type elementType = typeToConvert.GetGenericArguments()[0];
            return TypeHelper.IsNumberType(elementType);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new InternalTextualNumberListWithSplitConverter(typeToConvert, Separator);
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

        private sealed class InternalTextualNumberListWithSplitConverter : JsonConverter<object?>
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
            private readonly JsonConverterFactory _converterFactory;

            public InternalTextualNumberListWithSplitConverter(Type convertType, string separator)
            {
                _convertType = convertType;
                _converterFactory = new InternalTextualNumberArrayWithSplitConverter(separator);
            }

            public override bool CanConvert(Type typeToConvert)
            {
                // 实际是否可转换依赖外层转换器，此处不做判断，以提升效率
                return true;
            }

            public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                Type elementType = _convertType.GetGenericArguments()[0];
                Type arrayType = elementType.MakeArrayType();

                JsonConverter<object?> converter = (JsonConverter<object?>)_converterFactory.CreateConverter(arrayType, options)!;
                Array? array = (Array?)converter.Read(ref reader, elementType.MakeArrayType(), options);
                if (array == null)
                    return null;

                return _type2ToListMethodMap[elementType].Invoke(null, new object?[] { array })!;
            }

            public override void Write(Utf8JsonWriter writer, object? value, JsonSerializerOptions options)
            {
                if (value is null)
                {
                    writer.WriteNullValue();
                }
                else
                {
                    Type elementType = _convertType.GetGenericArguments()[0];
                    Type arrayType = elementType.MakeArrayType();

                    JsonConverter<object?> converter = (JsonConverter<object?>)_converterFactory.CreateConverter(arrayType, options)!;
                    Array array = (Array)_type2ToArrayMethodMap[elementType].Invoke(null, new object?[] { value })!;

                    converter.Write(writer, array, options);
                }
            }
        }
    }
}
