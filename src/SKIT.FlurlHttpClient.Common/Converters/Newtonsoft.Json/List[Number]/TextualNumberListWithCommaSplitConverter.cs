namespace Newtonsoft.Json.Converters.Common
{
    public sealed class TextualNumberListWithCommaSplitConverter : TextualNumberListWithSplitConverterBase
    {
        protected override string Separator { get { return ","; } }
    }
}
