using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ConcurrencyCookbook.Tests
{
    public class ImmediateCompletionForZeroDelay
    {
        //A zero delay should complete immediately instead of
        //scheduling unnecessary asynchronous work.
        private async Task<int> DelayResult(int v, TimeSpan zero)
        {
            await Task.Delay(zero);
            return v;
        }

        [Fact]
        public async Task ZeroDelayCompletesQuickly()
        {
            var sw = Stopwatch.StartNew();

            await DelayResult(1, TimeSpan.Zero);

            sw.Stop();

            Assert.True(sw.ElapsedMilliseconds < 20);
        }


        [Fact]
        public async Task ReturnsCorrectValue()
        {
            int result = await DelayResult(123,
                TimeSpan.Zero);

            Assert.Equal(123, result);
        }

        [Fact]
        public async Task NonZeroDelayStillWaits()
        {
            var sw = Stopwatch.StartNew();

            await DelayResult(1,
                TimeSpan.FromMilliseconds(200));

            sw.Stop();

            Assert.True(sw.ElapsedMilliseconds >= 180);
        }

        [Fact]
        public async Task NegativeDelayThrows()
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                async () =>
                {
                    await DelayResult(
                        1,
                        TimeSpan.FromMilliseconds(-2));
                });
        }

        [Fact]
        public void ZeroDelayMayCompleteSynchronously()
        {
            Task<int> task = DelayResult(1,
                TimeSpan.Zero);

            Assert.True(task.IsCompleted);
        }
    }
}
