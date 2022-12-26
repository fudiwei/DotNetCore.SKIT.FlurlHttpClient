namespace Newtonsoft.Json.Converters.Common
{
    public sealed class TextualNumberArrayWithCommaSplitConverter : TextualNumberArrayWithSplitConverterBase
    {
        protected override string Separator
        {
            get { return ",";  }
        }
    }
}
