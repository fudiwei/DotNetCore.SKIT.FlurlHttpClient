using System;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer.Helpers
{
    public static class JsonHelper
    {
        private static IJsonSerializer GetNewtonsoftJsonSerializer()
        {
            var settings = NewtonsoftJsonSerializer.GetDefaultSerializerSettings();
            settings.CheckAdditionalContent = true;
            settings.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Error;
            return new NewtonsoftJsonSerializer(settings);
        }

        private static IJsonSerializer GetSystemJsonSerializer()
        {
            var options = SystemTextJsonSerializer.GetDefaultSerializerOptions();
            options.AllowDuplicateProperties = true;
            options.AllowTrailingCommas = false;
            options.UnmappedMemberHandling = System.Text.Json.Serialization.JsonUnmappedMemberHandling.Disallow;
            return new SystemTextJsonSerializer(options);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool TryDeserialize(string json, Type type, out Exception error)
        {
            error = default!;

            try
            {
                GetNewtonsoftJsonSerializer().Deserialize(json, type);
                GetSystemJsonSerializer().Deserialize(json, type);
            }
            catch (Exception ex)
            {
                error = new Exception($"An unexpected exception was thrown when JSON deserialize '{type}'.", ex);
                return false;
            }

            return error is null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool TrySerialize(object obj, Type type, out Exception error)
        {
            error = default!;

            try
            {
                if (!type.IsAssignableFrom(obj.GetType()))
                    throw new InvalidCastException();

                GetNewtonsoftJsonSerializer().Serialize(obj);
                GetSystemJsonSerializer().Serialize(obj);
            }
            catch (Exception ex)
            {
                error = new Exception($"An unexpected exception was thrown when JSON serialize '{type}'.", ex);
                return false;
            }

            return error is null;
        }
    }
}
