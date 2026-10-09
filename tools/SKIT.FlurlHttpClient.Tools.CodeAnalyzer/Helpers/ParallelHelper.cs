using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer.Helpers
{
    public static class ParallelHelper
    {
        /// <summary>
        /// 按顺序执行并行操作，其返回结果的顺序与输入的顺序一致。
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="source"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static IEnumerable<TReturn> ForEachOrdered<TSource, TReturn>(IEnumerable<TSource> source, Func<TSource, TReturn> body)
        {
            TSource[] inputs = source as TSource[] ?? source.ToArray();
            TReturn[] outputs = new TReturn[inputs.Length];
            Exception[] exceptions = new Exception[inputs.Length];

            Parallel.ForEach(inputs, new ParallelOptions(), (e, _, i) =>
            {
                try
                {
                    outputs[i] = body(e);
                }
                catch (Exception ex)
                {
                    exceptions[i] = ex;
                }
            });

            exceptions = exceptions.Where(ex => ex is not null).ToArray();
            if (exceptions.Any())
                throw new AggregateException(exceptions);

            return outputs.Where(output => output is not null).ToArray();
        }
    }
}
