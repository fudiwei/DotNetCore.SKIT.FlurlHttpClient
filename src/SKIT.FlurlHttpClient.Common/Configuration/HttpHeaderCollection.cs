using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        public ReadOnlyDictionary<string, IEnumerable<string>>.KeyCollection Keys
        {
            get { return _dict.Keys; }
        }

        public ReadOnlyDictionary<string, IEnumerable<string>>.ValueCollection Values
        {
            get { return _dict.Values; }
        }

        public int Count
        {
            get { return _dict.Count; }
        }

        public bool IsReadOnly
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

        IEnumerable<string> IReadOnlyDictionary<string, IEnumerable<string>>.Keys
        {
            get { return this.Keys; }
        }

        IEnumerable<IEnumerable<string>> IReadOnlyDictionary<string, IEnumerable<string>>.Values
        {
            get { return this.Values; }
        }

        IEnumerable<string> IDictionary<string, IEnumerable<string>>.this[string key]
        {
            get { return this[key]; }
            set { throw new NotImplementedException(); }
        }

        internal HttpHeaderCollection(IDictionary<string, IEnumerable<string>> httpHeaders)
        {
            _dict = new ReadOnlyDictionary<string, IEnumerable<string>>(
                new Dictionary<string, IEnumerable<string>>(
                    httpHeaders.ToDictionary(
                        k => k.Key.ToLower(),
                        v => v.Value
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
                        k => k.Name.ToLower(),
                        v => httpHeaders.GetAll(v.Name)
                    ),
                    StringComparer.OrdinalIgnoreCase
                )
            );
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
            return _dict.TryGetValue(key, out value);
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
            throw new NotImplementedException();
        }

        bool IDictionary<string, IEnumerable<string>>.Remove(string key)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<string, IEnumerable<string>>>.Add(KeyValuePair<string, IEnumerable<string>> item)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<string, IEnumerable<string>>>.Clear()
        {
            throw new NotImplementedException();
        }
    }
}
