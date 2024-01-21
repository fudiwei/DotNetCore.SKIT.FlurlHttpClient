using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Newtonsoft.Json.Converters.Common
{
    using SKIT.FlurlHttpClient.Internal;

    /// <summary>
    /// 一个 JSON 转换器，可针对指定适配类型做如下形式的对象转换。
    /// <code>
    ///   .NET → IList&lt;int&gt; Foo { get; } = new List&lt;int&gt;() { 1, 2 };
    ///   JSON → { "Foo": ["1", "2"] }
    /// </code>
    /// 
    /// 适配类型：
    /// <code>  <see cref="sbyte"/>[] <see cref="sbyte"/>?[]</code>
    /// <code>  <see cref="byte"/>[] <see cref="byte"/>[]</code>
    /// <code>  <see cref="ushort"/>[] <see cref="ushort"/>?[]</code>
    /// <code>  <see cref="short"/>[] <see cref="short"/>?[]</code>
    /// <code>  <see cref="uint"/>[] <see cref="uint"/>?[]</code>
    /// <code>  <see cref="int"/>[] <see cref="int"/>?[]</code>
    /// <code>  <see cref="ulong"/>[] <see cref="ulong"/>?[]</code>
    /// <code>  <see cref="long"/>[] <see cref="long"/>?[]</code>
    /// <code>  <see cref="float"/>[] <see cref="float"/>?[]</code>
    /// <code>  <see cref="double"/>[] <see cref="double"/>?[]</code>
    /// <code>  <see cref="decimal"/>[] <see cref="decimal"/>?[]</code>
    /// </summary>
    public sealed class TextualNumberListConverter : JsonConverter
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
            if (!objectType.IsGenericType)
                return false;

            if (!typeof(IList<>).IsAssignableFrom(objectType.GetGenericTypeDefinition()))
                return false;

            Type elementType = objectType.GetGenericArguments()[0];
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
                Type elementType = objectType.GetGenericArguments()[0];
                Type convertType = Nullable.GetUnderlyingType(elementType) ?? elementType;

                IList list = TypeHelper.CreateNumberList(elementType);

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

                                        continue;
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
                                            if (str is not null && serializer.FloatFormatHandling == FloatFormatHandling.String)
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
                                            if (str is not null && serializer.FloatFormatHandling == FloatFormatHandling.String)
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
                                return list;
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

                foreach (object element in (IEnumerable)value)
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
                            switch (serializer.FloatFormatHandling)
                            {
                                case FloatFormatHandling.DefaultValue:
                                    {
                                        writer.WriteNull();
                                    }
                                    break;

                                case FloatFormatHandling.Symbol:
                                    {
                                        writer.WriteRawValue(VALUE_NAN);
                                    }
                                    break;

                                case FloatFormatHandling.String:
                                default:
                                    {
                                        writer.WriteValue(VALUE_NAN);
                                    }
                                    break;
                            }
                        }
                        else if ((typeof(float) == elementType && float.IsInfinity((float)element) && !float.IsNegativeInfinity((float)element)) ||
                                 (typeof(double) == elementType && double.IsInfinity((double)element) && !double.IsNegativeInfinity((double)element)))
                        {
                            switch (serializer.FloatFormatHandling)
                            {
                                case FloatFormatHandling.DefaultValue:
                                    {
                                        writer.WriteNull();
                                    }
                                    break;

                                case FloatFormatHandling.Symbol:
                                    {
                                        writer.WriteRawValue(VALUE_POSITIVE_INFINITY);
                                    }
                                    break;

                                case FloatFormatHandling.String:
                                default:
                                    {
                                        writer.WriteValue(VALUE_POSITIVE_INFINITY);
                                    }
                                    break;
                            }
                        }
                        else if ((typeof(float) == elementType && float.IsNegativeInfinity((float)element)) ||
                                 (typeof(double) == elementType && double.IsNegativeInfinity((double)element)))
                        {
                            switch (serializer.FloatFormatHandling)
                            {
                                case FloatFormatHandling.DefaultValue:
                                    {
                                        writer.WriteNull();
                                    }
                                    break;

                                case FloatFormatHandling.Symbol:
                                    {
                                        writer.WriteRawValue(VALUE_NEGATIVE_INFINITY);
                                    }
                                    break;

                                case FloatFormatHandling.String:
                                default:
                                    {
                                        writer.WriteValue(VALUE_NEGATIVE_INFINITY);
                                    }
                                    break;
                            }
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
