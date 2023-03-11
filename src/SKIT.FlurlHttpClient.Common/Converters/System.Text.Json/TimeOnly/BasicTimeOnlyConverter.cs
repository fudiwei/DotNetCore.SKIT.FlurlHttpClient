#if NET5_0_OR_GREATER
namespace System.Text.Json.Converters.Common
{
    public sealed class BasicTimeOnlyConverter : FormattedTimeOnlyConverterBase
    {
        protected override string FormatString
        {
            get { return "HH:mm:ss"; }
        }
    }
}
#endif
