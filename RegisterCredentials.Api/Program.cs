
using HealthChecks.MongoDb;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RegisterCredentials.Api.Middlewares;
using RegisterCredentials.Infra.Extensions;
using RegisterCredentials.Infra.Settings;
using Serilog;

//Logs Config
var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).Enrich.FromLogContext().CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.Configure<Database>(builder.Configuration.GetSection("Database"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureMongoDbRepositories(builder.Configuration);

builder.Services.AddHealthChecksUI().AddInMemoryStorage();
builder.Services
    .AddHealthChecks()
    .AddMongoDb(
        mongodbConnectionString: MongoDbConnectionExtensions.GetMongoDbConnectionString(builder.Configuration).connectionString,
        name: "MongoDB",
        failureStatus: HealthStatus.Unhealthy,
        tags: new String[] { "database", "mongo" }
    ); ;

builder.Services.AddLogging();
builder.Host.UseSerilog();
//Add database connection 

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

app
    .UseHealthChecks("/health", new HealthCheckOptions()
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    })
    .UseHealthChecksUI(options =>
    {
        options.UIPath = "/dashboard";
    });

app.MapControllers();
app.Run();