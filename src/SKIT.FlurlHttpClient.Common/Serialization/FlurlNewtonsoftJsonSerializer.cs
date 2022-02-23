using System;
using System.IO;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SKIT.FlurlHttpClient
{
    public class FlurlNewtonsoftJsonSerializer : ISerializer
    {
        private readonly JsonSerializerSettings _settings;

        public FlurlNewtonsoftJsonSerializer()
            : this(GetDefaultSerializerSettings())
        {
        }

        public FlurlNewtonsoftJsonSerializer(JsonSerializerSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
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

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _settings)!;
        }

        T ISerializer.Deserialize<T>(Stream stream)
        {
            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            using TextReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();
            return Deserialize<T>(json);
        }

        public object Deserialize(string json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type, _settings)!;
        }

        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, _settings);
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, _settings);
        }

        public string Serialize(object obj, Type type)
        {
            return JsonConvert.SerializeObject(obj, type, _settings);
        }
    }
}
