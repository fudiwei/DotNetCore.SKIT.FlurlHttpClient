using System;

namespace SKIT.FlurlHttpClient
{
    /// <summary>
    /// 表示一个包含错误信息的布尔值。
    /// </summary>
    public readonly struct ErroredResult : ICloneable, IComparable, IComparable<ErroredResult>, IEquatable<ErroredResult>
    {
        /// <summary>
        /// 表示“真”值的结果。此字段为只读。
        /// </summary>
        public static readonly ErroredResult True = new ErroredResult(true);

        /// <summary>
        /// 表示“假”值的结果。此字段为只读。
        /// </summary>
        public static readonly ErroredResult False = new ErroredResult(false);

        /// <summary>
        /// 返回一个表示“真”值的结果。
        /// </summary>
        /// <returns></returns>
        public static ErroredResult Ok()
        {
            return ErroredResult.True;
        }

        /// <summary>
        /// 返回一个表示“假”值的结果，并设置其包含的错误信息。
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static ErroredResult Fail(Exception? error = null)
        {
            return new ErroredResult(false, error);
        }

        /// <summary>
        /// 获取布尔值。
        /// </summary>
        public bool Result { get; }

        /// <summary>
        /// 获取包含的错误信息。
        /// </summary>
        public Exception? Error { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        public ErroredResult(bool result)
            : this(result, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="error"></param>
        public ErroredResult(bool result, Exception? error)
        {
            if (result && error is not null) throw new ArgumentException("Invalid result value.", nameof(result));

            this.Result = result;
            this.Error = error;
        }

        /// <inheritdoc/>
        public bool Equals(ErroredResult other)
        {
            return this.Result == other.Result
                && this.Error == other.Error;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (GetType() != obj.GetType())
                return false;

            return Equals((ErroredResult)obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
#if NETCOREAPP || NET5_0_OR_GREATER
            return HashCode.Combine(this.Result.GetHashCode(), this.Error?.GetHashCode());
#else
            return (this.Result.GetHashCode(), this.Error?.GetHashCode()).GetHashCode();
#endif
        }

        /// <inheritdoc/>
        public override string? ToString()
        {
            return this.Result.ToString();
        }

        /// <inheritdoc/>
        public static bool operator ==(ErroredResult left, ErroredResult right) => left.Equals(right);
        /// <inheritdoc/>
        public static bool operator !=(ErroredResult left, ErroredResult right) => !left.Equals(right);

        /// <inheritdoc/>
        public static bool operator ==(ErroredResult left, bool right) => left.Result.Equals(right);
        /// <inheritdoc/>
        public static bool operator !=(ErroredResult left, bool right) => !left.Result.Equals(right);

        /// <inheritdoc/>
        public static implicit operator bool(ErroredResult r) => r.Result;
        /// <inheritdoc/>
        public static explicit operator ErroredResult(bool b) => new ErroredResult(b);

#pragma warning disable CS8769
        #region Implement `ICloneable`
        object ICloneable.Clone()
        {
            return new ErroredResult(this.Result, this.Error);
        }
        #endregion

        #region Implement `IComparable`
        int IComparable.CompareTo(object obj)
        {
            ErroredResult other = (ErroredResult)obj;
            return ((IComparable<ErroredResult>)this).CompareTo(other);
        }

        int IComparable<ErroredResult>.CompareTo(ErroredResult other)
        {
            if (this.Result == other.Result)
                return 0;

            if (!this.Result)
                return -1;

            return 1;
        }
        #endregion
#pragma warning restore CS8769
    }
}
