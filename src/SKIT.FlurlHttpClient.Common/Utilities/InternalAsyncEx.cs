using System;
using System.Threading;
using System.Threading.Tasks;

namespace SKIT.FlurlHttpClient.Internal
{
#pragma warning disable IDE1006
    public static class _AsyncEx
#pragma warning restore IDE1006
    {
        public static async Task<T> RunTaskWithCancellationTokenAsync<T>(Task<T> task, CancellationToken cancellationToken = default)
        {
            if (task is null) throw new ArgumentNullException(nameof(task));

            Task taskWithCt = await Task.WhenAny(task, Task.Delay(Timeout.Infinite, cancellationToken)).ConfigureAwait(false);
            if (taskWithCt == task)
            {
                return await task.ConfigureAwait(false);
            }
            else
            {
                cancellationToken.ThrowIfCancellationRequested();
                throw new TaskCanceledException("Task was cancelled.");
            }
        }
    }
}
