using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases
{
    public class TestCase_ExtensionNewtonsoftJsonSerializerExtensionsTest
    {
        [Fact(DisplayName = "测试用例：扩展 `NewtonsoftJsonSerializerExtensions.ExtractSerializerSettings`")]
        public void TestExtractSerializerSettings()
        {
            var contractResolver = new DefaultContractResolver();
            var traceWriter = new MemoryTraceWriter();
            var converter = new Newtonsoft.Json.Converters.StringEnumConverter();
            var culture = CultureInfo.GetCultureInfo("zh-CN");
            var settings = new JsonSerializerSettings
            {
                Culture = culture,
                ContractResolver = contractResolver,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                CheckAdditionalContent = true,
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                DateFormatString = "yyyyMMdd",
                DateParseHandling = DateParseHandling.DateTimeOffset,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                DefaultValueHandling = DefaultValueHandling.Populate,
                FloatFormatHandling = FloatFormatHandling.String,
                Formatting = Formatting.Indented,
                FloatParseHandling = FloatParseHandling.Decimal,
                MaxDepth = 16,
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Error,
                NullValueHandling = NullValueHandling.Include,
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                TraceWriter = traceWriter,
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            };
            settings.Converters.Add(converter);

            JsonSerializer serializer = JsonSerializer.Create(settings);
            JsonSerializerSettings extractedSettings = serializer.ExtractSerializerSettings();

            Assert.Equal(culture, extractedSettings.Culture);
            Assert.Same(contractResolver, extractedSettings.ContractResolver);
            Assert.Equal(ConstructorHandling.AllowNonPublicDefaultConstructor, extractedSettings.ConstructorHandling);
            Assert.True(extractedSettings.CheckAdditionalContent);
            Assert.Equal(DateFormatHandling.MicrosoftDateFormat, extractedSettings.DateFormatHandling);
            Assert.Equal("yyyyMMdd", extractedSettings.DateFormatString);
            Assert.Equal(DateParseHandling.DateTimeOffset, extractedSettings.DateParseHandling);
            Assert.Equal(DateTimeZoneHandling.Utc, extractedSettings.DateTimeZoneHandling);
            Assert.Equal(DefaultValueHandling.Populate, extractedSettings.DefaultValueHandling);
            Assert.Equal(FloatFormatHandling.String, extractedSettings.FloatFormatHandling);
            Assert.Equal(Formatting.Indented, extractedSettings.Formatting);
            Assert.Equal(FloatParseHandling.Decimal, extractedSettings.FloatParseHandling);
            Assert.Equal(16, extractedSettings.MaxDepth);
            Assert.Equal(MetadataPropertyHandling.Ignore, extractedSettings.MetadataPropertyHandling);
            Assert.Equal(MissingMemberHandling.Error, extractedSettings.MissingMemberHandling);
            Assert.Equal(NullValueHandling.Include, extractedSettings.NullValueHandling);
            Assert.Equal(ObjectCreationHandling.Replace, extractedSettings.ObjectCreationHandling);
            Assert.Equal(PreserveReferencesHandling.Objects, extractedSettings.PreserveReferencesHandling);
            Assert.Equal(ReferenceLoopHandling.Ignore, extractedSettings.ReferenceLoopHandling);
            Assert.Equal(StringEscapeHandling.EscapeHtml, extractedSettings.StringEscapeHandling);
            Assert.Same(traceWriter, extractedSettings.TraceWriter);
            Assert.Equal(TypeNameHandling.Objects, extractedSettings.TypeNameHandling);
            Assert.Equal(TypeNameAssemblyFormatHandling.Simple, extractedSettings.TypeNameAssemblyFormatHandling);
            Assert.Contains(converter, extractedSettings.Converters);
        }

        [Fact(DisplayName = "测试用例：扩展 `NewtonsoftJsonSerializerExtensions.ExtractSerializerSettings` 之 Error 事件")]
        public void TestExtractSerializerSettings_ErrorEvent()
        {
            var serializer = new JsonSerializer();
            int firstHandlerCallCount = 0;
            int secondHandlerCallCount = 0;
            serializer.Error += (_, _) => firstHandlerCallCount++;
            serializer.Error += (_, _) => secondHandlerCallCount++;

            JsonSerializerSettings settings = serializer.ExtractSerializerSettings();

            Assert.NotNull(settings.Error);
            settings.Error!(serializer, null!);

            Assert.Equal(1, firstHandlerCallCount);
            Assert.Equal(1, secondHandlerCallCount);
        }

        [Fact(DisplayName = "测试用例：扩展 `NewtonsoftJsonSerializerExtensions.ExtractSerializerSettings` 之空入参")]
        public void TestExtractSerializerSettings_NullSerializer()
        {
            JsonSerializerSettings settings = NewtonsoftJsonSerializerExtensions.ExtractSerializerSettings(null!);

            Assert.NotNull(settings);
            Assert.Null(settings.Error);
        }
    }
}
