using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ConcurrencyCookbook.Tests
{
    public class RetryOnceAfterFailureTests
    {
        private async Task<T> RetryOnce<T>(Func<Task<T>> operation)
        {
            try
            {
                return await operation();
            }
            catch
            { 
            }

            return await operation();
        }

        [Fact]
        public async Task RetriesAfterFailure()
        {
            int attempts = 0;

            int result = await RetryOnce(async () =>
            {
                attempts++;

                if (attempts == 1)
                    throw new Exception();

                await Task.Delay(10);

                return 5;
            });

            Assert.Equal(5, result);
        }

        [Fact]
        public async Task ExecutesExactlyTwice()
        {
            int attempts = 0;

            await RetryOnce(async () =>
            {
                attempts++;

                if (attempts == 1)
                    throw new Exception();

                return 1;
            });

            Assert.Equal(2, attempts);
        }

        [Fact]
        public async Task DoesNotRetryOnSuccess()
        {
            int attempts = 0;

            int result = await RetryOnce(async () =>
            {
                attempts++;
                return 10;
            });

            Assert.Equal(1, attempts);
            Assert.Equal(10, result);
        }

        [Fact]
        public async Task ThrowsIfBothAttemptsFail()
        {
            await Assert.ThrowsAsync<Exception>(
                async () =>
                {
                    await RetryOnce<int>(async () =>
                    {
                        await Task.Delay(10);
                        throw new Exception();
                    });
                });
        }

        [Fact]
        public async Task SupportsAsyncOperations()
        {
            int result = await RetryOnce(async () =>
            {
                await Task.Delay(50);
                return 99;
            });

            Assert.Equal(99, result);
        }
    }
}
