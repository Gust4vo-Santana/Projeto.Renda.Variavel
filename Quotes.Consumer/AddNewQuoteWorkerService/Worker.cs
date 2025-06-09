using Application.UseCases.Quote.AddNewQuote;
using Confluent.Kafka;

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

            var config = new ConsumerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
                GroupId = "quotes-worker-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = BuildConsumer(config);
            consumer.Subscribe(topic);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var result = consumer.Consume(stoppingToken);
                    var message = result.Message.Value;

                    _logger.LogInformation("Received message from topic {Topic} | Partition {Partition} | Offset {Offset}",
                        result.Topic, result.Partition, result.Offset);

                    try
                    {
                        await _addNewQuoteUseCase.ExecuteAsync(message.MapToInput(), stoppingToken);

                        _logger.LogInformation("Successfully processed quote: {@Quote}", message);
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

        private IConsumer<Ignore, AddNewQuoteMessage> BuildConsumer(ConsumerConfig config)
        {
            return new ConsumerBuilder<Ignore, AddNewQuoteMessage>(config)
                .SetValueDeserializer(new MessageDeserializer<AddNewQuoteMessage>())
                .Build();
        }
    }
}
