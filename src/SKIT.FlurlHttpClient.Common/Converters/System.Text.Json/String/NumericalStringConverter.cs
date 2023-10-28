namespace System.Text.Json.Serialization.Common
{
    /// <summary>
    /// 一个 JSON 转换器，可针对指定适配类型做如下形式的对象转换。
    /// <para>与通过 System.Text.Json.Serialization.<see cref="JsonNumberHandling.WriteAsString"/> 参数转换相比，可支持空字符串等特殊形式。</para>
    /// <code>
    ///   .NET → string Foo { get; } = "1";
    ///   JSON → { "Foo": 1 }
    /// </code>
    /// 
    /// 适配类型：
    /// <code>  <see cref="string"/></code>
    /// </summary>
    public class NumericalStringConverter : JsonConverter<string?>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.TryGetInt32(out int valueAsInt32) ? valueAsInt32.ToString() :
                       reader.TryGetInt64(out long valueAsInt64) ? valueAsInt64.ToString() :
                       reader.TryGetUInt64(out ulong valueAsUInt64) ? valueAsUInt64.ToString() :
                       reader.TryGetDouble(out double valueAsDouble) ? valueAsDouble.ToString() :
                       reader.GetDecimal().ToString();
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                return reader.GetString();
            }

            throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
        }

        public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
            }
            else
            {
                if (string.IsNullOrEmpty(value))
                    writer.WriteStringValue(value);
                else if (long.TryParse(value, out long valueAsInt64))
                    writer.WriteNumberValue(valueAsInt64);
                else if (ulong.TryParse(value, out ulong valueAsUInt64))
                    writer.WriteNumberValue(valueAsUInt64);
                else if (decimal.TryParse(value, out decimal valueAsDecimal))
                    writer.WriteNumberValue(valueAsDecimal);
                else if (double.TryParse(value, out double valueAsDouble))
                    writer.WriteNumberValue(valueAsDouble);
                else
                    throw new JsonException($"Could not parse String '{value}' to Number.");
            }
        }
    }
}
