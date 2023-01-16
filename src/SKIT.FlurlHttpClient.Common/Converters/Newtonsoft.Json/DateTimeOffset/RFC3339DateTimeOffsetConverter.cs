namespace Newtonsoft.Json.Converters.Common
{
    public sealed class RFC3339DateTimeOffsetConverter : FormattedDateTimeOffsetConverterBase
    {
        protected override string FormatString
        {
            get { return "yyyy-MM-dd'T'HH:mm:sszzz"; }
        }
    }
}
