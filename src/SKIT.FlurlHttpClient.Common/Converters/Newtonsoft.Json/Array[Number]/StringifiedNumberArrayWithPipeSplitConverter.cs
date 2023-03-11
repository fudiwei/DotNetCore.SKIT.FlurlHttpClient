namespace Newtonsoft.Json.Converters.Common
{
    public sealed class StringifiedNumberArrayWithPipeSplitConverter : StringifiedNumberArrayWithSplitConverterBase
    {
        protected override string Separator
        {
            get { return "|";  }
        }
    }
}
