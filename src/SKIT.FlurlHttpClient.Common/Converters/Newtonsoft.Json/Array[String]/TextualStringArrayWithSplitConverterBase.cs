using System;

namespace Newtonsoft.Json.Converters.Common
{
    public abstract class TextualStringArrayWithSplitConverterBase : JsonConverter<string[]?>
    {
        protected abstract string Separator { get; }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override string[]? ReadJson(JsonReader reader, Type objectType, string[]? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            else if (reader.TokenType == JsonToken.String)
            {
                string? value = serializer.Deserialize<string>(reader);
                if (value == null)
                    return null;
                if (value == string.Empty)
                    return Array.Empty<string>();

#if NET5_0_OR_GREATER
                return value.Split(Separator);
#else
                return value.Split(new string[] { Separator }, StringSplitOptions.None);
#endif
            }

            throw new JsonSerializationException($"Unexpected token type '{reader.TokenType}' when deserializing. Path '{reader.Path}'.");
        }

        public override void WriteJson(JsonWriter writer, string[]? value, JsonSerializer serializer)
        {
            if (value is null)
                writer.WriteNull();
            else
                writer.WriteValue(string.Join(Separator, value));
        }
    }
}
