#if NET5_0_OR_GREATER
namespace Newtonsoft.Json.Converters.Common
{
    public sealed class DigitalDateOnlyConverter : FormattedDateOnlyConverterBase
    {
        protected override string FormatString
        {
            get { return "yyyyMMdd"; }
        }
    }
}
#endif
