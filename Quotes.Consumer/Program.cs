using Application.UseCases.Quote.AddNewQuote;
using Infrastructure.MySql.Installers;
using Quotes.Consumer.AddNewQuoteWorkerService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddMySql(builder.Configuration);
builder.Services.AddAddNewQuoteUseCase();

var host = builder.Build();
host.Run();
