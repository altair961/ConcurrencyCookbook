using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace ConcurrencyCookbook.Tests
{
    public class DelayResultTests
    {
        // You need to simulate an asynchronous operation that completes after a delay
        // and eventually returns a value. This is commonly used in unit tests when faking
        // asynchronous APIs.
        // Implement a method that:
        // asynchronously waits for a specified amount of time,
        // returns the provided result afterward,
        // does not block the calling thread.
        private async Task<T> DelayResult<T>(T result, TimeSpan delay)
        {
            await Task.Delay(delay);

            return result;
        }

        [Fact]
        public async Task ReturnsExpectedInteger()
        {
            int result = await DelayResult(42,
                TimeSpan.FromMilliseconds(50));

            Assert.Equal(42, result);
        }

        [Fact]
        public async Task ReturnsExpectedString()
        {
            string result = await DelayResult("hello",
                TimeSpan.FromMilliseconds(50));

            Assert.Equal("hello", result);
        }

        [Fact]
        public async Task SupportsNullResults()
        {
            string result = await DelayResult<string>(null,
                TimeSpan.FromMilliseconds(50));

            Assert.Null(result);
        }

        [Fact]
        public async Task WaitsBeforeCompleting()
        {
            var sw = Stopwatch.StartNew();

            await DelayResult(1,
                TimeSpan.FromMilliseconds(200));

            sw.Stop();

            Assert.True(sw.ElapsedMilliseconds >= 180);
        }

        [Fact]
        public void DoesNotCompleteSynchronously()
        {
            Task<int> task = DelayResult(1,
                TimeSpan.FromMilliseconds(300));

            Assert.False(task.IsCompleted);
        }

        [Fact]
        public async Task MultipleCallsRunIndependently()
        {
            Task<int> a = DelayResult(1,
                TimeSpan.FromMilliseconds(50));

            Task<int> b = DelayResult(2,
                TimeSpan.FromMilliseconds(100));

            int[] results = await Task.WhenAll(a, b);

            Assert.Equal(new[] { 1, 2 }, results);
        }
        
    }
}