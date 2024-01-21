using System.Collections;
using System.Collections.Generic;

namespace System.Text.Json.Serialization.Common
{
    using SKIT.FlurlHttpClient.Internal;

    public abstract partial class StringifiedNumberListWithSplitConverterBase : JsonConverterFactory
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
            return new InternalStringifiedNumberListWithSplitConverter(typeToConvert, Separator);
        }
    }

    partial class StringifiedNumberListWithSplitConverterBase
    {
        private sealed class InternalStringifiedNumberArrayWithSplitConverter : StringifiedNumberArrayWithSplitConverterBase
        {
            protected override string Separator { get; }

            public InternalStringifiedNumberArrayWithSplitConverter(string separator)
            {
                Separator = separator;
            }
        }

        private sealed class InternalStringifiedNumberListWithSplitConverter : JsonConverter<object?>
        {
            private readonly Type _convertType;
            private readonly JsonConverterFactory _factory;

            public InternalStringifiedNumberListWithSplitConverter(Type convertType, string separator)
            {
                _convertType = convertType;
                _factory = new InternalStringifiedNumberArrayWithSplitConverter(separator);
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

                JsonConverter<object?> converter = (JsonConverter<object?>)_factory.CreateConverter(arrayType, options)!;
                Array? array = (Array?)converter.Read(ref reader, elementType.MakeArrayType(), options);
                if (array is null)
                    return null;

                return TypeHelper.ConvertNumberArrayToList(array, elementType);
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
                    Array array = TypeHelper.ConvertNumberListToArray((IList)value, elementType);

                    JsonConverter<object?> converter = (JsonConverter<object?>)_factory.CreateConverter(arrayType, options)!;
                    converter.Write(writer, array, options);
                }
            }
        }
    }
}
