using System.Text.Json.Serialization;

namespace System.Text.Json.Converters.Common
{
    using SKIT.FlurlHttpClient.Converters.Internal;

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
                    return convertTypeIsNullable ? new InternalTextualNullableSingleReadOnlyConverter() : new InternalTextualSingleReadOnlyConverter();

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
            private readonly JsonConverter<sbyte?> _converter = new TextualNumberConverter.InternalTextualNullableSByteConverter();

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
            private readonly JsonConverter<sbyte> _converter = new TextualNumberConverter.InternalTextualSByteConverter();

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
            private readonly JsonConverter<byte?> _converter = new TextualNumberConverter.InternalTextualNullableByteConverter();

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
            private readonly JsonConverter<byte> _converter = new TextualNumberConverter.InternalTextualByteConverter();

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
            private readonly JsonConverter<short?> _converter = new TextualNumberConverter.InternalTextualNullableInt16Converter();

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
            private readonly JsonConverter<short> _converter = new TextualNumberConverter.InternalTextualInt16Converter();

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
            private readonly JsonConverter<ushort?> _converter = new TextualNumberConverter.InternalTextualNullableUInt16Converter();

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
            private readonly JsonConverter<ushort> _converter = new TextualNumberConverter.InternalTextualUInt16Converter();

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
            private readonly JsonConverter<int?> _converter = new TextualNumberConverter.InternalTextualNullableInt32Converter();

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
            private readonly JsonConverter<int> _converter = new TextualNumberConverter.InternalTextualInt32Converter();

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
            private readonly JsonConverter<uint?> _converter = new TextualNumberConverter.InternalTextualNullableUInt32Converter();

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
            private readonly JsonConverter<uint> _converter = new TextualNumberConverter.InternalTextualUInt32Converter();

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
            private readonly JsonConverter<long?> _converter = new TextualNumberConverter.InternalTextualNullableInt64Converter();

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
            private readonly JsonConverter<long> _converter = new TextualNumberConverter.InternalTextualInt64Converter();

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
            private readonly JsonConverter<ulong?> _converter = new TextualNumberConverter.InternalTextualNullableUInt64Converter();

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
            private readonly JsonConverter<ulong> _converter = new TextualNumberConverter.InternalTextualUInt64Converter();

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

        #region Single
        private sealed class InternalTextualNullableSingleReadOnlyConverter : JsonConverter<float?>
        {
            private readonly JsonConverter<float?> _converter = new TextualNumberConverter.InternalTextualNullableSingleConverter();

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

        private sealed class InternalTextualSingleReadOnlyConverter : JsonConverter<float>
        {
            private readonly JsonConverter<float> _converter = new TextualNumberConverter.InternalTextualSingleConverter();

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
            private readonly JsonConverter<double?> _converter = new TextualNumberConverter.InternalTextualNullableDoubleConverter();

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
            private readonly JsonConverter<double> _converter = new TextualNumberConverter.InternalTextualDoubleConverter();

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
            private readonly JsonConverter<decimal?> _converter = new TextualNumberConverter.InternalTextualNullableDecimalConverter();

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
            private readonly JsonConverter<decimal> _converter = new TextualNumberConverter.InternalTextualDecimalConverter();

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
