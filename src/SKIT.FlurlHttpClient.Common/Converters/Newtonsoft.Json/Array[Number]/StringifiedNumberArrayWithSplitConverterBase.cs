using System;

namespace Newtonsoft.Json.Converters.Common
{
    using SKIT.FlurlHttpClient.Internal;

    public abstract class StringifiedNumberArrayWithSplitConverterBase : JsonConverter
    {
        private const string VALUE_NULL = "null";
        private const string VALUE_NAN= "NaN";
        private const string VALUE_POSITIVE_INFINITY = "Infinity";
        private const string VALUE_NEGATIVE_INFINITY = "-Infinity";

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
            if (!objectType.IsArray)
                return false;

            Type elementType = objectType.GetElementType()!;
            return TypeHelper.IsNumberType(elementType);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return existingValue;
            }
            else if (reader.TokenType == JsonToken.String)
            {
                Type elementType = objectType.GetElementType()!;
                Type convertType = Nullable.GetUnderlyingType(elementType) ?? elementType;

                string? value = serializer.Deserialize<string>(reader);
                if (value is null)
                    return null;
                if (value == string.Empty)
                    return TypeHelper.CreateNumberArray(elementType);

#if NET5_0_OR_GREATER
                string[] array = value.Split(Separator);
#else
                string[] array = value.Split(new string[] { Separator }, StringSplitOptions.None);
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
                                    throw new JsonSerializationException($"Could not parse String '{str}' to SByte.");

                                result.SetValue(n, i);
                            }
                            break;

                        case TypeCode.Byte:
                            {
                                if (!byte.TryParse(str, out byte n))
                                    throw new JsonSerializationException($"Could not parse String '{str}' to Byte.");

                                result.SetValue(n, i);
                            }
                            break;

                        case TypeCode.Int16:
                            {
                                if (!short.TryParse(str, out short n))
                                    throw new JsonSerializationException($"Could not parse String '{str}' to Int16.");

                                result.SetValue(n, i);
                            }
                            break;

                        case TypeCode.UInt16:
                            {
                                if (!ushort.TryParse(str, out ushort n))
                                    throw new JsonSerializationException($"Could not parse String '{str}' to UInt16.");

                                result.SetValue(n, i);
                            }
                            break;

                        case TypeCode.Int32:
                            {
                                if (!int.TryParse(str, out int n))
                                    throw new JsonSerializationException($"Could not parse String '{str}' to Int32.");

                                result.SetValue(n, i);
                            }
                            break;

                        case TypeCode.UInt32:
                            {
                                if (!uint.TryParse(str, out uint n))
                                    throw new JsonSerializationException($"Could not parse String '{str}' to UInt32.");

                                result.SetValue(n, i);
                            }
                            break;

                        case TypeCode.Int64:
                            {
                                if (!long.TryParse(str, out long n))
                                    throw new JsonSerializationException($"Could not parse String '{str}' to Int64.");

                                result.SetValue(n, i);
                            }
                            break;

                        case TypeCode.UInt64:
                            {
                                if (!ulong.TryParse(str, out ulong n))
                                    throw new JsonSerializationException($"Could not parse String '{str}' to UInt64.");

                                result.SetValue(n, i);
                            }
                            break;

                        case TypeCode.Single:
                            {
                                float? tmp = default;
                                if (serializer.FloatFormatHandling == FloatFormatHandling.String)
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
                                        throw new JsonSerializationException($"Could not parse String '{str}' to Float.");
                                }

                                result.SetValue(n, i);
                            }
                            break;

                        case TypeCode.Double:
                            {
                                double? tmp = default;
                                if (serializer.FloatFormatHandling == FloatFormatHandling.String)
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
                                        throw new JsonSerializationException($"Could not parse String '{str}' to Double.");
                                }

                                result.SetValue(n, i);
                            }
                            break;

                        case TypeCode.Decimal:
                            {
                                if (!decimal.TryParse(str, out decimal n))
                                    throw new JsonSerializationException($"Could not parse String '{str}' to Decimal.");

                                result.SetValue(n, i);
                            }
                            break;

                        default:
                            throw new NotSupportedException();
                    }
                }
                return result;
            }

            throw new JsonSerializationException($"Unexpected token type '{reader.TokenType}' when deserializing. Path '{reader.Path}'.");
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is null)
            {
                writer.WriteNull();
            }
            else
            {
                Type convertType = value.GetType();

                if (typeof(sbyte[]) == convertType)
                    writer.WriteValue(string.Join(Separator, (sbyte[])value));
                else if (typeof(sbyte?[]) == convertType)
                    writer.WriteValue(string.Join(Separator, (sbyte?[])value));
                else if (typeof(byte[]) == convertType)
                    writer.WriteValue(string.Join(Separator, (byte[])value));
                else if (typeof(byte?[]) == convertType)
                    writer.WriteValue(string.Join(Separator, (byte?[])value));
                else if (typeof(short[]) == convertType)
                    writer.WriteValue(string.Join(Separator, (short[])value));
                else if (typeof(short?[]) == convertType)
                    writer.WriteValue(string.Join(Separator, (short?[])value));
                else if (typeof(ushort[]) == convertType)
                    writer.WriteValue(string.Join(Separator, (ushort[])value));
                else if (typeof(ushort?[]) == convertType)
                    writer.WriteValue(string.Join(Separator, (ushort?[])value));
                else if (typeof(int[]) == convertType)
                    writer.WriteValue(string.Join(Separator, (int[])value));
                else if (typeof(int?[]) == convertType)
                    writer.WriteValue(string.Join(Separator, (int?[])value));
                else if (typeof(uint[]) == convertType)
                    writer.WriteValue(string.Join(Separator, (uint[])value));
                else if (typeof(uint?[]) == convertType)
                    writer.WriteValue(string.Join(Separator, (uint?[])value));
                else if (typeof(long[]) == convertType)
                    writer.WriteValue(string.Join(Separator, (long[])value));
                else if (typeof(long?[]) == convertType)
                    writer.WriteValue(string.Join(Separator, (long?[])value));
                else if (typeof(ulong[]) == convertType)
                    writer.WriteValue(string.Join(Separator, (ulong[])value));
                else if (typeof(ulong?[]) == convertType)
                    writer.WriteValue(string.Join(Separator, (ulong?[])value));
                else if (typeof(float[]) == convertType)
                    writer.WriteValue(string.Join(Separator, (float[])value));
                else if (typeof(float?[]) == convertType)
                    writer.WriteValue(string.Join(Separator, (float?[])value));
                else if (typeof(double[]) == convertType)
                    writer.WriteValue(string.Join(Separator, (double[])value));
                else if (typeof(double?[]) == convertType)
                    writer.WriteValue(string.Join(Separator, (double?[])value));
                else if (typeof(decimal[]) == convertType)
                    writer.WriteValue(string.Join(Separator, (decimal[])value));
                else if (typeof(decimal?[]) == convertType)
                    writer.WriteValue(string.Join(Separator, (decimal?[])value));
                else
                    throw new NotSupportedException();
            }
        }
    }
}
