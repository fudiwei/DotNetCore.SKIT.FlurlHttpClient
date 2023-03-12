using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SKIT.FlurlHttpClient.Configuration
{
    /// <summary>
    /// <para>用于序列化 "application/json" 内容的序列化器。</para>
    /// <para>基于 System.Text.Json.<see cref="JsonSerializer"/> 实现。</para>
    /// </summary>
    public sealed class SystemTextJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerOptions _jsonOptions;

        public SystemTextJsonSerializer()
            : this(GetDefaultSerializerOptions())
        {
        }

        public SystemTextJsonSerializer(JsonSerializerOptions options)
        {
            _jsonOptions = options ?? throw new ArgumentNullException(nameof(options));
        }

        public static JsonSerializerOptions GetDefaultSerializerOptions()
        {
            JsonSerializerOptions options = new JsonSerializerOptions(JsonSerializerOptions.Default);
            options.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            options.NumberHandling = JsonNumberHandling.AllowReadingFromString;
            options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.WriteIndented = false;
            options.PropertyNamingPolicy = null;
            options.PropertyNameCaseInsensitive = true;
            return options;
        }

        public object? Deserialize(string json, Type type)
        {
            return JsonSerializer.Deserialize(json, type, _jsonOptions);
        }

        public string Serialize(object? obj, Type type)
        {
            return JsonSerializer.Serialize(obj, type, _jsonOptions);
        }
    }
}
