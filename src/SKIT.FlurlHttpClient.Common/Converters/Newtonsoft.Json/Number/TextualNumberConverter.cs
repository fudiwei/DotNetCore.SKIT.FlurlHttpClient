using System;

namespace Newtonsoft.Json.Converters.Common
{
    using SKIT.FlurlHttpClient.Internal;

    /// <summary>
    /// 一个 JSON 转换器，可针对指定适配类型做如下形式的对象转换。
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
    public sealed partial class TextualNumberConverter : JsonConverter
    {
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
            return TypeHelper.IsNumberType(objectType);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return existingValue;
            }
            else if (reader.TokenType == JsonToken.Integer ||
                     reader.TokenType == JsonToken.Float ||
                     reader.TokenType == JsonToken.String)
            {
                if (reader.TokenType == JsonToken.String)
                {
                    string? str = serializer.Deserialize<string>(reader);
                    if (string.IsNullOrEmpty(str))
                        return default;
                }

                Type convertType = Nullable.GetUnderlyingType(objectType) ?? objectType;
                switch (Type.GetTypeCode(convertType))
                {
                    case TypeCode.SByte:
                        return serializer.Deserialize<sbyte>(reader);

                    case TypeCode.Byte:
                        return serializer.Deserialize<byte>(reader);

                    case TypeCode.Int16:
                        return serializer.Deserialize<short>(reader);

                    case TypeCode.UInt16:
                        return serializer.Deserialize<ushort>(reader);

                    case TypeCode.Int32:
                        return serializer.Deserialize<int>(reader);

                    case TypeCode.UInt32:
                        return serializer.Deserialize<uint>(reader);

                    case TypeCode.Int64:
                        return serializer.Deserialize<long>(reader);

                    case TypeCode.UInt64:
                        return serializer.Deserialize<ulong>(reader);

                    case TypeCode.Single:
                        return serializer.Deserialize<float>(reader);

                    case TypeCode.Double:
                        return serializer.Deserialize<double>(reader);

                    case TypeCode.Decimal:
                        return serializer.Deserialize<decimal>(reader);

                    default:
                        throw new NotSupportedException($"Could not convert type '{convertType}' to a number.");
                }
            }

            throw new JsonSerializationException($"Unexpected token type '{reader.TokenType}' when deserializing. Path '{reader.Path}'.");
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is null)
                writer.WriteNull();
            else
                writer.WriteValue(value.ToString());
        }
    }
}
