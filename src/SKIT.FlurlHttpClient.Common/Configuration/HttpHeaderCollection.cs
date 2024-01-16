using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Flurl.Util;

namespace SKIT.FlurlHttpClient.Configuration
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

        public string? Age
        {
            get { return TryGetGroupingValue(Constants.HttpHeaders.Age, out string? value) ? value : default; }
        }

        public string? CacheControl
        {
            get { return TryGetGroupingValue(Constants.HttpHeaders.CacheControl, out string? value) ? value : default; }
        }

        public string? ContentEncoding
        {
            get { return TryGetGroupingValue(Constants.HttpHeaders.ContentEncoding, out string? value) ? value : default; }
        }

        public string? ContentLength
        {
            get { return TryGetGroupingValue(Constants.HttpHeaders.ContentLength, out string? value) ? value : default; }
        }

        public string? ContentType
        {
            get { return TryGetGroupingValue(Constants.HttpHeaders.ContentType, out string? value) ? value : default; }
        }

        public string? Date
        {
            get { return TryGetGroupingValue(Constants.HttpHeaders.Date, out string? value) ? value : default; }
        }

        public string? Expires
        {
            get { return TryGetGroupingValue(Constants.HttpHeaders.Expires, out string? value) ? value : default; }
        }

        public string? ETag
        {
            get { return TryGetGroupingValue(Constants.HttpHeaders.ETag, out string? value) ? value : default; }
        }

        public string? LastModified
        {
            get { return TryGetGroupingValue(Constants.HttpHeaders.LastModified, out string? value) ? value : default; }
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

        public string[] GetAllKeys()
        {
            return _dict.Keys.ToArray();
        }

        public bool ContainsKey(string key)
        {
            return _dict.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<string, IEnumerable<string>>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        public bool TryGetValue(string key, out IEnumerable<string> value)
        {
            return _dict.TryGetValue(key, out value!);
        }

        public bool TryGetGroupingValue(string key, out string? value)
        {
            bool result = _dict.TryGetValue(key, out IEnumerable<string>? values);
            value = values is null ? null : string.Join(", ", values);
            return result;
        }

        public bool TryGetFirstValue(string key, out string? value)
        {
            bool result = _dict.TryGetValue(key, out IEnumerable<string>? values);
            value = values?.FirstOrDefault();
            return result;
        }

        public IEnumerable<string> GetValueOrEmpty(string key)
        {
            TryGetValue(key, out IEnumerable<string>? values);
            return values ?? Enumerable.Empty<string>();
        }

        public string GetGroupingValueOrEmpty(string key)
        {
            TryGetGroupingValue(key, out string? value);
            return value ?? string.Empty;
        }

        public string GetFirstValueOrEmpty(string key)
        {
            TryGetFirstValue(key, out string? value);
            return value ?? string.Empty;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_dict).GetEnumerator();
        }

        void IDictionary<string, IEnumerable<string>>.Add(string key, IEnumerable<string> value)
        {
            throw new NotSupportedException();
        }

        bool IDictionary<string, IEnumerable<string>>.Remove(string key)
        {
            throw new NotSupportedException();
        }

        void ICollection<KeyValuePair<string, IEnumerable<string>>>.Add(KeyValuePair<string, IEnumerable<string>> item)
        {
            throw new NotSupportedException();
        }

        bool ICollection<KeyValuePair<string, IEnumerable<string>>>.Contains(KeyValuePair<string, IEnumerable<string>> item)
        {
            return ((ICollection<KeyValuePair<string, IEnumerable<string>>>)_dict).Contains(item);
        }

        void ICollection<KeyValuePair<string, IEnumerable<string>>>.CopyTo(KeyValuePair<string, IEnumerable<string>>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, IEnumerable<string>>>)_dict).CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<string, IEnumerable<string>>>.Remove(KeyValuePair<string, IEnumerable<string>> item)
        {
            throw new NotSupportedException();
        }

        void ICollection<KeyValuePair<string, IEnumerable<string>>>.Clear()
        {
            throw new NotSupportedException();
        }

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
