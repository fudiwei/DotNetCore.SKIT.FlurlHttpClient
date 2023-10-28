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

            public override sbyte? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, sbyte? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteNumberValue(value.Value);
            }

            public override sbyte? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, sbyte? value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }

        private sealed class InternalTextualSByteReadOnlyConverter : JsonConverter<sbyte>
        {
            private readonly JsonConverter<sbyte> _converter = new Internal.TextualSByteConverter();

            public override sbyte Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, sbyte value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value);
            }

            public override sbyte ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, sbyte value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region Byte
        private sealed class InternalTextualNullableByteReadOnlyConverter : JsonConverter<byte?>
        {
            private readonly JsonConverter<byte?> _converter = new Internal.TextualNullableByteConverter();

            public override byte? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, byte? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteNumberValue(value.Value);
            }

            public override byte? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, byte? value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }

        private sealed class InternalTextualByteReadOnlyConverter : JsonConverter<byte>
        {
            private readonly JsonConverter<byte> _converter = new Internal.TextualByteConverter();

            public override byte Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, byte value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value);
            }

            public override byte ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, byte value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region Int16
        private sealed class InternalTextualNullableInt16ReadOnlyConverter : JsonConverter<short?>
        {
            private readonly JsonConverter<short?> _converter = new Internal.TextualNullableInt16Converter();

            public override short? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, short? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteNumberValue(value.Value);
            }

            public override short? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, short? value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }

        private sealed class InternalTextualInt16ReadOnlyConverter : JsonConverter<short>
        {
            private readonly JsonConverter<short> _converter = new Internal.TextualInt16Converter();

            public override short Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, short value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value);
            }

            public override short ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, short value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region UInt16
        private sealed class InternalTextualNullableUInt16ReadOnlyConverter : JsonConverter<ushort?>
        {
            private readonly JsonConverter<ushort?> _converter = new Internal.TextualNullableUInt16Converter();

            public override ushort? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, ushort? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteNumberValue(value.Value);
            }

            public override ushort? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, ushort? value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }

        private sealed class InternalTextualUInt16ReadOnlyConverter : JsonConverter<ushort>
        {
            private readonly JsonConverter<ushort> _converter = new Internal.TextualUInt16Converter();

            public override ushort Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, ushort value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value);
            }

            public override ushort ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, ushort value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region Int32
        private sealed class InternalTextualNullableInt32ReadOnlyConverter : JsonConverter<int?>
        {
            private readonly JsonConverter<int?> _converter = new Internal.TextualNullableInt32Converter();

            public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteNumberValue(value.Value);
            }

            public override int? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }

        private sealed class InternalTextualInt32ReadOnlyConverter : JsonConverter<int>
        {
            private readonly JsonConverter<int> _converter = new Internal.TextualInt32Converter();

            public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value);
            }

            public override int ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region UInt32
        private sealed class InternalTextualNullableUInt32ReadOnlyConverter : JsonConverter<uint?>
        {
            private readonly JsonConverter<uint?> _converter = new Internal.TextualNullableUInt32Converter();

            public override uint? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, uint? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteNumberValue(value.Value);
            }

            public override uint? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, uint? value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }

        private sealed class InternalTextualUInt32ReadOnlyConverter : JsonConverter<uint>
        {
            private readonly JsonConverter<uint> _converter = new Internal.TextualUInt32Converter();

            public override uint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, uint value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value);
            }

            public override uint ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, uint value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region Int64
        private sealed class InternalTextualNullableInt64ReadOnlyConverter : JsonConverter<long?>
        {
            private readonly JsonConverter<long?> _converter = new Internal.TextualNullableInt64Converter();

            public override long? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, long? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteNumberValue(value.Value);
            }

            public override long? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, long? value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }

        private sealed class InternalTextualInt64ReadOnlyConverter : JsonConverter<long>
        {
            private readonly JsonConverter<long> _converter = new Internal.TextualInt64Converter();

            public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value);
            }

            public override long ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region UInt64
        private sealed class InternalTextualNullableUInt64ReadOnlyConverter : JsonConverter<ulong?>
        {
            private readonly JsonConverter<ulong?> _converter = new Internal.TextualNullableUInt64Converter();

            public override ulong? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, ulong? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteNumberValue(value.Value);
            }

            public override ulong? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, ulong? value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }

        private sealed class InternalTextualUInt64ReadOnlyConverter : JsonConverter<ulong>
        {
            private readonly JsonConverter<ulong> _converter = new Internal.TextualUInt64Converter();

            public override ulong Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }
            
            public override void Write(Utf8JsonWriter writer, ulong value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value);
            }

            public override ulong ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, ulong value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region Float
        private sealed class InternalTextualNullableFloatReadOnlyConverter : JsonConverter<float?>
        {
            private readonly JsonConverter<float?> _converter = new Internal.TextualNullableFloatConverter();

            public override float? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, float? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteNumberValue(value.Value);
            }

            public override float? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, float? value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }

        private sealed class InternalTextualFloatReadOnlyConverter : JsonConverter<float>
        {
            private readonly JsonConverter<float> _converter = new Internal.TextualFloatConverter();

            public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value);
            }

            public override float ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region Double
        private sealed class InternalTextualNullableDoubleReadOnlyConverter : JsonConverter<double?>
        {
            private readonly JsonConverter<double?> _converter = new Internal.TextualNullableDoubleConverter();

            public override double? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, double? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteNumberValue(value.Value);
            }

            public override double? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, double? value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }

        private sealed class InternalTextualDoubleReadOnlyConverter : JsonConverter<double>
        {
            private readonly JsonConverter<double> _converter = new Internal.TextualDoubleConverter();

            public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value);
            }

            public override double ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region Decimal
        private sealed class InternalTextualNullableDecimalReadOnlyConverter : JsonConverter<decimal?>
        {
            private readonly JsonConverter<decimal?> _converter = new Internal.TextualNullableDecimalConverter();

            public override decimal? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteNumberValue(value.Value);
            }

            public override decimal? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }

        private sealed class InternalTextualDecimalReadOnlyConverter : JsonConverter<decimal>
        {
            private readonly JsonConverter<decimal> _converter = new Internal.TextualDecimalConverter();

            public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.Read(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value);
            }

            public override decimal ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion
    }
}
