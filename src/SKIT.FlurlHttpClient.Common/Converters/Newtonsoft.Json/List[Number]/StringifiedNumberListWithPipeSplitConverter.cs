namespace Newtonsoft.Json.Converters.Common
{
    public sealed class StringifiedNumberListWithPipeSplitConverter : StringifiedNumberListWithSplitConverterBase
    {
        protected override string Separator { get { return "|"; } }
    }
}
