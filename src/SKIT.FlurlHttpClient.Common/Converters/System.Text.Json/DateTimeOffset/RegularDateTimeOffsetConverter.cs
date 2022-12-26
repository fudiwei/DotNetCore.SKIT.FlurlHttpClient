namespace System.Text.Json.Converters.Common
{
    public sealed class RegularDateTimeOffsetConverter : FormattedDateTimeOffsetConverterBase
    {
        protected override string DateFormat
        {
            get { return "yyyy-MM-dd HH:mm:ss"; }
        }
    }
}
