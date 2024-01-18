using System;
using System.Threading;
using System.Threading.Tasks;

namespace SKIT.FlurlHttpClient.Internal
{
    public static class AsyncEx
    {
        public static async Task<T> RunTaskWithCancellationTokenAsync<T>(Task<T> task, CancellationToken cancellationToken = default)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));

            Task taskWithCt = await Task.WhenAny(task, Task.Delay(Timeout.Infinite, cancellationToken));
            if (taskWithCt == task)
            {
                return await task;
            }
            else
            {
                cancellationToken.ThrowIfCancellationRequested();
                throw new TaskCanceledException("Task was cancelled.");
            }
        }
    }
}