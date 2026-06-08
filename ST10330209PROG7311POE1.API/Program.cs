using Microsoft.EntityFrameworkCore;
using ST10330209PROG7311POE1.Data;
using ST10330209PROG7311POE1.API.Repositories;
using ST10330209PROG7311POE1.API.Services;
using ST10330209PROG7311POE1.Patterns.Strategy;
using ST10330209PROG7311POE1.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<IContractService, ContractService>();

builder.Services.AddHttpClient();
builder.Services.AddTransient<LiveExchangeRateStrategy>();
builder.Services.AddTransient<FallbackExchangeRateStrategy>();
builder.Services.AddScoped<CurrencyService>(provider =>
{
    var httpClient = provider.GetRequiredService<HttpClient>();
    var liveStrategy = new LiveExchangeRateStrategy(httpClient);
    return new CurrencyService(liveStrategy);
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }