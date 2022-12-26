namespace System.Text.Json.Converters.Common
{
    public sealed class TextualStringListWithCommaSplitConverter : TextualStringListWithSplitConverterBase
    {
        protected override string Separator
        {
            get { return ","; }
        }
    }
}
