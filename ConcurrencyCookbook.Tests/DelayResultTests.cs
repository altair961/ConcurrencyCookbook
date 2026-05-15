using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace ConcurrencyCookbook.Tests
{
    public class DelayResultTests
    {
        // TODO:
        // Implement this method so that:
        // 1. It asynchronously waits for the specified delay
        // 2. It returns the provided result
        private async Task<T> DelayResult<T>(T result, TimeSpan delay)
        {
            await Task.Delay(delay);

            return result;
        }

        [Fact]
        public async Task DelayResult_ReturnsExpectedValue()
        {
            // Arrange
            const int expected = 42;

            // Act
            int actual = await DelayResult(expected,
                TimeSpan.FromMilliseconds(50));

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task DelayResult_WaitsApproximatelySpecifiedTime()
        {
            // Arrange
            TimeSpan delay = TimeSpan.FromMilliseconds(200);

            var stopwatch = Stopwatch.StartNew();

            // Act
            await DelayResult("done", delay);

            stopwatch.Stop();

            // Assert
            Assert.True(
                stopwatch.Elapsed >= delay,
                $"Expected at least {delay.TotalMilliseconds}ms " +
                $"but was {stopwatch.Elapsed.TotalMilliseconds}ms");
        }

        [Fact]
        public async Task DelayResult_DoesNotCompleteImmediately()
        {
            // Arrange
            Task<int> task = DelayResult(
                123,
                TimeSpan.FromMilliseconds(300));

            // Assert before awaiting
            Assert.False(task.IsCompleted);

            // Act
            int result = await task;

            // Final Assert
            Assert.Equal(123, result);
        }
    }
}