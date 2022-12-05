using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace System.Text.Json.Converters
{
    public class DynamicObjectConverter : JsonConverterFactory
    {
        private sealed class InnerDynamicObjectConverter : JsonConverter<object?>
        {
            private static readonly Hashtable _cachedJsonConverterWriteMethods = new Hashtable();
            private static readonly Hashtable _cachedTypeProperties = new Hashtable();
            private static readonly Hashtable _cachedTypeFields = new Hashtable();

            public override bool CanConvert(Type typeToConvert)
            {
                return true;
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

            private void WriteValue(ref Utf8JsonWriter writer, object? value, JsonSerializerOptions options)
            {
                bool requireWriteJsonNumberAsString = (options.NumberHandling & JsonNumberHandling.WriteAsString) != 0;

                if (value == null || Convert.IsDBNull(value))
                {
                    writer.WriteNullValue();
                    return;
                }

                Type type = value.GetType();
                JsonConverter? converter = options.GetConverter(type);
                if (converter != null && !GetType().IsAssignableFrom(converter.GetType()))
                {
                    WriteValueWithJsonConverter(ref writer, value, options, converter);
                    return;
                }

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = type.GetGenericArguments().Single();
                    converter = options.GetConverter(type);
                    if (converter != null && !GetType().IsAssignableFrom(converter.GetType()))
                    {
                        WriteValueWithJsonConverter(ref writer, value, options, converter);
                        return;
                    }
                }

                if (type == typeof(bool))
                {
                    writer.WriteBooleanValue(Convert.ToBoolean(value));
                }
                else if (type == typeof(byte))
                {
                    if (requireWriteJsonNumberAsString)
                        writer.WriteStringValue(Convert.ToByte(value).ToString());
                    else
                        writer.WriteNumberValue(Convert.ToByte(value));
                }
                else if (type == typeof(sbyte))
                {
                    if (requireWriteJsonNumberAsString)
                        writer.WriteStringValue(Convert.ToSByte(value).ToString());
                    else
                        writer.WriteNumberValue(Convert.ToSByte(value));
                }
                else if (type == typeof(short))
                {
                    if (requireWriteJsonNumberAsString)
                        writer.WriteStringValue(Convert.ToInt16(value).ToString());
                    else
                        writer.WriteNumberValue(Convert.ToInt16(value));
                }
                else if (type == typeof(ushort))
                {
                    if (requireWriteJsonNumberAsString)
                        writer.WriteStringValue(Convert.ToUInt16(value).ToString());
                    else
                        writer.WriteNumberValue(Convert.ToUInt16(value));
                }
                else if (type == typeof(int))
                {
                    if (requireWriteJsonNumberAsString)
                        writer.WriteStringValue(Convert.ToInt32(value).ToString());
                    else
                        writer.WriteNumberValue(Convert.ToInt32(value));
                }
                else if (type == typeof(uint))
                {
                    if (requireWriteJsonNumberAsString)
                        writer.WriteStringValue(Convert.ToUInt32(value).ToString());
                    else
                        writer.WriteNumberValue(Convert.ToUInt32(value));
                }
                else if (type == typeof(long))
                {
                    if (requireWriteJsonNumberAsString)
                        writer.WriteStringValue(Convert.ToInt64(value).ToString());
                    else
                        writer.WriteNumberValue(Convert.ToInt64(value));
                }
                else if (type == typeof(ulong))
                {
                    if (requireWriteJsonNumberAsString)
                        writer.WriteStringValue(Convert.ToUInt64(value).ToString());
                    else
                        writer.WriteNumberValue(Convert.ToUInt64(value));
                }
                else if (type == typeof(char))
                {
                    if (requireWriteJsonNumberAsString)
                        writer.WriteStringValue(Convert.ToChar(value).ToString());
                    else
                        writer.WriteNumberValue(Convert.ToChar(value));
                }
                else if (type == typeof(float))
                {
                    if (requireWriteJsonNumberAsString)
                        writer.WriteStringValue(Convert.ToSingle(value).ToString());
                    else
                        writer.WriteNumberValue(Convert.ToSingle(value));
                }
                else if (type == typeof(double))
                {
                    if (requireWriteJsonNumberAsString)
                        writer.WriteStringValue(Convert.ToDouble(value).ToString());
                    else
                        writer.WriteNumberValue(Convert.ToDouble(value));
                }
                else if (type == typeof(decimal))
                {
                    if (requireWriteJsonNumberAsString)
                        writer.WriteStringValue(Convert.ToDecimal(value).ToString());
                    else
                        writer.WriteNumberValue(Convert.ToDecimal(value));
                }
                else if (type == typeof(string))
                {
                    writer.WriteStringValue(Convert.ToString(value));
                }
                else if (type == typeof(Guid))
                {
                    writer.WriteStringValue((Guid)value);
                }
                else if (type == typeof(DateTime))
                {
                    writer.WriteStringValue((DateTime)value);
                }
                else if (type == typeof(DateTimeOffset))
                {
                    writer.WriteStringValue((DateTimeOffset)value);
                }
                else if (type == typeof(JsonEncodedText))
                {
                    writer.WriteStringValue((JsonEncodedText)value);
                }
                else if (type.IsArray)
                {
                    WriteArray(ref writer, (Array)value, options);
                }
                else if (type is IEnumerable && !(type is IDictionary))
                {
                    WriteArray(ref writer, (IEnumerable)type, options);
                }
                else if (type.IsPrimitive)
                {
                    writer.WriteStringValue(value.ToString());
                }
                else
                {
                    WriteObject(ref writer, value, options);
                }
            }

            private void WriteValueWithJsonConverter(ref Utf8JsonWriter writer, object value, JsonSerializerOptions options, JsonConverter converter)
            {
                Type converterType = converter.GetType();
                string converterMKey = converterType.AssemblyQualifiedName ?? converterType.GetHashCode().ToString();
                MethodInfo? converterMethod = (MethodInfo?)_cachedJsonConverterWriteMethods[converterMKey];
                if (converterMethod == null)
                {
                    converterMethod = converter.GetType().GetMethod(nameof(this.Write), BindingFlags.Public | BindingFlags.Instance)!;
                    _cachedJsonConverterWriteMethods[converterMKey] = converterMethod;
                }

                converterMethod.Invoke(converter, new object[] { writer, value, options });
            }

            private void WriteObject(ref Utf8JsonWriter writer, object value, JsonSerializerOptions options)
            {
                Type type = value.GetType();

                writer.WriteStartObject();

                if (value is IDictionary dict)
                {
                    foreach (DictionaryEntry entry in dict)
                    {
                        string entryKey = options.DictionaryKeyPolicy?.ConvertName(entry.Key.ToString()!) ?? entry.Key.ToString()!;
                        object? entryValue = entry.Value;

                        if (entryValue == null)
                        {
                            if (options.DefaultIgnoreCondition == JsonIgnoreCondition.Always ||
                                options.DefaultIgnoreCondition == JsonIgnoreCondition.WhenWritingNull)
                                continue;
                        }

                        writer.WritePropertyName(entryKey);
                        WriteValue(ref writer, entryValue, options);
                    }
                }
                else
                {
                    if (true)
                    {
                        string cacheKey = type.AssemblyQualifiedName ?? type.GetHashCode().ToString();
                        PropertyInfo[]? properties = (PropertyInfo[]?)_cachedTypeProperties[cacheKey];
                        if (properties == null)
                        {
                            properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                .Where(p =>
                                {
                                    if (!p.CanRead)
                                        return false;
                                    if (!p.CanWrite && (options.IgnoreReadOnlyProperties || p.GetCustomAttribute<JsonIncludeAttribute>() == null))
                                        return false;
                                    if (!p.SetMethod!.IsPublic && (options.IgnoreReadOnlyProperties || p.GetCustomAttribute<JsonIncludeAttribute>() == null))
                                        return false;
                                    if (p.GetCustomAttribute<JsonIgnoreAttribute>() != null)
                                        return false;
                                    if (p.GetCustomAttribute<JsonPropertyNameAttribute>() == null || p.GetCustomAttribute<JsonIncludeAttribute>(inherit: true) != null)
                                        return false;

                                    return true;
                                })
                                .OrderBy(p => p.GetCustomAttribute<JsonPropertyOrderAttribute>(inherit: true)?.Order ?? 0)
                                .ToArray();
                            _cachedTypeProperties[cacheKey] = properties;
                        }

                        foreach (PropertyInfo property in properties)
                        {
                            string propertyKey = property.GetCustomAttribute<JsonPropertyNameAttribute>(inherit: true)?.Name ?? property.Name;
                            object? propertyValue = property.GetValue(value);

                            if (propertyValue == null)
                            {
                                if (options.DefaultIgnoreCondition == JsonIgnoreCondition.Always ||
                                    options.DefaultIgnoreCondition == JsonIgnoreCondition.WhenWritingNull)
                                {
                                    continue;
                                }
                            }

                            writer.WritePropertyName(propertyKey);

                            JsonConverterAttribute? jsonConverterAttribute = property.GetCustomAttribute<JsonConverterAttribute>(inherit: true);
                            if (jsonConverterAttribute == null)
                            {
                                WriteValue(ref writer, propertyValue, options);
                            }
                            else
                            {
                                JsonConverter jsonConverter = jsonConverterAttribute.CreateConverter(property.PropertyType) ?? (JsonConverter)Activator.CreateInstance(jsonConverterAttribute.ConverterType!)!;
                                WriteValueWithJsonConverter(ref writer, value, options, jsonConverter);
                            }
                        }
                    }

                    if (options.IncludeFields)
                    {
                        string cacheKey = type.AssemblyQualifiedName ?? type.GetHashCode().ToString();
                        FieldInfo[]? fields = (FieldInfo[]?)_cachedTypeFields[cacheKey];
                        if (fields == null)
                        {
                            fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                                .Where(f =>
                                {
                                    if (!f.IsPublic && (options.IgnoreReadOnlyFields || f.GetCustomAttribute<JsonIncludeAttribute>() == null))
                                        return false;
                                    if (f.GetCustomAttribute<JsonIgnoreAttribute>() != null)
                                        return false;
                                    if (f.GetCustomAttribute<JsonPropertyNameAttribute>() == null || f.GetCustomAttribute<JsonIncludeAttribute>(inherit: true) != null)
                                        return false;

                                    return true;
                                })
                                .OrderBy(p => p.GetCustomAttribute<JsonPropertyOrderAttribute>(inherit: true)?.Order ?? 0)
                                .ToArray();
                            _cachedTypeFields[cacheKey] = fields;
                        }

                        foreach (FieldInfo field in fields)
                        {
                            string fieldKey = field.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? field.Name;
                            object? fieldValue = field.GetValue(value);

                            if (fieldValue == null)
                            {
                                if (options.DefaultIgnoreCondition == JsonIgnoreCondition.Always ||
                                    options.DefaultIgnoreCondition == JsonIgnoreCondition.WhenWritingNull)
                                {
                                    continue;
                                }
                            }

                            writer.WritePropertyName(fieldKey);

                            JsonConverterAttribute? jsonConverterAttribute = field.GetCustomAttribute<JsonConverterAttribute>(inherit: true);
                            if (jsonConverterAttribute == null)
                            {
                                WriteValue(ref writer, fieldValue, options);
                            }
                            else
                            {
                                JsonConverter jsonConverter = jsonConverterAttribute.CreateConverter(field.FieldType) ?? (JsonConverter)Activator.CreateInstance(jsonConverterAttribute.ConverterType!)!;
                                WriteValueWithJsonConverter(ref writer, value, options, jsonConverter);
                            }
                        }
                    }
                }

                writer.WriteEndObject();
            }

            private void WriteArray(ref Utf8JsonWriter writer, IEnumerable value, JsonSerializerOptions options)
            {
                Type type = value.GetType();
                if (type.IsArray && type.GetElementType() == typeof(byte))
                {
                    writer.WriteBase64StringValue((byte[])value);
                }
                else
                {
                    writer.WriteStartArray();

                    foreach (object item in value)
                    {
                        WriteValue(ref writer, item, options);
                    }

                    writer.WriteEndArray();
                }
            }
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return
                typeof(object) == typeToConvert ||
                typeof(ExpandoObject) == typeToConvert ||
                typeof(IDynamicMetaObjectProvider).IsAssignableFrom(typeToConvert);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new InnerDynamicObjectConverter();
        }
    }
}
