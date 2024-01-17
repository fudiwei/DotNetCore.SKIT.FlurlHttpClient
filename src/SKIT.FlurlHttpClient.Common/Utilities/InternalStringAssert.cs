using System;

namespace SKIT.FlurlHttpClient.Internal
{
    public static class StringAssert
    {
        public static bool MaybeJson(string value)
        {
            if (value == null) return false;

            return MaybeJson(value.AsSpan());
        }

        public static bool MaybeJson(byte[] value)
        {
            if (value == null) return false;

            return MaybeJson(value.AsSpan());
        }

        public static bool MaybeJson(ReadOnlySpan<byte> value)
        {
            if (value == null || value.Length == 0) return false;

            const byte B_SPACE = 0x20;
            const byte B_BRACE_L = 0x5b; // '['
            const byte B_BRACE_R = 0x5d; // ']'
            const byte B_BRACKET_L = 0x7b; // '{'
            const byte B_BRACKET_R = 0x7d; // '}'

            byte bs = default, be = default;

            for (int i = 0; i < value.Length; i++)
            {
                bs = value[i];
                if (bs > B_SPACE)
                    break;
            }

            if (bs != B_BRACE_L && bs != B_BRACKET_L)
                return false;

            for (int i = value.Length - 1; i >= 0; i--)
            {
                be = value[i];
                if (be > B_SPACE)
                    break;
            }

            return (bs == B_BRACE_L && be == B_BRACE_R)
                || (bs == B_BRACKET_L && be == B_BRACKET_R);
        }

        public static bool MaybeJson(ReadOnlySpan<char> value)
        {
            if (value == null || value.Length == 0) return false;

            const char B_SPACE = ' ';
            const char B_BRACE_L = '[';
            const char B_BRACE_R = ']';
            const char B_BRACKET_L = '{';
            const char B_BRACKET_R = '}';

            char bs = default, be = default;

            for (int i = 0; i < value.Length; i++)
            {
                bs = value[i];
                if (bs > B_SPACE)
                    break;
            }

            if (bs != B_BRACE_L && bs != B_BRACKET_L)
                return false;

            for (int i = value.Length - 1; i >= 0; i--)
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
            if (value == null) return false;

            return MaybeXml(value.AsSpan());
        }

        public static bool MaybeXml(byte[] value)
        {
            if (value == null) return false;

            return MaybeXml(value.AsSpan());
        }

        public static bool MaybeXml(ReadOnlySpan<byte> value)
        {
            if (value == null || value.Length == 0) return false;

            const byte B_SPACE = 0x20;
            const byte B_ANGLEDBRACKET_L = 0x3c; // '<'
            const byte B_ANGLEDBRACKET_R = 0x3e; // '>'

            const int MIN_XML_LENGTH = 4;

            byte bs = default, be = default;
            int ns = default, ne = default;

            for (int i = 0; i < value.Length; i++)
            {
                bs = value[i];
                if (bs > B_SPACE)
                {
                    ns = i;
                    break;
                }
            }

            if (bs != B_ANGLEDBRACKET_L)
                return false;

            for (int i = value.Length - 1; i >= 0; i--)
            {
                be = value[i];
                if (be > B_SPACE)
                {
                    ne = i;
                    break;
                }
            }

            return (bs == B_ANGLEDBRACKET_L && be == B_ANGLEDBRACKET_R && (ne - ns + 1) >= MIN_XML_LENGTH);
        }

        public static bool MaybeXml(ReadOnlySpan<char> value)
        {
            if (value == null || value.Length == 0) return false;

            const char B_SPACE = ' ';
            const char B_ANGLEDBRACKET_L = '<';
            const char B_ANGLEDBRACKET_R = '>';

            const int MIN_XML_LENGTH = 4;

            char bs = default, be = default;
            int ns = default, ne = default;

            for (int i = 0; i < value.Length; i++)
            {
                bs = value[i];
                if (bs > B_SPACE)
                {
                    ns = i;
                    break;
                }
            }

            if (bs != B_ANGLEDBRACKET_L)
                return false;

            for (int i = value.Length - 1; i >= 0; i--)
            {
                be = value[i];
                if (be > B_SPACE)
                {
                    ne = i;
                    break;
                }
            }

            return (bs == B_ANGLEDBRACKET_L && be == B_ANGLEDBRACKET_R && (ne - ns + 1) >= MIN_XML_LENGTH);
        }
    }
}
