namespace Newtonsoft.Json.Converters.Common
{
    public sealed class RegularDateTimeOffsetConverter : FormattedDateTimeOffsetConverterBase
    {
        protected override string FormatString
        {
            get { return "yyyy-MM-dd HH:mm:ss"; }
        }
    }
}
