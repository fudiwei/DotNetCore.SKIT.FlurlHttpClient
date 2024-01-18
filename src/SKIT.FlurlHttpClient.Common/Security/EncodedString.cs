using System;

namespace SKIT.FlurlHttpClient
{
    /// <summary>
    /// 表示一个经过编码的字符串。
    /// </summary>
    public readonly struct EncodedString : ICloneable, IComparable, IComparable<EncodedString>, IEquatable<EncodedString>
    {
        /// <summary>
        /// 将指定的经过 Base64 编码的字符串转换为等效的字节数组。
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] FromBase64String(EncodedString s)
        {
            switch (s.EncodingKind)
            {
                case EncodingKinds.Unspecified:
                case EncodingKinds.Base64:
                    return s.Value == null ? Array.Empty<byte>() : Convert.FromBase64String(s.Value);

                default:
                    throw new FormatException($"Input string was not encoded in Base64.");
            }
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
                        if (s.Value == null)
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
        /// 将指定的经过编码的字符串转换为等效的字节数组。
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultEncodingKind"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        public static byte[] FromEncodedString(string s, EncodingKinds encodingKind)
        {
            switch (encodingKind)
            {
                case EncodingKinds.Base64:
                    return FromBase64String(new EncodedString(s, EncodingKinds.Base64));

                case EncodingKinds.Hex:
                    return FromHexString(new EncodedString(s, EncodingKinds.Hex));

                default:
                    throw new FormatException("The encoding kind is not specified.");
            }
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
        /// <returns></returns>
        public static EncodedString ToEncodedString(byte[] bytes, EncodingKinds encodingKind)
        {
            switch (encodingKind)
            {
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

        public static bool operator ==(EncodedString left, EncodedString right) => left.Equals(right);
        public static bool operator !=(EncodedString left, EncodedString right) => !left.Equals(right);

        public static implicit operator string?(EncodedString s) => s.Value;
        public static explicit operator EncodedString(string? s) => new EncodedString(s);

#pragma warning disable CS8603
#pragma warning disable CS8604
#pragma warning disable CS8769
        #region Implement `ICloneable`
        object ICloneable.Clone()
        {
            return new EncodedString(Value, EncodingKind);
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

            return EncodingKind.CompareTo(other.EncodingKind);
        }
        #endregion
#pragma warning restore CS8769
#pragma warning restore CS8604
#pragma warning restore CS8603
    }
}