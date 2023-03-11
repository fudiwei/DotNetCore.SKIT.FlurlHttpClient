namespace System.Text.Json.Converters.Common
{
    public sealed class StringifiedStringListWithSemicolonSplitConverter : StringifiedStringListWithSplitConverterBase
    {
        protected override string Separator
        {
            get { return ";"; }
        }
    }
}
