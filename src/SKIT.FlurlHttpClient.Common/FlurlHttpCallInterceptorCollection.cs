using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKIT.FlurlHttpClient
{
    public sealed class FlurlHttpCallInterceptorCollection : IEnumerable<FlurlHttpCallInterceptor>
    {
        private readonly IList<FlurlHttpCallInterceptor> _list;

        public int Count
        {
            get { return _list.Count; }
        }

        public FlurlHttpCallInterceptor this[int index]
        {
            get { return _list[index]; }
        }

        public FlurlHttpCallInterceptorCollection()
        {
            _list = new List<FlurlHttpCallInterceptor>();
        }

        public void Add(FlurlHttpCallInterceptor interceptor)
        {
            _list.Add(interceptor);
        }

        public void Remove(FlurlHttpCallInterceptor interceptor)
        {
            _list.Remove(interceptor);
        }

        public IEnumerator<FlurlHttpCallInterceptor> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_list).GetEnumerator();
        }
    }
}
