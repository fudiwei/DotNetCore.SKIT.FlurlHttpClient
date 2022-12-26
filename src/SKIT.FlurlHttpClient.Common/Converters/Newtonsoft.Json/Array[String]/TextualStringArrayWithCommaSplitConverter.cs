namespace Newtonsoft.Json.Converters.Common
{
    public sealed class TextualStringArrayWithCommaSplitConverter : TextualStringArrayWithSplitConverterBase
    {
        protected override string Separator
        {
            get { return ","; }
        }
    }
}
