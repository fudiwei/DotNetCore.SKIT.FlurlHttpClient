namespace SKIT.FlurlHttpClient.Utilities.Internal
{
    public static class FormatUtility
    {
        public static bool MaybeJson(string value)
        {
            if (string.IsNullOrEmpty(value)) return false;

            string str = value.Trim();
            return (str.StartsWith("[") && str.EndsWith("]"))
                || (str.StartsWith("{") && str.EndsWith("}"));
        }

        public static bool MaybeJson(byte[] value)
        {
            if (value == null || value.Length == 0) return false;

            const byte B_SPACE = 0x20;
            const byte B_BRACE_L = 0x5b; // '['
            const byte B_BRACE_R = 0x5d; // ']'
            const byte B_BRACKET_L = 0x7b; // '{'
            const byte B_BRACKET_R = 0x7d; // '}'

            byte bs = default, be = default;

            for (long i = 0; i < value.LongLength; i++)
            {
                bs = value[i];
                if (bs > B_SPACE)
                    break;
            }

            if (bs != B_BRACE_L && bs != B_BRACKET_L)
                return false;

            for (long i = value.LongLength - 1; i >= 0; i--)
            {
                be = value[i];
                if (be > B_SPACE)
                    break;
            }

            return (bs == B_BRACE_L && be == B_BRACE_R)
                || (bs == B_BRACKET_L && be == B_BRACKET_R);
        }

        public static bool MaybeXml(string value)
        {
            if (string.IsNullOrEmpty(value)) return false;

            const int MIN_XML_LENGTH = 4;

            string str = value.Trim();
            return (str.StartsWith("<") && str.EndsWith(">") && str.Length >= MIN_XML_LENGTH);
        }

        public static bool MaybeXml(byte[] value)
        {
            if (value == null || value.Length == 0) return false;

            const byte B_SPACE = 0x20;
            const byte B_ANGLEDBRACKET_L = 0x3c; // '<'
            const byte B_ANGLEDBRACKET_R = 0x3e; // '>'
            const int MIN_XML_LENGTH = 4;

            byte bs = default, be = default;

            for (long i = 0; i < value.LongLength; i++)
            {
                bs = value[i];
                if (bs > B_SPACE)
                    break;
            }

            if (bs != B_ANGLEDBRACKET_L)
                return false;

            for (long i = value.LongLength - 1; i >= 0; i--)
            {
                be = value[i];
                if (be > B_SPACE)
                    break;
            }

            return (bs == B_ANGLEDBRACKET_L && be == B_ANGLEDBRACKET_R && value.Length >= MIN_XML_LENGTH);
        }
    }
}
