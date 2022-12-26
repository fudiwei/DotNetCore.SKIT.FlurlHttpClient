using System;
using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json.Converters.Common
{
    public abstract partial class TextualStringListWithSplitConverterBase : JsonConverter
    {
        protected abstract string Separator { get; }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsGenericType &&
                   typeof(IList<>).IsAssignableFrom(objectType.GetGenericTypeDefinition()) &&
                   typeof(string) == objectType.GetGenericArguments()[0];
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            JsonConverter<IList<string>?> converter = new InternalTextualStringListWithSplitConverter(Separator);
            return converter.ReadJson(reader, objectType, (IList<string>?)existingValue, (IList<string>?)existingValue != null, serializer);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            JsonConverter<IList<string>?> converter = new InternalTextualStringListWithSplitConverter(Separator);
            converter.WriteJson(writer, (IList<string>?)value, serializer);
        }
    }

    partial class TextualStringListWithSplitConverterBase
    {
        private sealed class InternalTextualStringArrayWithSplitConverter : TextualStringArrayWithSplitConverterBase
        {
            protected override string Separator { get; }

            public InternalTextualStringArrayWithSplitConverter(string separator)
            {
                Separator = separator;
            }

            public override string[]? ReadJson(JsonReader reader, Type objectType, string[]? existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return base.ReadJson(reader, objectType, existingValue, hasExistingValue, serializer);
            }

            public override void WriteJson(JsonWriter writer, string[]? value, JsonSerializer serializer)
            {
                base.WriteJson(writer, value, serializer);
            }
        }

        private sealed class InternalTextualStringListWithSplitConverter : JsonConverter<IList<string>?>
        {
            private readonly JsonConverter<string[]?> _converter;

            public InternalTextualStringListWithSplitConverter(string separator)
            {
                _converter = new InternalTextualStringArrayWithSplitConverter(separator);
            }

            public override IList<string>? ReadJson(JsonReader reader, Type objectType, IList<string>? existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return _converter.ReadJson(reader, typeof(string[]), existingValue?.ToArray(), hasExistingValue, serializer);
            }

            public override void WriteJson(JsonWriter writer, IList<string>? value, JsonSerializer serializer)
            {
                _converter.WriteJson(writer, value?.ToArray(), serializer);
            }
        }
    }
}
