
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RegisterCredentials.Api.Middlewares;
using RegisterCredentials.Infra.Settings;
using Serilog;
using StoneCo.FinancialPositionHub.Infra.Settings;

//Logs Config
var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).Enrich.FromLogContext().CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = config.GetValue<string>("Database:ConnectionString");
builder.Services.AddHealthChecks().AddMongoDb(mongodbConnectionString: connectionString, name: "MongoDB", tags: new String[] {"db", "database", "mongo"});
builder.Services.AddHealthChecksUI().AddInMemoryStorage();
builder.Services.AddLogging();
builder.Host.UseSerilog();

//Add database connection 
builder.Services.ConfigureMongoDbRepositories(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<LogMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions()
{
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    },
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseHealthChecksUI(options =>
{
    options.UIPath = "/Dashboard";
});
app.Run();