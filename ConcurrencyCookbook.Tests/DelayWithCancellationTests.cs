using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ConcurrencyCookbook.Tests
{
    public class DelayWithCancellationTests
    {
        // Asynchronous operations often need cancellation support.
        // If cancellation is requested before the delay completes,
        // the operation should stop immediately.
        async Task<T> DelayResult<T>(T result, TimeSpan delay, CancellationToken token)
        {
            await Task.Delay(delay, token);
            return result;
        }

        [Fact]
        public async Task ReturnsResultWhenNotCanceled()
        {
            int result = await DelayResult(
                5,
                TimeSpan.FromMilliseconds(50),
                CancellationToken.None);

            Assert.Equal(5, result);
        }

        [Fact]
        public async Task CancelsBeforeCompletion()
        {
            using var cts = new CancellationTokenSource();

            Task<int> task = DelayResult(
                1,
                TimeSpan.FromSeconds(5),
                cts.Token);

            cts.Cancel();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(
                async () => await task);
        }

        [Fact]
        public async Task HonorsAlreadyCanceledToken()
        {
            using var cts = new CancellationTokenSource();

            cts.Cancel();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(
                async () =>
                {
                    await DelayResult(1,
                        TimeSpan.FromSeconds(1),
                        cts.Token);
                });
        }

        [Fact]
        public async Task DoesNotCancelAfterCompletion()
        {
            using var cts = new CancellationTokenSource();

            int result = await DelayResult(
                7,
                TimeSpan.FromMilliseconds(50),
                cts.Token);

            cts.Cancel();

            Assert.Equal(7, result);
        }

        [Fact]
        public async Task ThrowsCorrectCancellationException()
        {
            using var cts = new CancellationTokenSource(50);

            await Assert.ThrowsAsync<TaskCanceledException>(
                async () =>
                {
                    await DelayResult(
                        1,
                        TimeSpan.FromSeconds(5),
                        cts.Token);
                });
        }
    }
}
