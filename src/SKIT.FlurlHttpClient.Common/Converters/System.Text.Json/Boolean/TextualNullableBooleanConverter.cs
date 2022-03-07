﻿using System.Text.Json.Serialization;

namespace System.Text.Json.Converters
{
    public class TextualNullableBooleanConverter : JsonConverter<bool?>
    {
        public override bool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }
            else if (reader.TokenType == JsonTokenType.True)
            {
                return true;
            }
            else if (reader.TokenType == JsonTokenType.False)
            {
                return false;
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                string? value = reader.GetString();
                if (string.IsNullOrEmpty(value))
                    return null;

                if ("true".Equals(value, StringComparison.OrdinalIgnoreCase))
                    return true;
                else if ("false".Equals(value, StringComparison.OrdinalIgnoreCase))
                    return false;

                throw new JsonException($"Could not parse String '{value}' to Boolean.");
            }

            throw new JsonException($"Unexpected JSON token type '{reader.TokenType}' when reading.");
        }

        public override void Write(Utf8JsonWriter writer, bool? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteStringValue(value.Value ? "true" : "false");
            else
                writer.WriteNullValue();
        }
    }
}
