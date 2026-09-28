using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Newtonsoft.Json
{
    public static class NewtonsoftJsonSerializerExtensions
    {
        private static readonly FieldInfo? _newtonsoftErrorEventField = typeof(JsonSerializer).GetField(
            nameof(JsonSerializer.Error),
            BindingFlags.Instance | BindingFlags.NonPublic
        );
        private static readonly Func<JsonSerializer, EventHandler<Serialization.ErrorEventArgs>?>? _newtonsoftErrorEventGetter;

        static NewtonsoftJsonSerializerExtensions()
        {
            if (_newtonsoftErrorEventField is null) return;

            try
            {
                ParameterExpression serializerParameter = Expression.Parameter(typeof(JsonSerializer), "serializer");
                MemberExpression errorEventField = Expression.Field(serializerParameter, _newtonsoftErrorEventField);
                UnaryExpression convertExpression = Expression.Convert(errorEventField, typeof(EventHandler<Serialization.ErrorEventArgs>));
                _newtonsoftErrorEventGetter = Expression.Lambda<Func<JsonSerializer, EventHandler<Serialization.ErrorEventArgs>?>>(convertExpression, serializerParameter).Compile();
            }
            catch
            {
            }
        }

        public static JsonSerializerSettings ExtractSerializerSettings(this JsonSerializer serializer)
        {
            if (serializer is null)
                return new JsonSerializerSettings();

            return new JsonSerializerSettings
            {
#pragma warning disable CS0618
                Binder = serializer.Binder,
#pragma warning restore CS0618
                Context = serializer.Context,
                Culture = serializer.Culture,
                ContractResolver = serializer.ContractResolver,
                ConstructorHandling = serializer.ConstructorHandling,
                Converters = serializer.Converters,
                CheckAdditionalContent = serializer.CheckAdditionalContent,
                DateFormatHandling = serializer.DateFormatHandling,
                DateFormatString = serializer.DateFormatString,
                DateParseHandling = serializer.DateParseHandling,
                DateTimeZoneHandling = serializer.DateTimeZoneHandling,
                DefaultValueHandling = serializer.DefaultValueHandling,
                EqualityComparer = serializer.EqualityComparer,
                Error = _newtonsoftErrorEventGetter is null ? _newtonsoftErrorEventField?.GetValue(serializer) as EventHandler<Serialization.ErrorEventArgs> : _newtonsoftErrorEventGetter(serializer),
                FloatFormatHandling = serializer.FloatFormatHandling,
                Formatting = serializer.Formatting,
                FloatParseHandling = serializer.FloatParseHandling,
                MaxDepth = serializer.MaxDepth,
                MetadataPropertyHandling = serializer.MetadataPropertyHandling,
                MissingMemberHandling = serializer.MissingMemberHandling,
                NullValueHandling = serializer.NullValueHandling,
                ObjectCreationHandling = serializer.ObjectCreationHandling,
                PreserveReferencesHandling = serializer.PreserveReferencesHandling,
                ReferenceLoopHandling = serializer.ReferenceLoopHandling,
#pragma warning disable CS0618
                ReferenceResolver = serializer.ReferenceResolver,
#pragma warning restore CS0618
                ReferenceResolverProvider = () => serializer.ReferenceResolver,
                StringEscapeHandling = serializer.StringEscapeHandling,
                TraceWriter = serializer.TraceWriter,
                TypeNameHandling = serializer.TypeNameHandling,
                SerializationBinder = serializer.SerializationBinder,
#pragma warning disable CS0618
                TypeNameAssemblyFormat = serializer.TypeNameAssemblyFormat,
#pragma warning restore CS0618
                TypeNameAssemblyFormatHandling = serializer.TypeNameAssemblyFormatHandling
            };
        }
    }
}
