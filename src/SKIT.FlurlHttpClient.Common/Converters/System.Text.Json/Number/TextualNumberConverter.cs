using System.Text.Json.Serialization;

namespace System.Text.Json.Converters.Common
{
    using SKIT.FlurlHttpClient.Converters.Internal;

    public partial class TextualNumberConverter : JsonConverterFactory
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
                    return convertTypeIsNullable ? new InternalTextualNullableSByteConverter() : new InternalTextualSByteConverter();

                case TypeCode.Byte:
                    return convertTypeIsNullable ? new InternalTextualNullableByteConverter() : new InternalTextualByteConverter();
                    
                case TypeCode.Int16:
                    return convertTypeIsNullable ? new InternalTextualNullableInt16Converter() : new InternalTextualInt16Converter();

                case TypeCode.UInt16:
                    return convertTypeIsNullable ? new InternalTextualNullableUInt16Converter() : new InternalTextualUInt16Converter();

                case TypeCode.Int32:
                    return convertTypeIsNullable ? new InternalTextualNullableInt32Converter() : new InternalTextualInt32Converter();

                case TypeCode.UInt32:
                    return convertTypeIsNullable ? new InternalTextualNullableUInt32Converter() : new InternalTextualUInt32Converter();

                case TypeCode.Int64:
                    return convertTypeIsNullable ? new InternalTextualNullableInt64Converter() : new InternalTextualInt64Converter();

                case TypeCode.UInt64:
                    return convertTypeIsNullable ? new InternalTextualNullableUInt64Converter() : new InternalTextualUInt64Converter();

                case TypeCode.Single:
                    return convertTypeIsNullable ? new InternalTextualNullableSingleConverter() : new InternalTextualSingleConverter();

                case TypeCode.Double:
                    return convertTypeIsNullable ? new InternalTextualNullableDoubleConverter() : new InternalTextualDoubleConverter();

                case TypeCode.Decimal:
                    return convertTypeIsNullable ? new InternalTextualNullableDecimalConverter() : new InternalTextualDecimalConverter();

                default:
                    throw new NotSupportedException();
            }
        }
    }

    partial class TextualNumberConverter
    {
        #region SByte
        internal sealed class InternalTextualNullableSByteConverter : JsonConverter<sbyte?>
        {
            public override sbyte? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return null;
                }
                else if (reader.TokenType == JsonTokenType.Number)
                {
                    return reader.GetSByte();
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    string? value = reader.GetString();
                    if (string.IsNullOrEmpty(value))
                        return null;

                    if (sbyte.TryParse(value, out sbyte result))
                        return result;

                    throw new JsonException($"Could not parse String '{value}' to SByte.");
                }

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
            }

            public override void Write(Utf8JsonWriter writer, sbyte? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteStringValue(value.Value.ToString());
            }

            public override sbyte? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string propName = reader.GetString()!;
                if (string.IsNullOrEmpty(propName))
                    return null;

                if (sbyte.TryParse(propName, out sbyte result))
                    return result;

                throw new JsonException($"Could not parse String '{propName}' to SByte.");
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, sbyte? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WritePropertyName(string.Empty);
                else
                    writer.WritePropertyName(value.Value.ToString());
            }
        }

        internal sealed class InternalTextualSByteConverter : JsonConverter<sbyte>
        {
            private readonly JsonConverter<sbyte?> _converter = new InternalTextualNullableSByteConverter();

            public override sbyte Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                sbyte? result = _converter.Read(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void Write(Utf8JsonWriter writer, sbyte value, JsonSerializerOptions options)
            {
                _converter.Write(writer, value, options);
            }

            public override sbyte ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                sbyte? result = _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, sbyte value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region Byte
        internal sealed class InternalTextualNullableByteConverter : JsonConverter<byte?>
        {
            public override byte? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return null;
                }
                else if (reader.TokenType == JsonTokenType.Number)
                {
                    return reader.GetByte();
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    string? value = reader.GetString();
                    if (string.IsNullOrEmpty(value))
                        return null;

                    if (byte.TryParse(value, out byte result))
                        return result;

                    throw new JsonException($"Could not parse String '{value}' to Byte.");
                }

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
            }

            public override void Write(Utf8JsonWriter writer, byte? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteStringValue(value.Value.ToString());
            }

            public override byte? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string propName = reader.GetString()!;
                if (string.IsNullOrEmpty(propName))
                    return null;

                if (byte.TryParse(propName, out byte result))
                    return result;

                throw new JsonException($"Could not parse String '{propName}' to Byte.");
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, byte? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WritePropertyName(string.Empty);
                else
                    writer.WritePropertyName(value.Value.ToString());
            }
        }

        internal sealed class InternalTextualByteConverter : JsonConverter<byte>
        {
            private readonly JsonConverter<byte?> _converter = new InternalTextualNullableByteConverter();

            public override byte Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                byte? result = _converter.Read(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void Write(Utf8JsonWriter writer, byte value, JsonSerializerOptions options)
            {
                _converter.Write(writer, value, options);
            }

            public override byte ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                byte? result = _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, byte value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region Int16
        internal sealed class InternalTextualNullableInt16Converter : JsonConverter<short?>
        {
            public override short? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return null;
                }
                else if (reader.TokenType == JsonTokenType.Number)
                {
                    return reader.GetInt16();
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    string? value = reader.GetString();
                    if (string.IsNullOrEmpty(value))
                        return null;

                    if (short.TryParse(value, out short result))
                        return result;

                    throw new JsonException($"Could not parse String '{value}' to Int16.");
                }

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
            }

            public override void Write(Utf8JsonWriter writer, short? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteStringValue(value.Value.ToString());
            }

            public override short? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string propName = reader.GetString()!;
                if (string.IsNullOrEmpty(propName))
                    return null;

                if (short.TryParse(propName, out short result))
                    return result;

                throw new JsonException($"Could not parse String '{propName}' to Int16.");
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, short? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WritePropertyName(string.Empty);
                else
                    writer.WritePropertyName(value.Value.ToString());
            }
        }

        internal sealed class InternalTextualInt16Converter : JsonConverter<short>
        {
            private readonly JsonConverter<short?> _converter = new InternalTextualNullableInt16Converter();

            public override short Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                short? result = _converter.Read(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void Write(Utf8JsonWriter writer, short value, JsonSerializerOptions options)
            {
                _converter.Write(writer, value, options);
            }

            public override short ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                short? result = _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, short value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region UInt16
        internal sealed class InternalTextualNullableUInt16Converter : JsonConverter<ushort?>
        {
            public override ushort? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return null;
                }
                else if (reader.TokenType == JsonTokenType.Number)
                {
                    return reader.GetUInt16();
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    string? value = reader.GetString();
                    if (string.IsNullOrEmpty(value))
                        return null;

                    if (ushort.TryParse(value, out ushort result))
                        return result;

                    throw new JsonException($"Could not parse String '{value}' to UInt16.");
                }

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
            }

            public override void Write(Utf8JsonWriter writer, ushort? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteStringValue(value.Value.ToString());
            }

            public override ushort? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string propName = reader.GetString()!;
                if (string.IsNullOrEmpty(propName))
                    return null;

                if (ushort.TryParse(propName, out ushort result))
                    return result;

                throw new JsonException($"Could not parse String '{propName}' to UInt16.");
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, ushort? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WritePropertyName(string.Empty);
                else
                    writer.WritePropertyName(value.Value.ToString());
            }
        }

        internal sealed class InternalTextualUInt16Converter : JsonConverter<ushort>
        {
            private readonly JsonConverter<ushort?> _converter = new InternalTextualNullableUInt16Converter();

            public override ushort Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                ushort? result = _converter.Read(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void Write(Utf8JsonWriter writer, ushort value, JsonSerializerOptions options)
            {
                _converter.Write(writer, value, options);
            }

            public override ushort ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                ushort? result = _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, ushort value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region Int32
        internal sealed class InternalTextualNullableInt32Converter : JsonConverter<int?>
        {
            public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return null;
                }
                else if (reader.TokenType == JsonTokenType.Number)
                {
                    return reader.GetInt32();
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    string? value = reader.GetString();
                    if (string.IsNullOrEmpty(value))
                        return null;

                    if (int.TryParse(value, out int result))
                        return result;

                    throw new JsonException($"Could not parse String '{value}' to Int32.");
                }

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
            }

            public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteStringValue(value.Value.ToString());
            }

            public override int? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string propName = reader.GetString()!;
                if (string.IsNullOrEmpty(propName))
                    return null;

                if (int.TryParse(propName, out int result))
                    return result;

                throw new JsonException($"Could not parse String '{propName}' to Int32.");
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WritePropertyName(string.Empty);
                else
                    writer.WritePropertyName(value.Value.ToString());
            }
        }

        internal sealed class InternalTextualInt32Converter : JsonConverter<int>
        {
            private readonly JsonConverter<int?> _converter = new InternalTextualNullableInt32Converter();

            public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                int? result = _converter.Read(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
            {
                _converter.Write(writer, value, options);
            }

            public override int ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                int? result = _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region UInt32
        internal sealed class InternalTextualNullableUInt32Converter : JsonConverter<uint?>
        {
            public override uint? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return null;
                }
                else if (reader.TokenType == JsonTokenType.Number)
                {
                    return reader.GetUInt32();
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    string? value = reader.GetString();
                    if (string.IsNullOrEmpty(value))
                        return null;

                    if (uint.TryParse(value, out uint result))
                        return result;

                    throw new JsonException($"Could not parse String '{value}' to UInt32.");
                }

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
            }

            public override void Write(Utf8JsonWriter writer, uint? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteStringValue(value.Value.ToString());
            }

            public override uint? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string propName = reader.GetString()!;
                if (string.IsNullOrEmpty(propName))
                    return null;

                if (uint.TryParse(propName, out uint result))
                    return result;

                throw new JsonException($"Could not parse String '{propName}' to UInt32.");
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, uint? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WritePropertyName(string.Empty);
                else
                    writer.WritePropertyName(value.Value.ToString());
            }
        }

        internal sealed class InternalTextualUInt32Converter : JsonConverter<uint>
        {
            private readonly JsonConverter<uint?> _converter = new InternalTextualNullableUInt32Converter();

            public override uint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                uint? result = _converter.Read(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void Write(Utf8JsonWriter writer, uint value, JsonSerializerOptions options)
            {
                _converter.Write(writer, value, options);
            }

            public override uint ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                uint? result = _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, uint value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region Int64
        internal sealed class InternalTextualNullableInt64Converter : JsonConverter<long?>
        {
            public override long? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return null;
                }
                else if (reader.TokenType == JsonTokenType.Number)
                {
                    return reader.GetInt64();
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    string? value = reader.GetString();
                    if (string.IsNullOrEmpty(value))
                        return null;

                    if (long.TryParse(value, out long result))
                        return result;

                    throw new JsonException($"Could not parse String '{value}' to Int64.");
                }

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
            }

            public override void Write(Utf8JsonWriter writer, long? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteStringValue(value.Value.ToString());
            }

            public override long? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string propName = reader.GetString()!;
                if (string.IsNullOrEmpty(propName))
                    return null;

                if (long.TryParse(propName, out long result))
                    return result;

                throw new JsonException($"Could not parse String '{propName}' to Int64.");
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, long? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WritePropertyName(string.Empty);
                else
                    writer.WritePropertyName(value.Value.ToString());
            }
        }

        internal sealed class InternalTextualInt64Converter : JsonConverter<long>
        {
            private readonly JsonConverter<long?> _converter = new InternalTextualNullableInt64Converter();

            public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                long? result = _converter.Read(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
            {
                _converter.Write(writer, value, options);
            }

            public override long ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                long? result = _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region UInt64
        internal sealed class InternalTextualNullableUInt64Converter : JsonConverter<ulong?>
        {
            public override ulong? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return null;
                }
                else if (reader.TokenType == JsonTokenType.Number)
                {
                    return reader.GetUInt64();
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    string? value = reader.GetString();
                    if (string.IsNullOrEmpty(value))
                        return null;

                    if (ulong.TryParse(value, out ulong result))
                        return result;

                    throw new JsonException($"Could not parse String '{value}' to UInt64.");
                }

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
            }

            public override void Write(Utf8JsonWriter writer, ulong? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteStringValue(value.Value.ToString());
            }

            public override ulong? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string propName = reader.GetString()!;
                if (string.IsNullOrEmpty(propName))
                    return null;

                if (ulong.TryParse(propName, out ulong result))
                    return result;

                throw new JsonException($"Could not parse String '{propName}' to UInt64.");
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, ulong? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WritePropertyName(string.Empty);
                else
                    writer.WritePropertyName(value.Value.ToString());
            }
        }

        internal sealed class InternalTextualUInt64Converter : JsonConverter<ulong>
        {
            private readonly JsonConverter<ulong?> _converter = new InternalTextualNullableUInt64Converter();

            public override ulong Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                ulong? result = _converter.Read(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void Write(Utf8JsonWriter writer, ulong value, JsonSerializerOptions options)
            {
                _converter.Write(writer, value, options);
            }

            public override ulong ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                ulong? result = _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, ulong value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region Single
        internal sealed class InternalTextualNullableSingleConverter : JsonConverter<float?>
        {
            public override float? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return null;
                }
                else if (reader.TokenType == JsonTokenType.Number)
                {
                    return reader.GetSingle();
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    string? value = reader.GetString();
                    if (string.IsNullOrEmpty(value))
                        return null;

                    if (float.TryParse(value, out float result))
                        return result;

                    throw new JsonException($"Could not parse String '{value}' to Single.");
                }

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
            }

            public override void Write(Utf8JsonWriter writer, float? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteStringValue(value.Value.ToString());
            }

            public override float? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string propName = reader.GetString()!;
                if (string.IsNullOrEmpty(propName))
                    return null;

                if (float.TryParse(propName, out float result))
                    return result;

                throw new JsonException($"Could not parse String '{propName}' to Single.");
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, float? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WritePropertyName(string.Empty);
                else
                    writer.WritePropertyName(value.Value.ToString());
            }
        }

        internal sealed class InternalTextualSingleConverter : JsonConverter<float>
        {
            private readonly JsonConverter<float?> _converter = new InternalTextualNullableSingleConverter();

            public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                float? result = _converter.Read(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
            {
                _converter.Write(writer, value, options);
            }

            public override float ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                float? result = _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region Double
        internal sealed class InternalTextualNullableDoubleConverter : JsonConverter<double?>
        {
            public override double? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return null;
                }
                else if (reader.TokenType == JsonTokenType.Number)
                {
                    return reader.GetDouble();
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    string? value = reader.GetString();
                    if (string.IsNullOrEmpty(value))
                        return null;

                    if (double.TryParse(value, out double result))
                        return result;

                    throw new JsonException($"Could not parse String '{value}' to Double.");
                }

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
            }

            public override void Write(Utf8JsonWriter writer, double? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteStringValue(value.Value.ToString());
            }

            public override double? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string propName = reader.GetString()!;
                if (string.IsNullOrEmpty(propName))
                    return null;

                if (double.TryParse(propName, out double result))
                    return result;

                throw new JsonException($"Could not parse String '{propName}' to Double.");
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, double? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WritePropertyName(string.Empty);
                else
                    writer.WritePropertyName(value.Value.ToString());
            }
        }

        internal sealed class InternalTextualDoubleConverter : JsonConverter<double>
        {
            private readonly JsonConverter<double?> _converter = new InternalTextualNullableDoubleConverter();

            public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                double? result = _converter.Read(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
            {
                _converter.Write(writer, value, options);
            }

            public override double ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                double? result = _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion

        #region Decimal
        internal sealed class InternalTextualNullableDecimalConverter : JsonConverter<decimal?>
        {
            public override decimal? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return null;
                }
                else if (reader.TokenType == JsonTokenType.Number)
                {
                    return reader.GetDecimal();
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    string? value = reader.GetString();
                    if (string.IsNullOrEmpty(value))
                        return null;

                    if (decimal.TryParse(value, out decimal result))
                        return result;

                    throw new JsonException($"Could not parse String '{value}' to Decimal.");
                }

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
            }

            public override void Write(Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WriteNullValue();
                else
                    writer.WriteStringValue(value.Value.ToString());
            }

            public override decimal? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string propName = reader.GetString()!;
                if (string.IsNullOrEmpty(propName))
                    return null;

                if (decimal.TryParse(propName, out decimal result))
                    return result;

                throw new JsonException($"Could not parse String '{propName}' to Decimal.");
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options)
            {
                if (value is null)
                    writer.WritePropertyName(string.Empty);
                else
                    writer.WritePropertyName(value.Value.ToString());
            }
        }

        internal sealed class InternalTextualDecimalConverter : JsonConverter<decimal>
        {
            private readonly JsonConverter<decimal?> _converter = new InternalTextualNullableDecimalConverter();

            public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                decimal? result = _converter.Read(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
            {
                _converter.Write(writer, value, options);
            }

            public override decimal ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                decimal? result = _converter.ReadAsPropertyName(ref reader, typeToConvert, options);
                return result.GetValueOrDefault();
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
            {
                _converter.WriteAsPropertyName(writer, value, options);
            }
        }
        #endregion
    }
}
