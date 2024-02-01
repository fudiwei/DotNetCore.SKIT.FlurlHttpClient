using System.Collections;
using System.Collections.Generic;

namespace SKIT.FlurlHttpClient
{
    /// <summary>
    /// SKIT.FlurlHttpClient HTTP 拦截器集合。
    /// </summary>
    public sealed class HttpInterceptorCollection : IEnumerable, IEnumerable<HttpInterceptor>, ICollection<HttpInterceptor>
    {
        private readonly IList<HttpInterceptor> _list;

        /// <inheritdoc/>
        public int Count
        {
            get { return _list.Count; }
        }

        /// <inheritdoc/>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <inheritdoc/>
        public HttpInterceptor this[int index]
        {
            get { return _list[index]; }
        }

        internal HttpInterceptorCollection()
        {
            _list = new List<HttpInterceptor>(capacity: 4);
        }

        /// <inheritdoc/>
        public void Add(HttpInterceptor interceptor)
        {
            _list.Add(interceptor);
        }

        /// <inheritdoc/>
        public bool Remove(HttpInterceptor interceptor)
        {
            return _list.Remove(interceptor);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            _list.Clear();
        }

        /// <inheritdoc/>
        public IEnumerator<HttpInterceptor> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_list).GetEnumerator();
        }

        /// <inheritdoc/>
        bool ICollection<HttpInterceptor>.Contains(HttpInterceptor item)
        {
            return _list.Contains(item);
        }

        /// <inheritdoc/>
        void ICollection<HttpInterceptor>.CopyTo(HttpInterceptor[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }
    }
}
