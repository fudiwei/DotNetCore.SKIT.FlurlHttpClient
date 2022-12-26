using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Converters.Common
{
    using SKIT.FlurlHttpClient.Converters.Internal;

    public sealed class TextualNumberArrayConverter : JsonConverter
    {
        private const string VALUE_NULL = "null";
        private const string VALUE_NAN= "NaN";
        private const string VALUE_POSITIVE_INFINITY = "Infinity";
        private const string VALUE_NEGATIVE_INFINITY = "-Infinity";

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
            else if (reader.TokenType == JsonToken.StartArray)
            {
                Type elementType = objectType.GetElementType()!;
                Type convertType = Nullable.GetUnderlyingType(elementType) ?? elementType;

                IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType))!;

                while (reader.Read())
                {
                    switch (reader.TokenType)
                    {
                        case JsonToken.Null:
                            {
                                // N or Nullable<N>
                                if (convertType == elementType)
                                    list.Add(0);
                                else
                                    list.Add(null);
                            }
                            break;

                        case JsonToken.Integer:
                        case JsonToken.Float:
                        case JsonToken.String:
                            {
                                string? str = default;

                                if (reader.TokenType == JsonToken.String)
                                {
                                    str = serializer.Deserialize<string>(reader);
                                    if (str == string.Empty || str == VALUE_NULL)
                                    {
                                        // N or Nullable<N>
                                        if (convertType == elementType)
                                            list.Add(0);
                                        else
                                            list.Add(null);
                                    }
                                }

                                switch (Type.GetTypeCode(convertType))
                                {
                                    case TypeCode.SByte:
                                        {
                                            list.Add(serializer.Deserialize<sbyte>(reader));
                                        }
                                        break;

                                    case TypeCode.Byte:
                                        {
                                            list.Add(serializer.Deserialize<byte>(reader));
                                        }
                                        break;

                                    case TypeCode.Int16:
                                        {
                                            list.Add(serializer.Deserialize<short>(reader));
                                        }
                                        break;

                                    case TypeCode.UInt16:
                                        {
                                            list.Add(serializer.Deserialize<ushort>(reader));
                                        }
                                        break;

                                    case TypeCode.Int32:
                                        {
                                            list.Add(serializer.Deserialize<int>(reader));
                                        }
                                        break;

                                    case TypeCode.UInt32:
                                        {
                                            list.Add(serializer.Deserialize<uint>(reader));
                                        }
                                        break;

                                    case TypeCode.Int64:
                                        {
                                            list.Add(serializer.Deserialize<long>(reader));
                                        }
                                        break;

                                    case TypeCode.UInt64:
                                        {
                                            list.Add(serializer.Deserialize<ulong>(reader));
                                        }
                                        break;

                                    case TypeCode.Single:
                                        {
                                            float? tmp = default;
                                            if (str != null && serializer.FloatFormatHandling == FloatFormatHandling.String)
                                            {
                                                if (str == VALUE_NAN)
                                                    tmp = float.NaN;
                                                else if (str == VALUE_POSITIVE_INFINITY)
                                                    tmp = float.PositiveInfinity;
                                                else if (str == VALUE_NEGATIVE_INFINITY)
                                                    tmp = float.NegativeInfinity;
                                            }

                                            if (tmp.HasValue)
                                                list.Add(tmp);
                                            else
                                                list.Add(serializer.Deserialize<float>(reader));
                                        }
                                        break;

                                    case TypeCode.Double:
                                        {
                                            double? tmp = default;
                                            if (str != null && serializer.FloatFormatHandling == FloatFormatHandling.String)
                                            {
                                                if (str == VALUE_NAN)
                                                    tmp = double.NaN;
                                                else if (str == VALUE_POSITIVE_INFINITY)
                                                    tmp = double.PositiveInfinity;
                                                else if (str == VALUE_NEGATIVE_INFINITY)
                                                    tmp = double.NegativeInfinity;

                                                if (tmp.HasValue)
                                                {
                                                    list.Add(tmp);
                                                    break;
                                                }
                                            }

                                            if (tmp.HasValue)
                                                list.Add(tmp);
                                            else
                                                list.Add(serializer.Deserialize<double>(reader));
                                        }
                                        break;

                                    case TypeCode.Decimal:
                                        {
                                            list.Add(serializer.Deserialize<decimal>(reader));
                                        }
                                        break;

                                    default:
                                        throw new NotSupportedException($"Could not convert type '{convertType}' to a number.");
                                }
                            }
                            break;

                        case JsonToken.EndArray:
                            {
                                Array result = Array.CreateInstance(elementType, list.Count);
                                list.CopyTo(result, 0);
                                return result;
                            }

                        default:
                            break;
                    }
                }
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
                writer.WriteStartArray();

                foreach (object element in (Array)value)
                {
                    if (element is null)
                    { 
                        writer.WriteNull();
                    }
                    else
                    {
                        Type elementType = element.GetType();
                        if ((typeof(float) == elementType && float.IsNaN((float)element)) ||
                            (typeof(double) == elementType && double.IsNaN((double)element)))
                        {
                            writer.WriteValue(VALUE_NAN);
                        }
                        else if ((typeof(float) == elementType && float.IsInfinity((float)element) && !float.IsNegativeInfinity((float)element)) ||
                                 (typeof(double) == elementType && double.IsInfinity((double)element) && !double.IsNegativeInfinity((double)element)))
                        {
                            writer.WriteValue(VALUE_POSITIVE_INFINITY);
                        }
                        else if ((typeof(float) == elementType && float.IsNegativeInfinity((float)element)) ||
                                 (typeof(double) == elementType && double.IsNegativeInfinity((double)element)))
                        {
                            writer.WriteValue(VALUE_NEGATIVE_INFINITY);
                        }
                        else
                        {
                            // 正负无穷数需特殊处理，不能直接 ToString()，否则会输出成 "∞"、"-∞"
                            writer.WriteValue(element.ToString());
                        }
                    }
                }

                writer.WriteEndArray();
            }
        }
    }
}
