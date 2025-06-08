using FarmProject.API.DependencyInjection;
using FarmProject.API.Middlewares;
using FarmProject.Application.Common;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

builder.Services.AddFarmInfrastructure(
    builder.Configuration.GetConnectionString("FarmContext")!
);

builder.Services.AddScoped<LoggingHelper>();

builder.Services.AddEventArchitecture();
builder.Services.AddFarmServices();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseTiming();

app.UseErrorHandling();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();
