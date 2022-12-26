using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SKIT.FlurlHttpClient.Configuration
{
    public class NewtonsoftJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerSettings _jsonSettings;

        public NewtonsoftJsonSerializer()
            : this(GetDefaultSerializerSettings())
        {
        }

        public NewtonsoftJsonSerializer(JsonSerializerSettings settings)
        {
            _jsonSettings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

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

        public object? Deserialize(string json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type, _jsonSettings);
        }

        public string Serialize(object? obj, Type type)
        {
            return JsonConvert.SerializeObject(obj, type, _jsonSettings);
        }
    }
}
