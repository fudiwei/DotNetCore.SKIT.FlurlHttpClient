using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace System.Text.Json.Converters
{
    public class DynamicObjectReadOnlyConverter : JsonConverter<dynamic?>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return base.CanConvert(typeToConvert) || typeof(IDynamicMetaObjectProvider).IsAssignableFrom(typeToConvert);
        }

        public override dynamic? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return ReadValue(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, dynamic? value, JsonSerializerOptions options)
        {
        }

        private object? ReadValue(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.None:
                case JsonTokenType.Null:
                    return null;

                case JsonTokenType.True:
                    return true;

                case JsonTokenType.False:
                    return false;

                case JsonTokenType.Number:
                    return reader.TryGetInt64(out long longValue) ? longValue : reader.GetDouble();

                case JsonTokenType.String:
                    return reader.GetString();

                case JsonTokenType.StartObject:
                    return ReadObject(ref reader, options);

                case JsonTokenType.StartArray:
                    return ReadArray(ref reader, options);

                default:
                    return JsonNode.Parse(ref reader, new JsonNodeOptions() { PropertyNameCaseInsensitive = options.PropertyNameCaseInsensitive });
            }
        }

        private object? ReadObject(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            IDictionary<string, object?> expandoObject = new ExpandoObject();

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.PropertyName:
                        {
                            string key = reader.GetString()!;
                            if (!reader.Read())
                            {
                                throw new JsonException("Unexpected end when reading ExpandoObject.");
                            }

                            object? value = ReadValue(ref reader, options);
                            expandoObject[key] = value;
                        }
                        break;

                    case JsonTokenType.Comment:
                        break;

                    case JsonTokenType.EndObject:
                        return expandoObject;
                }
            }

            throw new JsonException("Unexpected end when reading ExpandoObject.");
        }

        private object? ReadArray(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            IList<object?> list = new List<object?>();

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.Comment:
                        break;

                    case JsonTokenType.EndArray:
                        return list.ToArray();

                    default:
                        {
                            object? element = ReadValue(ref reader, options);
                            list.Add(element);
                        }
                        break;
                }
            }

            throw new JsonException("Unexpected end when reading ExpandoObject.");
        }
    }
}
