namespace System.Text.Json.Converters.Common
{
    public sealed class StringifiedNumberArrayWithCommaSplitConverter : StringifiedNumberArrayWithSplitConverterBase
    {
        protected override string Separator
        {
            get { return ",";  }
        }
    }
}
