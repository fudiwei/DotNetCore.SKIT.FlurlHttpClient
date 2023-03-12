using System;

namespace Newtonsoft.Json.Converters.Common
{
    /// <summary>
    /// 一个 JSON 转换器，可针对指定适配类型做如下形式的对象转换。
    /// <code>
    ///   .NET → string Foo { get; } = "1";
    ///   JSON → { "Foo": 1 }
    /// </code>
    /// 
    /// 适配类型：
    /// <code>  <see cref="string"/></code>
    /// </summary>
    public sealed class NumericalStringConverter : JsonConverter<string?>
    {
        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override string? ReadJson(JsonReader reader, Type objectType, string? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return existingValue;
            }
            else if (reader.TokenType == JsonToken.Integer)
            {
                try
                {
                    long value = serializer.Deserialize<long>(reader);
                    return value.ToString();
                }
                catch { }

                try
                {
                    ulong value = serializer.Deserialize<ulong>(reader);
                    return value.ToString();
                }
                catch { }

                return serializer.Deserialize<string>(reader);
            }
            else if (reader.TokenType == JsonToken.Float)
            {
                switch (serializer.FloatParseHandling)
                {
                    case FloatParseHandling.Decimal:
                        {
                            decimal value = serializer.Deserialize<decimal>(reader);
                            return value.ToString();
                        }

                    case FloatParseHandling.Double:
                    default:
                        {
                            double value = serializer.Deserialize<double>(reader);
                            return value.ToString();
                        }
                }
            }
            else if (reader.TokenType == JsonToken.String)
            {
                return serializer.Deserialize<string>(reader);
            }

            throw new JsonSerializationException($"Unexpected token type '{reader.TokenType}' when deserializing. Path '{reader.Path}'.");
        }

        public override void WriteJson(JsonWriter writer, string? value, JsonSerializer serializer)
        {
            if (value is null)
            {
                writer.WriteNull();
            }
            else
            {
                if (string.IsNullOrEmpty(value))
                    writer.WriteValue(value);
                else if (long.TryParse(value, out long valueAsInt64))
                    writer.WriteValue(valueAsInt64);
                else if (ulong.TryParse(value, out ulong valueAsUInt64))
                    writer.WriteValue(valueAsUInt64);
                else if (decimal.TryParse(value, out decimal valueAsDecimal))
                    writer.WriteValue(valueAsDecimal);
                else if (double.TryParse(value, out double valueAsDouble))
                    writer.WriteValue(valueAsDouble);
                else
                    throw new JsonSerializationException($"Could not parse String '{value}' to Number.");
            }
        }
    }
}
