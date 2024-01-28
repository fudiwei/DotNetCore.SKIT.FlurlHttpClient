using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer.Helpers.Internal
{
    internal static class ParallelHelper
    {
        public static void ForEachOrdered<TSource>(IEnumerable<TSource> source, Action<TSource> body, CancellationToken cancellationToken = default)
        {
            Exception[] exceptions = new Exception[source.Count()];

            Parallel.ForEach(source, new ParallelOptions() { CancellationToken = cancellationToken }, (e, _, i) =>
            {
                try
                {
                    body(e);
                }
                catch (Exception ex)
                {
                    exceptions[i] = ex;
                }
            });

            exceptions = exceptions
                .Where(ex => ex is not null)
                .SelectMany(ex => ex is AggregateException aex ? aex.InnerExceptions.ToArray() : new Exception[] { ex })
                .ToArray();
            if (exceptions.Any())
                throw new AggregateException(exceptions);
        }
    }
}
