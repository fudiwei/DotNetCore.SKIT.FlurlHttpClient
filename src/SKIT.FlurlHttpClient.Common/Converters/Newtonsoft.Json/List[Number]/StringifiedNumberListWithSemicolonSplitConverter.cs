namespace Newtonsoft.Json.Converters.Common
{
    public sealed class StringifiedNumberListWithSemicolonSplitConverter : StringifiedNumberListWithSplitConverterBase
    {
        protected override string Separator { get { return ";"; } }
    }
}
