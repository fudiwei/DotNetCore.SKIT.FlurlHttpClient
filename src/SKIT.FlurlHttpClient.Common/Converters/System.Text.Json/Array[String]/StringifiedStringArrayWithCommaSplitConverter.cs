namespace System.Text.Json.Converters.Common
{
    public sealed class StringifiedStringArrayWithCommaSplitConverter : StringifiedStringArrayWithSplitConverterBase
    {
        protected override string Separator
        {
            get { return ","; }
        }
    }
}
