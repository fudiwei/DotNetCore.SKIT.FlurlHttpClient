namespace System.Text.Json.Converters.Common
{
    public sealed class BasicDateTimeOffsetConverter : FormattedDateTimeOffsetConverterBase
    {
        protected override string FormatString
        {
            get { return "yyyy-MM-dd HH:mm:ss"; }
        }
    }
}
