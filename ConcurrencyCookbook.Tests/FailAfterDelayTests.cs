using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ConcurrencyCookbook.Tests
{
    public class FailAfterDelayTests
    {
        // Implement an asynchronous operation that waits for a period of time and then fails.
        private async Task FailAfterDelay(TimeSpan timeSpan)
        {
            await Task.Delay(timeSpan);
            throw new InvalidOperationException();
        }

        [Fact]
        public async Task ThrowsInvalidOperationException()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () =>
                {
                    await FailAfterDelay(
                        TimeSpan.FromMilliseconds(50));
                });
        }

        [Fact]
        public async Task ThrowsAfterDelay()
        {
            var sw = Stopwatch.StartNew();

            await Assert.ThrowsAsync<InvalidOperationException>(
                async () =>
                {
                    await FailAfterDelay(
                        TimeSpan.FromMilliseconds(200));
                });

            sw.Stop();

            Assert.True(sw.ElapsedMilliseconds >= 180);
        }

        [Fact]
        public async Task ExceptionPropagatesThroughAwait()
        {
            Task task = FailAfterDelay(
                TimeSpan.FromMilliseconds(50));

            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await task);
        }

        [Fact]
        public void DoesNotThrowSynchronously()
        {
            Task task = FailAfterDelay(
                TimeSpan.FromMilliseconds(50));

            Assert.NotNull(task);
        }

        [Fact]
        public async Task MultipleCallsFailIndependently()
        {
            Task a = FailAfterDelay(
                TimeSpan.FromMilliseconds(50));

            Task b = FailAfterDelay(
                TimeSpan.FromMilliseconds(50));

            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await a);

            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await b);
        }
    }
}
