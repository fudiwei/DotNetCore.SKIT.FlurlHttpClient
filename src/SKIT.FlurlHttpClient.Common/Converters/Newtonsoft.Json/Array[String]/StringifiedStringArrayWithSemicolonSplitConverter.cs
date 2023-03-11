namespace Newtonsoft.Json.Converters.Common
{
    public sealed class StringifiedStringArrayWithSemicolonSplitConverter : StringifiedStringArrayWithSplitConverterBase
    {
        protected override string Separator
        {
            get { return ";"; }
        }
    }
}
