using Application.UseCases.Quote.AddNewQuote;
using Quotes.Consumer.AddNewQuoteWorkerService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddAddNewQuoteUseCase();

var host = builder.Build();
host.Run();
