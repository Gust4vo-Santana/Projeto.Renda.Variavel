using Polly;
using Polly.CircuitBreaker;

namespace Quotes.Consumer.AddNewQuoteWorkerService.ResiliencePolicies
{
    public static class CircuitBreakerPolicyProvider
    {
        public static AsyncCircuitBreakerPolicy GetCircuitBreakerPolicy(int exceptionsAllowedBeforeBreaking, int durationOfBreakSeconds, ILogger logger)
        {
            return Policy.Handle<Exception>()
                         .CircuitBreakerAsync(
                            exceptionsAllowedBeforeBreaking: exceptionsAllowedBeforeBreaking,
                            durationOfBreak: TimeSpan.FromSeconds(durationOfBreakSeconds),
                            onBreak: (exception, duration) =>
                            {
                                logger.LogError(exception, "Circuit breaker activated due to error: {Error} for {Duration} minutes", exception.Message, duration.TotalMinutes);
                            },
                            onReset: () =>
                            {
                                logger.LogInformation("Circuit breaker reset.");
                            }
                         );
        }
    }
}
