using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Converters.Common
{
    using SKIT.FlurlHttpClient.Internal;

    public abstract partial class StringifiedNumberListWithSplitConverterBase : JsonConverter
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
            JsonConverter converter = new InternalStringifiedNumberListWithSplitConverter(objectType, Separator);
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
                JsonConverter converter = new InternalStringifiedNumberListWithSplitConverter(value.GetType(), Separator);
                converter.WriteJson(writer, (IList?)value, serializer);
            }
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

        private sealed class InternalStringifiedNumberListWithSplitConverter : JsonConverter
        {
            private readonly Type _convertType;
            private readonly JsonConverter _converter;

            public InternalStringifiedNumberListWithSplitConverter(Type convertType, string separator)
            {
                _convertType = convertType;
                _converter = new InternalStringifiedNumberArrayWithSplitConverter(separator);
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
                if (array is null)
                    return null;

                return TypeHelper.ConvertNumberArrayToList(array, elementType);
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
                    Array array = TypeHelper.ConvertNumberListToArray((IList)value, elementType);

                    _converter.WriteJson(writer, array, serializer);
                }
            }
        }
    }
}
