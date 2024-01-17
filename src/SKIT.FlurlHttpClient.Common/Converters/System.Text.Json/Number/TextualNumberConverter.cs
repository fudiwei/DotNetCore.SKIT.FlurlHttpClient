namespace System.Text.Json.Serialization.Common
{
    using SKIT.FlurlHttpClient.Internal;

    /// <summary>
    /// 一个 JSON 转换器，可针对指定适配类型做如下形式的对象转换。
    /// <para>与通过 System.Text.Json.Serialization.<see cref="JsonNumberHandling.AllowReadingFromString"/> 参数转换相比，可支持空字符串等特殊形式。</para>
    /// <code>
    ///   .NET → int Foo { get; } = 1;
    ///   JSON → { "Foo": "1" }
    /// </code>
    /// 
    /// 适配类型：
    /// <code>  <see cref="sbyte"/> <see cref="sbyte"/>?</code>
    /// <code>  <see cref="byte"/> <see cref="byte"/></code>
    /// <code>  <see cref="ushort"/> <see cref="ushort"/>?</code>
    /// <code>  <see cref="short"/> <see cref="short"/>?</code>
    /// <code>  <see cref="uint"/> <see cref="uint"/>?</code>
    /// <code>  <see cref="int"/> <see cref="int"/>?</code>
    /// <code>  <see cref="ulong"/> <see cref="ulong"/>?</code>
    /// <code>  <see cref="long"/> <see cref="long"/>?</code>
    /// <code>  <see cref="float"/> <see cref="float"/>?</code>
    /// <code>  <see cref="double"/> <see cref="double"/>?</code>
    /// <code>  <see cref="decimal"/> <see cref="decimal"/>?</code>
    /// </summary>
    public class TextualNumberConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return TypeHelper.IsNumberType(typeToConvert);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            Type convertType = Nullable.GetUnderlyingType(typeToConvert) ?? typeToConvert;
            bool convertTypeIsNullable = convertType != typeToConvert;

            switch (Type.GetTypeCode(convertType))
            {
                case TypeCode.SByte:
                    return convertTypeIsNullable ? new Internal.TextualNullableSByteConverter() : new Internal.TextualSByteConverter();

                case TypeCode.Byte:
                    return convertTypeIsNullable ? new Internal.TextualNullableByteConverter() : new Internal.TextualByteConverter();
                    
                case TypeCode.Int16:
                    return convertTypeIsNullable ? new Internal.TextualNullableInt16Converter() : new Internal.TextualInt16Converter();

                case TypeCode.UInt16:
                    return convertTypeIsNullable ? new Internal.TextualNullableUInt16Converter() : new Internal.TextualUInt16Converter();

                case TypeCode.Int32:
                    return convertTypeIsNullable ? new Internal.TextualNullableInt32Converter() : new Internal.TextualInt32Converter();

                case TypeCode.UInt32:
                    return convertTypeIsNullable ? new Internal.TextualNullableUInt32Converter() : new Internal.TextualUInt32Converter();

                case TypeCode.Int64:
                    return convertTypeIsNullable ? new Internal.TextualNullableInt64Converter() : new Internal.TextualInt64Converter();

                case TypeCode.UInt64:
                    return convertTypeIsNullable ? new Internal.TextualNullableUInt64Converter() : new Internal.TextualUInt64Converter();

                case TypeCode.Single:
                    return convertTypeIsNullable ? new Internal.TextualNullableFloatConverter() : new Internal.TextualFloatConverter();

                case TypeCode.Double:
                    return convertTypeIsNullable ? new Internal.TextualNullableDoubleConverter() : new Internal.TextualDoubleConverter();

                case TypeCode.Decimal:
                    return convertTypeIsNullable ? new Internal.TextualNullableDecimalConverter() : new Internal.TextualDecimalConverter();

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
