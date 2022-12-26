using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace System.Text.Json.Converters.Common
{
    public partial class DynamicObjectConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(object) == typeToConvert ||
                   typeof(ExpandoObject) == typeToConvert ||
                   typeof(IDynamicMetaObjectProvider).IsAssignableFrom(typeToConvert);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new InternalDynamicObjectConverter(typeToConvert);
        }
    }

    partial class DynamicObjectConverter
    {
        private sealed class InternalDynamicObjectConverter : JsonConverter<object?>
        {
            private readonly Type _convertType;

            public InternalDynamicObjectConverter(Type convertType)
            {
                _convertType = convertType;
            }

            public override bool CanConvert(Type typeToConvert)
            {
                return _convertType == typeToConvert ||
                       _convertType.IsAssignableFrom(typeToConvert) ||
                       typeToConvert.IsSubclassOf(_convertType);
            }

            public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return ReadValue(ref reader, options);
            }

            public override void Write(Utf8JsonWriter writer, object? value, JsonSerializerOptions options)
            {
                WriteValue(ref writer, value, options);
            }

            private object? ReadValue(ref Utf8JsonReader reader, JsonSerializerOptions options)
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.Null:
                        return null;

                    case JsonTokenType.True:
                        return true;

                    case JsonTokenType.False:
                        return false;

                    case JsonTokenType.Number:
                        return reader.TryGetInt64(out long valueAsInt64) ? valueAsInt64 : reader.GetDouble();

                    case JsonTokenType.String:
                        return reader.GetString();

                    case JsonTokenType.StartObject:
                        return ReadObject(ref reader, options);

                    case JsonTokenType.StartArray:
                        return ReadArray(ref reader, options);

                    case JsonTokenType.Comment:
                        {
                            if (options.ReadCommentHandling == JsonCommentHandling.Disallow)
                                throw new JsonException($"JSON comment is disallowed.");
                            return null;
                        }

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
                            {
                                if (options.ReadCommentHandling == JsonCommentHandling.Disallow)
                                    throw new JsonException($"JSON comment is disallowed.");
                            }
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
                        case JsonTokenType.EndArray:
                            return list.ToArray();

                        case JsonTokenType.Comment:
                            {
                                if (options.ReadCommentHandling == JsonCommentHandling.Disallow)
                                    throw new JsonException($"JSON comment is disallowed.");
                            }
                            break;

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

            private void WriteValue(ref Utf8JsonWriter writer, object? value, JsonSerializerOptions options)
            {
                if (value is null || Convert.IsDBNull(value))
                {
                    writer.WriteNullValue();
                }
                else
                {
                    Type convertType = value.GetType();
                    JsonSerializer.Serialize(writer, value, convertType, options);
                }
            }
        }
    }
}
