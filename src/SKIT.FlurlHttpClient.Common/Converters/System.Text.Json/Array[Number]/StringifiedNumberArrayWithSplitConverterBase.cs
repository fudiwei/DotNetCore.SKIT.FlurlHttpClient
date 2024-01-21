namespace System.Text.Json.Serialization.Common
{
    using SKIT.FlurlHttpClient.Internal;

    public abstract partial class StringifiedNumberArrayWithSplitConverterBase : JsonConverterFactory
    {
        protected abstract string Separator { get; }

        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsArray)
                return false;

            Type elementType = typeToConvert.GetElementType()!;
            return TypeHelper.IsNumberType(elementType);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new InternalStringifiedNumberArrayWithSplitConverter(Separator);
        }
    }

    partial class StringifiedNumberArrayWithSplitConverterBase
    {
        private sealed class InternalStringifiedNumberArrayWithSplitConverter : JsonConverter<object?>
        {
            private const string VALUE_NULL = "null";
            private const string VALUE_NAN = "NaN";
            private const string VALUE_POSITIVE_INFINITY = "Infinity";
            private const string VALUE_NEGATIVE_INFINITY = "-Infinity";

            private readonly string _separator;

            public InternalStringifiedNumberArrayWithSplitConverter(string separator)
            {
                _separator = separator;
            }

            public override bool CanConvert(Type typeToConvert)
            {
                // 实际是否可转换依赖外层转换器，此处不做判断，以提升效率
                return true;
            }

            public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return null;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    Type elementType = typeToConvert.GetElementType()!;
                    Type convertType = Nullable.GetUnderlyingType(elementType) ?? elementType;

                    string? value = reader.GetString();
                    if (value is null)
                        return null;
                    if (value == string.Empty)
                        return TypeHelper.CreateNumberArray(elementType);

#if NET5_0_OR_GREATER
                    string[] array = value.Split(_separator);
#else
                    string[] array = value.Split(new string[] { _separator }, StringSplitOptions.None);
#endif
                    Array result = TypeHelper.CreateNumberArray(elementType, array.Length);
                    for (long i = 0, len = result.Length; i < len; i++)
                    {
                        string str = array[i];
                        if (str == string.Empty || str == VALUE_NULL)
                        {
                            continue;
                        }

                        switch (Type.GetTypeCode(convertType))
                        {
                            case TypeCode.SByte:
                                {
                                    if (!sbyte.TryParse(str, out sbyte n))
                                        throw new JsonException($"Could not parse String '{str}' to SByte.");

                                    result.SetValue(n, i);
                                }
                                break;

                            case TypeCode.Byte:
                                {
                                    if (!byte.TryParse(str, out byte n))
                                        throw new JsonException($"Could not parse String '{str}' to Byte.");

                                    result.SetValue(n, i);
                                }
                                break;

                            case TypeCode.Int16:
                                {
                                    if (!short.TryParse(str, out short n))
                                        throw new JsonException($"Could not parse String '{str}' to Int16.");

                                    result.SetValue(n, i);
                                }
                                break;

                            case TypeCode.UInt16:
                                {
                                    if (!ushort.TryParse(str, out ushort n))
                                        throw new JsonException($"Could not parse String '{str}' to UInt16.");

                                    result.SetValue(n, i);
                                }
                                break;

                            case TypeCode.Int32:
                                {
                                    if (!int.TryParse(str, out int n))
                                        throw new JsonException($"Could not parse String '{str}' to Int32.");

                                    result.SetValue(n, i);
                                }
                                break;

                            case TypeCode.UInt32:
                                {
                                    if (!uint.TryParse(str, out uint n))
                                        throw new JsonException($"Could not parse String '{str}' to UInt32.");

                                    result.SetValue(n, i);
                                }
                                break;

                            case TypeCode.Int64:
                                {
                                    if (!long.TryParse(str, out long n))
                                        throw new JsonException($"Could not parse String '{str}' to Int64.");

                                    result.SetValue(n, i);
                                }
                                break;

                            case TypeCode.UInt64:
                                {
                                    if (!ulong.TryParse(str, out ulong n))
                                        throw new JsonException($"Could not parse String '{str}' to UInt64.");

                                    result.SetValue(n, i);
                                }
                                break;

                            case TypeCode.Single:
                                {
                                    float? tmp = default;
                                    if ((options.NumberHandling & JsonNumberHandling.AllowNamedFloatingPointLiterals) > 0)
                                    {
                                        if (str == VALUE_NAN)
                                            tmp = float.NaN;
                                        else if (str == VALUE_POSITIVE_INFINITY)
                                            tmp = float.PositiveInfinity;
                                        else if (str == VALUE_NEGATIVE_INFINITY)
                                            tmp = float.NegativeInfinity;
                                    }

                                    float n;
                                    if (tmp.HasValue)
                                    {
                                        n = tmp.Value;
                                    }
                                    else
                                    {
                                        if (!float.TryParse(str, out n))
                                            throw new JsonException($"Could not parse String '{str}' to Float.");
                                    }

                                    result.SetValue(n, i);
                                }
                                break;

                            case TypeCode.Double:
                                {
                                    double? tmp = default;
                                    if ((options.NumberHandling & JsonNumberHandling.AllowNamedFloatingPointLiterals) > 0)
                                    {
                                        if (str == VALUE_NAN)
                                            tmp = double.NaN;
                                        else if (str == VALUE_POSITIVE_INFINITY)
                                            tmp = double.PositiveInfinity;
                                        else if (str == VALUE_NEGATIVE_INFINITY)
                                            tmp = double.NegativeInfinity;
                                    }

                                    double n;
                                    if (tmp.HasValue)
                                    {
                                        n = tmp.Value;
                                    }
                                    else
                                    {
                                        if (!double.TryParse(str, out n))
                                            throw new JsonException($"Could not parse String '{str}' to Double.");
                                    }

                                    result.SetValue(n, i);
                                }
                                break;

                            case TypeCode.Decimal:
                                {
                                    if (!decimal.TryParse(str, out decimal n))
                                        throw new JsonException($"Could not parse String '{str}' to Decimal.");

                                    result.SetValue(n, i);
                                }
                                break;

                            default:
                                throw new NotSupportedException();
                        }
                    }
                    return result;
                }

                throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
            }

            public override void Write(Utf8JsonWriter writer, object? value, JsonSerializerOptions options)
            {
                if (value is null)
                {
                    writer.WriteNullValue();
                }
                else
                {
                    Type convertType = value.GetType();

                    if (typeof(sbyte[]) == convertType)
                        writer.WriteStringValue(string.Join(_separator, (sbyte[])value));
                    else if(typeof(sbyte?[]) == convertType)
                        writer.WriteStringValue(string.Join(_separator, (sbyte?[])value));
                    else if(typeof(byte[]) == convertType)
                        writer.WriteStringValue(string.Join(_separator, (byte[])value));
                    else if (typeof(byte?[]) == convertType)
                        writer.WriteStringValue(string.Join(_separator, (byte?[])value));
                    else if (typeof(short[]) == convertType)
                        writer.WriteStringValue(string.Join(_separator, (short[])value));
                    else if (typeof(short?[]) == convertType)
                        writer.WriteStringValue(string.Join(_separator, (short?[])value));
                    else if (typeof(ushort[]) == convertType)
                        writer.WriteStringValue(string.Join(_separator, (ushort[])value));
                    else if (typeof(ushort?[]) == convertType)
                        writer.WriteStringValue(string.Join(_separator, (ushort?[])value));
                    else if (typeof(int[]) == convertType)
                        writer.WriteStringValue(string.Join(_separator, (int[])value));
                    else if (typeof(int?[]) == convertType)
                        writer.WriteStringValue(string.Join(_separator, (int?[])value));
                    else if (typeof(uint[]) == convertType)
                        writer.WriteStringValue(string.Join(_separator, (uint[])value));
                    else if (typeof(uint?[]) == convertType)
                        writer.WriteStringValue(string.Join(_separator, (uint?[])value));
                    else if (typeof(long[]) == convertType)
                        writer.WriteStringValue(string.Join(_separator, (long[])value));
                    else if (typeof(long?[]) == convertType)
                        writer.WriteStringValue(string.Join(_separator, (long?[])value));
                    else if (typeof(ulong[]) == convertType)
                        writer.WriteStringValue(string.Join(_separator, (ulong[])value));
                    else if (typeof(ulong?[]) == convertType)
                        writer.WriteStringValue(string.Join(_separator, (ulong?[])value));
                    else if (typeof(float[]) == convertType)
                        writer.WriteStringValue(string.Join(_separator, (float[])value));
                    else if (typeof(float?[]) == convertType)
                        writer.WriteStringValue(string.Join(_separator, (float?[])value));
                    else if (typeof(double[]) == convertType)
                        writer.WriteStringValue(string.Join(_separator, (double[])value));
                    else if (typeof(double?[]) == convertType)
                        writer.WriteStringValue(string.Join(_separator, (double?[])value));
                    else if (typeof(decimal[]) == convertType)
                        writer.WriteStringValue(string.Join(_separator, (decimal[])value));
                    else if (typeof(decimal?[]) == convertType)
                        writer.WriteStringValue(string.Join(_separator, (decimal?[])value));
                    else
                        throw new NotSupportedException();
                }
            }
        }
    }
}
