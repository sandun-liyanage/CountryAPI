using Core.AppSettings;
using RepositoryLayer.Repositories;
using Serilog;
using ServiceLayer.Services;

var builder = WebApplication.CreateBuilder(args);

string? connStr = Environment.GetEnvironmentVariable("COUNTRY_DB_CONN")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File("Logs/log-.txt",
                  rollingInterval: RollingInterval.Day,
                  retainedFileCountLimit: 7, 
                  fileSizeLimitBytes: 10_000_000,
                  rollOnFileSizeLimit: true)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddSingleton(new AppSettings { ConnectionString = connStr! });
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
