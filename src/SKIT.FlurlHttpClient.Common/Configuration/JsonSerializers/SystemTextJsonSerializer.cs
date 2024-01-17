using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SKIT.FlurlHttpClient
{
    /// <summary>
    /// <para>用于序列化 "application/json" 内容的序列化器。</para>
    /// <para>基于 System.Text.Json.<see cref="JsonSerializer"/> 实现。</para>
    /// </summary>
    public sealed class SystemTextJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerOptions _jsonOptions;

        /// <summary>
        /// 
        /// </summary>
        public SystemTextJsonSerializer()
            : this(GetDefaultSerializerOptions())
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SystemTextJsonSerializer(JsonSerializerOptions options)
        {
            _jsonOptions = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// 获取 SKIT.FlurlHttpClient 默认使用的 <see cref="JsonSerializerOptions"/> 实例。
        /// </summary>
        /// <returns></returns>
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

        /// <inheritdoc/>
        public object? Deserialize(string json, Type type)
        {
            return JsonSerializer.Deserialize(json, type, _jsonOptions);
        }

        /// <inheritdoc/>
        public string Serialize(object? obj, Type type)
        {
            return JsonSerializer.Serialize(obj, type, _jsonOptions);
        }
    }
}
