#if NET5_0_OR_GREATER
namespace Newtonsoft.Json.Converters.Common
{
    public sealed class RegularDateOnlyConverter : FormattedDateOnlyConverterBase
    {
        protected override string FormatString
        {
            get { return "yyyy-MM-dd"; }
        }
    }
}
#endif
