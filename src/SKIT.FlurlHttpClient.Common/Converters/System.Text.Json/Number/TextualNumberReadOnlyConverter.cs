namespace System.Text.Json.Serialization.Common
{
    using SKIT.FlurlHttpClient.Converters.Internal;

    /// <summary>
    /// 一个 JSON 转换器，可针对指定适配类型做如下形式的对象转换。与 <seealso cref="TextualNumberConverter"/> 类似，但转换过程是单向只读的。
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
    public partial class TextualNumberReadOnlyConverter : JsonConverterFactory
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
                    return convertTypeIsNullable ? new InternalTextualNullableSByteReadOnlyConverter() : new InternalTextualSByteReadOnlyConverter();

                case TypeCode.Byte:
                    return convertTypeIsNullable ? new InternalTextualNullableByteReadOnlyConverter() : new InternalTextualByteReadOnlyConverter();

                case TypeCode.Int16:
                    return convertTypeIsNullable ? new InternalTextualNullableInt16ReadOnlyConverter() : new InternalTextualInt16ReadOnlyConverter();

                case TypeCode.UInt16:
                    return convertTypeIsNullable ? new InternalTextualNullableUInt16ReadOnlyConverter() : new InternalTextualUInt16ReadOnlyConverter();

                case TypeCode.Int32:
                    return convertTypeIsNullable ? new InternalTextualNullableInt32ReadOnlyConverter() : new InternalTextualInt32ReadOnlyConverter();

                case TypeCode.UInt32:
                    return convertTypeIsNullable ? new InternalTextualNullableUInt32ReadOnlyConverter() : new InternalTextualUInt32ReadOnlyConverter();

                case TypeCode.Int64:
                    return convertTypeIsNullable ? new InternalTextualNullableInt64ReadOnlyConverter() : new InternalTextualInt64ReadOnlyConverter();

                case TypeCode.UInt64:
                    return convertTypeIsNullable ? new InternalTextualNullableUInt64ReadOnlyConverter() : new InternalTextualUInt64ReadOnlyConverter();

                case TypeCode.Single:
                    return convertTypeIsNullable ? new InternalTextualNullableFloatReadOnlyConverter() : new InternalTextualFloatReadOnlyConverter();

                case TypeCode.Double:
                    return convertTypeIsNullable ? new InternalTextualNullableDoubleReadOnlyConverter() : new InternalTextualDoubleReadOnlyConverter();

                case TypeCode.Decimal:
                    return convertTypeIsNullable ? new InternalTextualNullableDecimalReadOnlyConverter() : new InternalTextualDecimalReadOnlyConverter();

                default:
                    throw new NotSupportedException();
            }
        }
    }

    partial class TextualNumberReadOnlyConverter
    {
        #region SByte
        private sealed class InternalTextualNullableSByteReadOnlyConverter : JsonConverter<sbyte?>
        {
            private readonly JsonConverter<sbyte?> _converter = new Internal.TextualNullableSByteConverter();
            private readonly JsonConverter<sbyte?> _fallback = (JsonConverter<sbyte?>)JsonSerializerOptions.Default.GetConverter(typeof(sbyte?));

            public override sbyte? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, sbyte? value, JsonSerializerOptions options)
            {
                _fallback.Write(writer, value, options);
            }

            public override sbyte? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, sbyte? value, JsonSerializerOptions options)
            {
                _fallback.WriteAsPropertyName(writer, value!, options);
            }
        }

        private sealed class InternalTextualSByteReadOnlyConverter : JsonConverter<sbyte>
        {
            private readonly JsonConverter<sbyte> _converter = new Internal.TextualSByteConverter();
            private readonly JsonConverter<sbyte> _fallback = (JsonConverter<sbyte>)JsonSerializerOptions.Default.GetConverter(typeof(sbyte));

            public override sbyte Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, sbyte value, JsonSerializerOptions options)
            {
                _fallback.Write(writer, value, options);
            }

            public override sbyte ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, sbyte value, JsonSerializerOptions options)
            {
                _fallback.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region Byte
        private sealed class InternalTextualNullableByteReadOnlyConverter : JsonConverter<byte?>
        {
            private readonly JsonConverter<byte?> _converter = new Internal.TextualNullableByteConverter();
            private readonly JsonConverter<byte?> _fallback = (JsonConverter<byte?>)JsonSerializerOptions.Default.GetConverter(typeof(byte?));

            public override byte? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, byte? value, JsonSerializerOptions options)
            {
                _fallback.Write(writer, value, options);
            }

            public override byte? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, byte? value, JsonSerializerOptions options)
            {
                _fallback.WriteAsPropertyName(writer, value!, options);
            }
        }

        private sealed class InternalTextualByteReadOnlyConverter : JsonConverter<byte>
        {
            private readonly JsonConverter<byte> _converter = new Internal.TextualByteConverter();
            private readonly JsonConverter<byte> _fallback = (JsonConverter<byte>)JsonSerializerOptions.Default.GetConverter(typeof(byte));

            public override byte Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, byte value, JsonSerializerOptions options)
            {
                _fallback.Write(writer, value, options);
            }

            public override byte ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, byte value, JsonSerializerOptions options)
            {
                _fallback.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region Int16
        private sealed class InternalTextualNullableInt16ReadOnlyConverter : JsonConverter<short?>
        {
            private readonly JsonConverter<short?> _converter = new Internal.TextualNullableInt16Converter();
            private readonly JsonConverter<short?> _fallback = (JsonConverter<short?>)JsonSerializerOptions.Default.GetConverter(typeof(short?));

            public override short? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, short? value, JsonSerializerOptions options)
            {
                _fallback.Write(writer, value, options);
            }

            public override short? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, short? value, JsonSerializerOptions options)
            {
                _fallback.WriteAsPropertyName(writer, value!, options);
            }
        }

        private sealed class InternalTextualInt16ReadOnlyConverter : JsonConverter<short>
        {
            private readonly JsonConverter<short> _converter = new Internal.TextualInt16Converter();
            private readonly JsonConverter<short> _fallback = (JsonConverter<short>)JsonSerializerOptions.Default.GetConverter(typeof(short));

            public override short Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, short value, JsonSerializerOptions options)
            {
                _fallback.Write(writer, value, options);
            }

            public override short ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, short value, JsonSerializerOptions options)
            {
                _fallback.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region UInt16
        private sealed class InternalTextualNullableUInt16ReadOnlyConverter : JsonConverter<ushort?>
        {
            private readonly JsonConverter<ushort?> _converter = new Internal.TextualNullableUInt16Converter();
            private readonly JsonConverter<ushort?> _fallback = (JsonConverter<ushort?>)JsonSerializerOptions.Default.GetConverter(typeof(ushort?));

            public override ushort? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, ushort? value, JsonSerializerOptions options)
            {
                _fallback.Write(writer, value, options);
            }

            public override ushort? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, ushort? value, JsonSerializerOptions options)
            {
                _fallback.WriteAsPropertyName(writer, value!, options);
            }
        }

        private sealed class InternalTextualUInt16ReadOnlyConverter : JsonConverter<ushort>
        {
            private readonly JsonConverter<ushort> _converter = new Internal.TextualUInt16Converter();
            private readonly JsonConverter<ushort> _fallback = (JsonConverter<ushort>)JsonSerializerOptions.Default.GetConverter(typeof(ushort));

            public override ushort Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, ushort value, JsonSerializerOptions options)
            {
                _fallback.Write(writer, value, options);
            }

            public override ushort ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, ushort value, JsonSerializerOptions options)
            {
                _fallback.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region Int32
        private sealed class InternalTextualNullableInt32ReadOnlyConverter : JsonConverter<int?>
        {
            private readonly JsonConverter<int?> _converter = new Internal.TextualNullableInt32Converter();
            private readonly JsonConverter<int?> _fallback = (JsonConverter<int?>)JsonSerializerOptions.Default.GetConverter(typeof(int?));

            public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
            {
                _fallback.Write(writer, value, options);
            }

            public override int? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
            {
                _fallback.WriteAsPropertyName(writer, value!, options);
            }
        }

        private sealed class InternalTextualInt32ReadOnlyConverter : JsonConverter<int>
        {
            private readonly JsonConverter<int> _converter = new Internal.TextualInt32Converter();
            private readonly JsonConverter<int> _fallback = (JsonConverter<int>)JsonSerializerOptions.Default.GetConverter(typeof(int));

            public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
            {
                _fallback.Write(writer, value, options);
            }

            public override int ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
            {
                _fallback.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region UInt32
        private sealed class InternalTextualNullableUInt32ReadOnlyConverter : JsonConverter<uint?>
        {
            private readonly JsonConverter<uint?> _converter = new Internal.TextualNullableUInt32Converter();
            private readonly JsonConverter<uint?> _fallback = (JsonConverter<uint?>)JsonSerializerOptions.Default.GetConverter(typeof(uint?));

            public override uint? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, uint? value, JsonSerializerOptions options)
            {
                _fallback.Write(writer, value, options);
            }

            public override uint? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, uint? value, JsonSerializerOptions options)
            {
                _fallback.WriteAsPropertyName(writer, value!, options);
            }
        }

        private sealed class InternalTextualUInt32ReadOnlyConverter : JsonConverter<uint>
        {
            private readonly JsonConverter<uint> _converter = new Internal.TextualUInt32Converter();
            private readonly JsonConverter<uint> _fallback = (JsonConverter<uint>)JsonSerializerOptions.Default.GetConverter(typeof(uint));

            public override uint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, uint value, JsonSerializerOptions options)
            {
                _fallback.Write(writer, value, options);
            }

            public override uint ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, uint value, JsonSerializerOptions options)
            {
                _fallback.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region Int64
        private sealed class InternalTextualNullableInt64ReadOnlyConverter : JsonConverter<long?>
        {
            private readonly JsonConverter<long?> _converter = new Internal.TextualNullableInt64Converter();
            private readonly JsonConverter<long?> _fallback = (JsonConverter<long?>)JsonSerializerOptions.Default.GetConverter(typeof(long?));

            public override long? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, long? value, JsonSerializerOptions options)
            {
                _fallback.Write(writer, value, options);
            }

            public override long? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, long? value, JsonSerializerOptions options)
            {
                _fallback.WriteAsPropertyName(writer, value!, options);
            }
        }

        private sealed class InternalTextualInt64ReadOnlyConverter : JsonConverter<long>
        {
            private readonly JsonConverter<long> _converter = new Internal.TextualInt64Converter();
            private readonly JsonConverter<long> _fallback = (JsonConverter<long>)JsonSerializerOptions.Default.GetConverter(typeof(long));

            public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
            {
                _fallback.Write(writer, value, options);
            }

            public override long ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
            {
                _fallback.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region UInt64
        private sealed class InternalTextualNullableUInt64ReadOnlyConverter : JsonConverter<ulong?>
        {
            private readonly JsonConverter<ulong?> _converter = new Internal.TextualNullableUInt64Converter();
            private readonly JsonConverter<ulong?> _fallback = (JsonConverter<ulong?>)JsonSerializerOptions.Default.GetConverter(typeof(ulong?));

            public override ulong? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, ulong? value, JsonSerializerOptions options)
            {
                _fallback.Write(writer, value, options);
            }

            public override ulong? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, ulong? value, JsonSerializerOptions options)
            {
                _fallback.WriteAsPropertyName(writer, value!, options);
            }
        }

        private sealed class InternalTextualUInt64ReadOnlyConverter : JsonConverter<ulong>
        {
            private readonly JsonConverter<ulong> _converter = new Internal.TextualUInt64Converter();
            private readonly JsonConverter<ulong> _fallback = (JsonConverter<ulong>)JsonSerializerOptions.Default.GetConverter(typeof(ulong));

            public override ulong Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }
            
            public override void Write(Utf8JsonWriter writer, ulong value, JsonSerializerOptions options)
            {
                _fallback.Write(writer, value, options);
            }

            public override ulong ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, ulong value, JsonSerializerOptions options)
            {
                _fallback.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region Float
        private sealed class InternalTextualNullableFloatReadOnlyConverter : JsonConverter<float?>
        {
            private readonly JsonConverter<float?> _converter = new Internal.TextualNullableFloatConverter();
            private readonly JsonConverter<float?> _fallback = (JsonConverter<float?>)JsonSerializerOptions.Default.GetConverter(typeof(float?));

            public override float? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, float? value, JsonSerializerOptions options)
            {
                _fallback.Write(writer, value, options);
            }

            public override float? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, float? value, JsonSerializerOptions options)
            {
                _fallback.WriteAsPropertyName(writer, value!, options);
            }
        }

        private sealed class InternalTextualFloatReadOnlyConverter : JsonConverter<float>
        {
            private readonly JsonConverter<float> _converter = new Internal.TextualFloatConverter();
            private readonly JsonConverter<float> _fallback = (JsonConverter<float>)JsonSerializerOptions.Default.GetConverter(typeof(float));

            public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
            {
                _fallback.Write(writer, value, options);
            }

            public override float ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
            {
                _fallback.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region Double
        private sealed class InternalTextualNullableDoubleReadOnlyConverter : JsonConverter<double?>
        {
            private readonly JsonConverter<double?> _converter = new Internal.TextualNullableDoubleConverter();
            private readonly JsonConverter<double?> _fallback = (JsonConverter<double?>)JsonSerializerOptions.Default.GetConverter(typeof(double?));

            public override double? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, double? value, JsonSerializerOptions options)
            {
                _fallback.Write(writer, value, options);
            }

            public override double? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, double? value, JsonSerializerOptions options)
            {
                _fallback.WriteAsPropertyName(writer, value!, options);
            }
        }

        private sealed class InternalTextualDoubleReadOnlyConverter : JsonConverter<double>
        {
            private readonly JsonConverter<double> _converter = new Internal.TextualDoubleConverter();
            private readonly JsonConverter<double> _fallback = (JsonConverter<double>)JsonSerializerOptions.Default.GetConverter(typeof(double));

            public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
            {
                _fallback.Write(writer, value, options);
            }

            public override double ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
            {
                _fallback.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region Decimal
        private sealed class InternalTextualNullableDecimalReadOnlyConverter : JsonConverter<decimal?>
        {
            private readonly JsonConverter<decimal?> _converter = new Internal.TextualNullableDecimalConverter();
            private readonly JsonConverter<decimal?> _fallback = (JsonConverter<decimal?>)JsonSerializerOptions.Default.GetConverter(typeof(decimal?));

            public override decimal? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options)
            {
                _fallback.Write(writer, value, options);
            }

            public override decimal? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options)
            {
                _fallback.WriteAsPropertyName(writer, value!, options);
            }
        }

        private sealed class InternalTextualDecimalReadOnlyConverter : JsonConverter<decimal>
        {
            private readonly JsonConverter<decimal> _converter = new Internal.TextualDecimalConverter();
            private readonly JsonConverter<decimal> _fallback = (JsonConverter<decimal>)JsonSerializerOptions.Default.GetConverter(typeof(decimal));

            public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
            {
                _fallback.Write(writer, value, options);
            }

            public override decimal ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
            {
                _fallback.WriteAsPropertyName(writer, value!, options);
            }
        }
        #endregion
    }
}
