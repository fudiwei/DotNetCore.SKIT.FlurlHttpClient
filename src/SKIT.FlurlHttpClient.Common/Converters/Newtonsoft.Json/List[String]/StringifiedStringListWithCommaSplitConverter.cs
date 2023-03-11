namespace Newtonsoft.Json.Converters.Common
{
    public sealed class StringifiedStringListWithCommaSplitConverter : StringifiedStringListWithSplitConverterBase
    {
        protected override string Separator
        {
            get { return ","; }
        }
    }
}
