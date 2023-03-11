namespace System.Text.Json.Converters.Common
{
    public sealed class StringifiedStringListWithPipeSplitConverter : StringifiedStringListWithSplitConverterBase
    {
        protected override string Separator
        {
            get { return "|"; }
        }
    }
}
