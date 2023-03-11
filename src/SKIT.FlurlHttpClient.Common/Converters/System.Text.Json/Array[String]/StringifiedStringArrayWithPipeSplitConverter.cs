namespace System.Text.Json.Converters.Common
{
    public sealed class StringifiedStringArrayWithPipeSplitConverter : StringifiedStringArrayWithSplitConverterBase
    {
        protected override string Separator
        {
            get { return "|"; }
        }
    }
}
