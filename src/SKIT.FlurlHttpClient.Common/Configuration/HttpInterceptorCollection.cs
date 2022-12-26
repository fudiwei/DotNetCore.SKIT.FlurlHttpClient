using System.Collections;
using System.Collections.Generic;

namespace SKIT.FlurlHttpClient.Configuration
{
    /// <summary>
    /// SKIT.FlurlHttpClient HTTP 拦截器集合。
    /// </summary>
    public sealed class HttpInterceptorCollection : IEnumerable, IEnumerable<HttpInterceptor>, ICollection<HttpInterceptor>
    {
        private readonly IList<HttpInterceptor> _list;

        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public HttpInterceptor this[int index]
        {
            get { return _list[index]; }
        }

        internal HttpInterceptorCollection()
        {
            _list = new List<HttpInterceptor>();
        }

        public void Add(HttpInterceptor interceptor)
        {
            _list.Add(interceptor);
        }

        public bool Remove(HttpInterceptor interceptor)
        {
            return _list.Remove(interceptor);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public IEnumerator<HttpInterceptor> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_list).GetEnumerator();
        }

        bool ICollection<HttpInterceptor>.Contains(HttpInterceptor item)
        {
            return _list.Contains(item);
        }

        void ICollection<HttpInterceptor>.CopyTo(HttpInterceptor[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }
    }
}
