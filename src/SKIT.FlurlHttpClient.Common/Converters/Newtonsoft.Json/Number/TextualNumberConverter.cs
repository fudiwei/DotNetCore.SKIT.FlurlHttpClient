using System;

namespace Newtonsoft.Json.Converters.Common
{
    using SKIT.FlurlHttpClient.Converters.Internal;

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
