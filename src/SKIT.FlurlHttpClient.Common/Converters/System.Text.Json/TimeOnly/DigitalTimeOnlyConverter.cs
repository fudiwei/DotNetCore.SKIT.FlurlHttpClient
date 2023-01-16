#if NET5_0_OR_GREATER
namespace System.Text.Json.Converters.Common
{
    public sealed class DigitalTimeOnlyConverter : FormattedTimeOnlyConverterBase
    {
        protected override string FormatString
        {
            get { return "HHmmss"; }
        }
    }
}
#endif
