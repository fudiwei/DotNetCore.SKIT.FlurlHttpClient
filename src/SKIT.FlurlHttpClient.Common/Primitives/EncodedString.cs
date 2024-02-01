using System;
using System.Text;

namespace SKIT.FlurlHttpClient.Primitives
{
    /// <summary>
    /// 表示一个经过编码的字符串。
    /// </summary>
    public readonly struct EncodedString : ICloneable, IComparable, IComparable<EncodedString>, IEquatable<EncodedString>
    {
        /// <summary>
        /// 将指定的未编码的字符串转换为等效的字节数组。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] FromLiteralString(EncodedString s)
        {
            switch (s.EncodingKind)
            {
                case EncodingKinds.Unspecified:
                case EncodingKinds.Literal:
                    return s.Value is null ? Array.Empty<byte>() : Encoding.UTF8.GetBytes(s.Value);

                default:
                    throw new FormatException($"Input string was not encoded in literal.");
            }
        }

        /// <summary>
        /// 将指定的未编码的字符串转换为等效的字节数组。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] FromLiteralString(string? s)
        {
            return FromLiteralString(new EncodedString(s, EncodingKinds.Literal));
        }

        /// <summary>
        /// 将指定的经过 Base64 编码的字符串转换为等效的字节数组。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] FromBase64String(EncodedString s)
        {
            switch (s.EncodingKind)
            {
                case EncodingKinds.Unspecified:
                case EncodingKinds.Base64:
                    return s.Value is null ? Array.Empty<byte>() : Convert.FromBase64String(s.Value);

                default:
                    throw new FormatException($"Input string was not encoded in Base64.");
            }
        }

        /// <summary>
        /// 将指定的经过 Base64 编码的字符串转换为等效的字节数组。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] FromBase64String(string? s)
        {
            return FromBase64String(new EncodedString(s, EncodingKinds.Base64));
        }

        /// <summary>
        /// 将指定的经过十六进制编码的字符串转换为等效的字节数组。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] FromHexString(EncodedString s)
        {
            switch (s.EncodingKind)
            {
                case EncodingKinds.Unspecified:
                case EncodingKinds.Hex:
                    {
                        if (s.Value is null)
                            return Array.Empty<byte>();

#if NETCOREAPP || NET5_0_OR_GREATER
                        byte[] bytes = Convert.FromHexString(s.Value);
#else
                        if (s.Value.Length == 0)
                            return Array.Empty<byte>();
                        if ((uint)s.Value.Length % 2 != 0)
                            throw new FormatException("The length of value is not zero or a multiple of 2.");

                        byte[] bytes = new byte[s.Value.Length >> 1];
                        for (int i = 0, j = s.Value.Length; i < j; i += 2)
                        {
                            char c1 = s.Value[i];
                            char c2 = s.Value[i + 1];
                            byte b = (byte)Convert.ToInt32($"{c1}{c2}", 16);
                            bytes[i >> 1] = b;
                        }
#endif

                        return bytes;
                    }

                default:
                    throw new FormatException($"Input string was not encoded in Hex.");
            }
        }

        /// <summary>
        /// 将指定的经过十六进制编码的字符串转换为等效的字节数组。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] FromHexString(string? s)
        {
            return FromHexString(new EncodedString(s, EncodingKinds.Hex));
        }

        /// <summary>
        /// 将指定的经过编码的字符串转换为等效的字节数组。
        /// </summary>
        /// <param name="s"></param>
        /// <param name="fallbackEncodingKind"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        public static byte[] FromString(EncodedString s, EncodingKinds fallbackEncodingKind = EncodingKinds.Literal)
        {
            return FromString(s.Value, s.EncodingKind == EncodingKinds.Unspecified ? fallbackEncodingKind : s.EncodingKind);
        }

        /// <summary>
        /// 将指定的经过编码的字符串转换为等效的字节数组。
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encodingKind"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        public static byte[] FromString(string? s, EncodingKinds encodingKind)
        {
            switch (encodingKind)
            {
                case EncodingKinds.Literal:
                    return FromLiteralString(s);

                case EncodingKinds.Base64:
                    return FromBase64String(s);

                case EncodingKinds.Hex:
                    return FromHexString(s);

                default:
                    throw new FormatException("The encoding kind is not specified.");
            }
        }

        /// <summary>
        /// 将字节数组转换为其未编码的等效 <see cref="EncodedString"/> 表示形式。
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static EncodedString ToLiteralString(byte[] bytes)
        {
            string s = Encoding.UTF8.GetString(bytes);
            return new EncodedString(s, EncodingKinds.Literal);
        }

        /// <summary>
        /// 将字节数组转换为其用 Base64 编码的等效 <see cref="EncodedString"/> 表示形式。
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static EncodedString ToBase64String(byte[] bytes)
        {
            string s = Convert.ToBase64String(bytes);
            return new EncodedString(s, EncodingKinds.Base64);
        }

        /// <summary>
        /// 将字节数组转换为其用十六进制编码的等效 <see cref="EncodedString"/> 表示形式。
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static EncodedString ToHexString(byte[] bytes)
        {

#if NETCOREAPP || NET5_0_OR_GREATER
            string s = Convert.ToHexString(bytes);
#else
            string s = BitConverter.ToString(bytes).Replace("-", "");
#endif
            return new EncodedString(s, EncodingKinds.Hex);
        }

        /// <summary>
        /// 将字节数组转换为等效 <see cref="EncodedString"/> 表示形式。
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="encodingKind"></param>
        /// <returns></returns>
        public static EncodedString ToString(byte[] bytes, EncodingKinds encodingKind)
        {
            switch (encodingKind)
            {
                case EncodingKinds.Literal:
                    return ToLiteralString(bytes);

                case EncodingKinds.Base64:
                    return ToBase64String(bytes);

                case EncodingKinds.Hex:
                    return ToHexString(bytes);

                default:
                    throw new FormatException("The encoding kind is not specified.");
            }
        }

        /// <summary>
        /// 获取字符串值。
        /// </summary>
        public string? Value { get; }

        /// <summary>
        /// 获取编码方式。
        /// </summary>
        public EncodingKinds EncodingKind { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public EncodedString(string? value)
            : this(value, EncodingKinds.Unspecified)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encodingKind"></param>
        /// <param name="value"></param>
        public EncodedString(string? value, EncodingKinds encodingKind)
        {
            this.Value = value;
            this.EncodingKind = encodingKind;
        }

        /// <inheritdoc/>
        public bool Equals(EncodedString other)
        {
            return this.EncodingKind == other.EncodingKind
                && this.Value == other.Value;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (GetType() != obj.GetType())
                return false;

            return Equals((EncodedString)obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
#if NETCOREAPP || NET5_0_OR_GREATER
            return HashCode.Combine(this.EncodingKind.GetHashCode(), this.Value?.GetHashCode());
#else
            return (this.EncodingKind.GetHashCode(), this.Value?.GetHashCode()).GetHashCode();
#endif
        }

        /// <inheritdoc/>
        public override string? ToString()
        {
            return this.Value;
        }

        /// <inheritdoc/>
        public static bool operator ==(EncodedString left, EncodedString right) => left.Equals(right);
        /// <inheritdoc/>
        public static bool operator !=(EncodedString left, EncodedString right) => !left.Equals(right);

        /// <inheritdoc/>
        public static implicit operator string?(EncodedString s) => s.Value;
        /// <inheritdoc/>
        public static explicit operator EncodedString(string? s) => new EncodedString(s);

#pragma warning disable CS8769
        #region Implement `ICloneable`
        object ICloneable.Clone()
        {
            return new EncodedString(this.Value, this.EncodingKind);
        }
        #endregion

        #region Implement `IComparable`
        int IComparable.CompareTo(object obj)
        {
            EncodedString other = (EncodedString)obj;
            return ((IComparable<EncodedString>)this).CompareTo(other);
        }

        int IComparable<EncodedString>.CompareTo(EncodedString other)
        {
            int ret = string.Compare(this.Value, other.Value);
            if (ret != 0)
                return ret;

            return this.EncodingKind.CompareTo(other.EncodingKind);
        }
        #endregion
#pragma warning restore CS8769
    }
}
