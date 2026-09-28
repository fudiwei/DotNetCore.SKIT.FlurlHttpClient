namespace SKIT.FlurlHttpClient.UnitTests.TestCases
{
    using SKIT.FlurlHttpClient.Internal;

    public class TestCase_UtilityInternalAsyncExTest
    {
        [Fact(DisplayName = "测试用例：工具类 `_AsyncEx.RunTaskWithCancellationTokenAsync` 之任务完成")]
        public async Task TestUtilityInternalAsyncEx_RunTaskWithCancellationTokenAsync_CompletedTask()
        {
            int result = await _AsyncEx.RunTaskWithCancellationTokenAsync(Task.FromResult(42));

            Assert.Equal(42, result);
        }

        [Fact(DisplayName = "测试用例：工具类 `_AsyncEx.RunTaskWithCancellationTokenAsync` 之任务失败")]
        public async Task TestUtilityInternalAsyncEx_RunTaskWithCancellationTokenAsync_FaultedTask()
        {
            var exception = new InvalidOperationException("task failed");
            Task<int> task = Task.FromException<int>(exception);

            InvalidOperationException actual = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _AsyncEx.RunTaskWithCancellationTokenAsync(task));

            Assert.Same(exception, actual);
        }

        [Fact(DisplayName = "测试用例：工具类 `_AsyncEx.RunTaskWithCancellationTokenAsync` 之任务取消")]
        public async Task TestUtilityInternalAsyncEx_RunTaskWithCancellationTokenAsync_CancellationTokenCanceled()
        {
            var taskSource = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
            using var cancellationTokenSource = new CancellationTokenSource();
            Task<int> task = _AsyncEx.RunTaskWithCancellationTokenAsync(taskSource.Task, cancellationTokenSource.Token);

            cancellationTokenSource.Cancel();

            OperationCanceledException exception = await Assert.ThrowsAnyAsync<OperationCanceledException>(() => task);
            Assert.Equal(cancellationTokenSource.Token, exception.CancellationToken);
            Assert.False(taskSource.Task.IsCompleted);
        }

        [Fact(DisplayName = "测试用例：工具类 `_AsyncEx.RunTaskWithCancellationTokenAsync` 之空任务")]
        public async Task TestUtilityInternalAsyncEx_RunTaskWithCancellationTokenAsync_NullTask()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
            {
                return _AsyncEx.RunTaskWithCancellationTokenAsync<int>(null!);
            });
        }
    }
}
