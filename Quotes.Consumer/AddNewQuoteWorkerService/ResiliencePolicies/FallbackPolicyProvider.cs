using Confluent.Kafka;
using Polly;
using Polly.Fallback;
using Quotes.Consumer.AddNewQuoteWorkerService.Message;

namespace Quotes.Consumer.AddNewQuoteWorkerService.ResiliencePolicies
{
    public static class FallbackPolicyProvider
    {
        public static AsyncFallbackPolicy GetFallbackPolicy(AddNewQuoteMessage originalMessage, 
                                                            IProducer<Ignore, AddNewQuoteMessage> dlqProducer,
                                                            ILogger logger, 
                                                            string dlqTopic, 
                                                            CancellationToken stoppingToken)
        {
            return Policy.Handle<Exception>()
                         .FallbackAsync(
                             fallbackAction: async (stoppingToken) =>
                             {
                                 await dlqProducer.ProduceAsync(dlqTopic, 
                                                                new Message<Ignore, AddNewQuoteMessage> 
                                                                { 
                                                                    Value = originalMessage 
                                                                }, stoppingToken);
                                 logger.LogWarning("An error occurred, message sent to DLQ: {@Message}", originalMessage);
                             },
                             onFallbackAsync: async (exception) =>
                             {
                                 logger.LogError(exception, "An error occurred, executing fallback action: {Error}", exception.Message);
                                 await Task.CompletedTask;
                             }
                         );
        }
    }
}
