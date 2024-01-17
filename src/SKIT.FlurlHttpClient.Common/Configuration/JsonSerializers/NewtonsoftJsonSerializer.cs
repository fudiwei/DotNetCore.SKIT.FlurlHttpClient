using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SKIT.FlurlHttpClient
{
    /// <summary>
    /// <para>用于序列化 "application/json" 内容的序列化器。</para>
    /// <para>基于 Newtonsoft.Json.<see cref="JsonSerializer"/> 实现。</para>
    /// </summary>
    public sealed class NewtonsoftJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerSettings _jsonSettings;

        /// <summary>
        /// 
        /// </summary>
        public NewtonsoftJsonSerializer()
            : this(GetDefaultSerializerSettings())
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public NewtonsoftJsonSerializer(JsonSerializerSettings settings)
        {
            _jsonSettings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        /// <summary>
        /// 获取 SKIT.FlurlHttpClient 默认使用的 <see cref="JsonSerializerSettings"/> 实例。
        /// </summary>
        /// <returns></returns>
        public static JsonSerializerSettings GetDefaultSerializerSettings()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            settings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Formatting = Formatting.None;
            settings.ContractResolver = new DefaultContractResolver();
            return settings;
        }

        /// <inheritdoc/>
        public object? Deserialize(string json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type, _jsonSettings);
        }

        /// <inheritdoc/>
        public string Serialize(object? obj, Type type)
        {
            return JsonConvert.SerializeObject(obj, type, _jsonSettings);
        }
    }
}
