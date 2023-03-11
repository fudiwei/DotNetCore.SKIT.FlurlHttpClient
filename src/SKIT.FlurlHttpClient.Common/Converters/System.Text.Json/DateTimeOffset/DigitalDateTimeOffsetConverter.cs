namespace System.Text.Json.Converters.Common
{
    public sealed class DigitalDateTimeOffsetConverter : FormattedDateTimeOffsetConverterBase
    {
        protected override string FormatString
        {
            get { return "yyyyMMddHHmmss"; }
        }
    }
}
