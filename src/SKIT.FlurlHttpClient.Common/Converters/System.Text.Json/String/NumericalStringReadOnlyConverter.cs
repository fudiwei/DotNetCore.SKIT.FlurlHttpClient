using System.Text.Json.Serialization;

namespace System.Text.Json.Converters
{
    public class NumericalStringReadOnlyConverter : JsonConverter<string?>
    {
        private readonly JsonConverter<string?> _converter = new NumericalStringConverter();

        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return _converter.Read(ref reader, typeToConvert, options);
        }

        public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
        {
            if (value != null)
                writer.WriteStringValue(value);
            else
                writer.WriteNullValue();
        }
    }
}
