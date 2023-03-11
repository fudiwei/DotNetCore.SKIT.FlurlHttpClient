namespace System.Text.Json.Converters.Common
{
    public sealed class StringifiedNumberListWithCommaSplitConverter : StringifiedNumberListWithSplitConverterBase
    {
        protected override string Separator { get { return ","; } }
    }
}
