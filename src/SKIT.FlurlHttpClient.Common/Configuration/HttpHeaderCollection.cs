using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Flurl.Util;

namespace SKIT.FlurlHttpClient
{
    /// <summary>
    /// SKIT.FlurlHttpClient HTTP 标头集合。
    /// </summary>
    public sealed class HttpHeaderCollection : IDictionary<string, IEnumerable<string>>, IReadOnlyDictionary<string, IEnumerable<string>>
    {
        public static readonly HttpHeaderCollection Empty = new HttpHeaderCollection(new Dictionary<string, IEnumerable<string>>());

        private readonly ReadOnlyDictionary<string, IEnumerable<string>> _dict;

        public IEnumerable<string> this[string key]
        {
            get { return _dict[key]; }
        }

        int ICollection<KeyValuePair<string, IEnumerable<string>>>.Count
        {
            get { return _dict.Count; }
        }

        bool ICollection<KeyValuePair<string, IEnumerable<string>>>.IsReadOnly
        {
            get { return true; }
        }

        ICollection<string> IDictionary<string, IEnumerable<string>>.Keys
        {
            get { return this.Keys; }
        }

        ICollection<IEnumerable<string>> IDictionary<string, IEnumerable<string>>.Values
        {
            get { return this.Values; }
        }

        int IReadOnlyCollection<KeyValuePair<string, IEnumerable<string>>>.Count
        {
            get { return _dict.Count; }
        }

        IEnumerable<string> IReadOnlyDictionary<string, IEnumerable<string>>.Keys
        {
            get { return this.Keys; }
        }

        IEnumerable<IEnumerable<string>> IReadOnlyDictionary<string, IEnumerable<string>>.Values
        {
            get { return this.Values; }
        }

        ReadOnlyDictionary<string, IEnumerable<string>>.KeyCollection Keys
        {
            get { return _dict.Keys; }
        }

        ReadOnlyDictionary<string, IEnumerable<string>>.ValueCollection Values
        {
            get { return _dict.Values; }
        }

        /// <summary>
        /// 获取集合中 Age 标头的值。
        /// </summary>
        public string? Age
        {
            get { return TryGetGroupingValue(HttpHeaders.Age, out string? value) ? value : default; }
        }

        /// <summary>
        /// 获取集合中 Cache-Control 标头的值。
        /// </summary>
        public string? CacheControl
        {
            get { return TryGetGroupingValue(HttpHeaders.CacheControl, out string? value) ? value : default; }
        }

        /// <summary>
        /// 获取集合中 Content-Encoding 标头的值。
        /// </summary>
        public string? ContentEncoding
        {
            get { return TryGetGroupingValue(HttpHeaders.ContentEncoding, out string? value) ? value : default; }
        }

        /// <summary>
        /// 获取集合中 Content-Language 标头的值。
        /// </summary>
        public string? ContentLanguage
        {
            get { return TryGetGroupingValue(HttpHeaders.ContentLanguage, out string? value) ? value : default; }
        }

        /// <summary>
        /// 获取集合中 Content-Length 标头的值。
        /// </summary>
        public string? ContentLength
        {
            get { return TryGetGroupingValue(HttpHeaders.ContentLength, out string? value) ? value : default; }
        }

        /// <summary>
        /// 获取集合中 Content-Type 标头的值。
        /// </summary>
        public string? ContentType
        {
            get { return TryGetGroupingValue(HttpHeaders.ContentType, out string? value) ? value : default; }
        }

        /// <summary>
        /// 获取集合中 Date 标头的值。
        /// </summary>
        public string? Date
        {
            get { return TryGetGroupingValue(HttpHeaders.Date, out string? value) ? value : default; }
        }

        /// <summary>
        /// 获取集合中 Expires 标头的值。
        /// </summary>
        public string? Expires
        {
            get { return TryGetGroupingValue(HttpHeaders.Expires, out string? value) ? value : default; }
        }

        /// <summary>
        /// 获取集合中 ETag 标头的值。
        /// </summary>
        public string? ETag
        {
            get { return TryGetGroupingValue(HttpHeaders.ETag, out string? value) ? value : default; }
        }

        /// <summary>
        /// 获取集合中 LastModified 标头的值。
        /// </summary>
        public string? LastModified
        {
            get { return TryGetGroupingValue(HttpHeaders.LastModified, out string? value) ? value : default; }
        }

        IEnumerable<string> IDictionary<string, IEnumerable<string>>.this[string key]
        {
            get { return this[key]; }
            set { throw new NotSupportedException(); }
        }

        internal HttpHeaderCollection(IDictionary<string, IEnumerable<string>> httpHeaders)
        {
            _dict = new ReadOnlyDictionary<string, IEnumerable<string>>(
                new Dictionary<string, IEnumerable<string>>(
                    httpHeaders.ToDictionary(
                        static k => k.Key.ToLower(),
                        static v => v.Value
                    ),
                    StringComparer.OrdinalIgnoreCase
                )
            );
        }

        internal HttpHeaderCollection(IReadOnlyNameValueList<string> httpHeaders)
        {
            _dict = new ReadOnlyDictionary<string, IEnumerable<string>>(
                new Dictionary<string, IEnumerable<string>>(
                    httpHeaders.ToDictionary(
                        static k => k.Name.ToLower(),
                        v => httpHeaders.GetAll(v.Name)
                    ),
                    StringComparer.OrdinalIgnoreCase
                )
            );
        }

        /// <summary>
        /// 获取集合中全部标头的名称。
        /// </summary>
        /// <returns></returns>
        public string[] GetAllKeys()
        {
            return _dict.Keys.ToArray();
        }

        /// <summary>
        /// 判断指定的标头是否存在于集合中。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return _dict.ContainsKey(key);
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<string, IEnumerable<string>>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }
        
        /// <inheritdoc/>
        public bool TryGetValue(string key, out IEnumerable<string> value)
        {
            return _dict.TryGetValue(key, out value!);
        }

        /// <inheritdoc/>
        public bool TryGetGroupingValue(string key, out string? value)
        {
            bool result = _dict.TryGetValue(key, out IEnumerable<string>? values);
            value = values is null ? null : string.Join(", ", values);
            return result;
        }

        /// <inheritdoc/>
        public bool TryGetFirstValue(string key, out string? value)
        {
            bool result = _dict.TryGetValue(key, out IEnumerable<string>? values);
            value = values?.FirstOrDefault();
            return result;
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetValueOrEmpty(string key)
        {
            TryGetValue(key, out IEnumerable<string>? values);
            return values ?? Enumerable.Empty<string>();
        }

        /// <inheritdoc/>
        public string GetGroupingValueOrEmpty(string key)
        {
            TryGetGroupingValue(key, out string? value);
            return value ?? string.Empty;
        }

        /// <inheritdoc/>
        public string GetFirstValueOrEmpty(string key)
        {
            TryGetFirstValue(key, out string? value);
            return value ?? string.Empty;
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_dict).GetEnumerator();
        }

        /// <inheritdoc/>
        void IDictionary<string, IEnumerable<string>>.Add(string key, IEnumerable<string> value)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        bool IDictionary<string, IEnumerable<string>>.Remove(string key)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        void ICollection<KeyValuePair<string, IEnumerable<string>>>.Add(KeyValuePair<string, IEnumerable<string>> item)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<string, IEnumerable<string>>>.Contains(KeyValuePair<string, IEnumerable<string>> item)
        {
            return ((ICollection<KeyValuePair<string, IEnumerable<string>>>)_dict).Contains(item);
        }

        /// <inheritdoc/>
        void ICollection<KeyValuePair<string, IEnumerable<string>>>.CopyTo(KeyValuePair<string, IEnumerable<string>>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, IEnumerable<string>>>)_dict).CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<string, IEnumerable<string>>>.Remove(KeyValuePair<string, IEnumerable<string>> item)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        void ICollection<KeyValuePair<string, IEnumerable<string>>>.Clear()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            using (IEnumerator<KeyValuePair<string, IEnumerable<string>>> enumerator = GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    KeyValuePair<string, IEnumerable<string>> current = enumerator.Current;
                    stringBuilder.Append(current.Key);
                    stringBuilder.Append(": ");
                    stringBuilder.Append(GetGroupingValueOrEmpty(current.Key));
                    stringBuilder.Append("\r\n");
                }
            }
            return stringBuilder.ToString();
        }
    }
}
