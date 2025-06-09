using Application.UseCases.Quote.AddNewQuote;
using Confluent.Kafka;
using Polly;
using Quotes.Consumer.AddNewQuoteWorkerService.Message;
using Quotes.Consumer.AddNewQuoteWorkerService.ResiliencePolicies;

namespace Quotes.Consumer.AddNewQuoteWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly IAddNewQuoteUseCase _addNewQuoteUseCase;
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        public Worker(IAddNewQuoteUseCase addNewQuoteUseCase,
                      ILogger<Worker> logger,
                      IConfiguration configuration)
        {
            _addNewQuoteUseCase = addNewQuoteUseCase;
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var topic = _configuration["Kafka:Topic"];
            var dlqTopic = _configuration["Kafka:DeadLetterQueueTopic"];

            using var consumer = BuildConsumer();
            consumer.Subscribe(topic);

            using var dlqProducer = BuildDlqProducer();

            try
            {
                var retryPolicy = RetryPolicyProvider.GetRetryPolicy(5, 1, _logger);
                var circuitBreakerPolicy = CircuitBreakerPolicyProvider.GetCircuitBreakerPolicy(5, 30, _logger);

                while (!stoppingToken.IsCancellationRequested)
                {
                    var result = consumer.Consume(stoppingToken);
                    var message = result.Message.Value;

                    _logger.LogInformation("Received message from topic {Topic} | Partition {Partition} | Offset {Offset}",
                        result.Topic, result.Partition, result.Offset);

                    var policy = Policy.WrapAsync(retryPolicy,
                                                  circuitBreakerPolicy,
                                                  FallbackPolicyProvider.GetFallbackPolicy(message, dlqProducer, _logger, dlqTopic!, stoppingToken));
                    try
                    {
                        await policy.ExecuteAsync(async () =>
                        {
                            await _addNewQuoteUseCase.ExecuteAsync(message.MapToInput(), stoppingToken);

                            _logger.LogInformation("Successfully processed quote: {@Quote}", message);
                        });
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing quote message: {@Quote}", message);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Worker canceled due to shutdown request.");
            }
            finally
            {
                consumer.Close();
            }
        }

        private IConsumer<Ignore, AddNewQuoteMessage> BuildConsumer()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
                GroupId = "quotes-worker-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            return new ConsumerBuilder<Ignore, AddNewQuoteMessage>(config)
                .SetValueDeserializer(new MessageDeserializer<AddNewQuoteMessage>())
                .Build();
        }

        private IProducer<Ignore, AddNewQuoteMessage> BuildDlqProducer()
        {
            var dlqConfig = new ProducerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"]
            };

            return new ProducerBuilder<Ignore, AddNewQuoteMessage>(dlqConfig)
                .SetValueSerializer(new MessageSerializer<AddNewQuoteMessage>())
                .Build();
        }
    }
}
