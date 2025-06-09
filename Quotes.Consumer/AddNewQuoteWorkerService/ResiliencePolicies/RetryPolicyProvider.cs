using Polly;
using Polly.Retry;

namespace Quotes.Consumer.AddNewQuoteWorkerService.ResiliencePolicies
{
    public static class RetryPolicyProvider
    {
        public static AsyncRetryPolicy GetRetryPolicy(int retryCount, int sleepDurationSeconds, ILogger logger)
        {
            return Policy.Handle<Exception>()
                         .WaitAndRetryAsync(
                            retryCount: retryCount,
                            sleepDurationProvider: attempt => TimeSpan.FromSeconds(sleepDurationSeconds),
                            onRetry: (exception, timeSpan, context) =>
                            {
                                logger.LogWarning(exception, "Retrying due to error: {Error} after {TimeSpan} seconds", exception.Message, timeSpan.TotalSeconds);
                            }
                         );
        }
    }
}
