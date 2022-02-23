using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Flurl.Http.Configuration;

namespace SKIT.FlurlHttpClient
{
    public class FlurlSystemTextJsonSerializer : ISerializer
    {
        private readonly JsonSerializerOptions _options;

        public FlurlSystemTextJsonSerializer()
            : this(GetDefaultSerializerOptions())
        {
        }

        public FlurlSystemTextJsonSerializer(JsonSerializerOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public static JsonSerializerOptions GetDefaultSerializerOptions()
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            options.NumberHandling = JsonNumberHandling.AllowReadingFromString;
            options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.WriteIndented = false;
            options.PropertyNamingPolicy = null;
            options.PropertyNameCaseInsensitive = true;
            return options;
        }

        public T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, _options)!;
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
            return JsonSerializer.Deserialize(json, type, _options)!;
        }

        public string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj, _options);
        }

        public string Serialize(object obj)
        {
            return JsonSerializer.Serialize(obj, _options);
        }

        public string Serialize(object obj, Type type)
        {
            return JsonSerializer.Serialize(obj, type, _options);
        }
    }
}
