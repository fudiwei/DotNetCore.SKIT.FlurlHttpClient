using System.Text.Json.Serialization;

namespace System.Text.Json.Converters
{
    public class NumericalNullableBooleanReadOnlyConverter : JsonConverter<bool?>
    {
        private readonly JsonConverter<bool?> _converter = new NumericalNullableBooleanConverter();

        public override bool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return _converter.Read(ref reader, typeToConvert, options);
        }

        public override void Write(Utf8JsonWriter writer, bool? value, JsonSerializerOptions options)
        {
            if (value != null)
                writer.WriteBooleanValue(value.Value);
            else
                writer.WriteNullValue();
        }
    }
}
