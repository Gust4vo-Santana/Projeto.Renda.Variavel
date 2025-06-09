using Application.UseCases.Quote.AddNewQuote;
using Infrastructure.MySql.Installers;
using Quotes.Consumer.AddNewQuoteWorkerService;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);

builder.Services.AddHostedService<Worker>();
builder.Services.AddMySql(builder.Configuration);
builder.Services.AddAddNewQuoteUseCase();

try
{
    Log.Information("Starting host");
    var host = builder.Build();
    host.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
