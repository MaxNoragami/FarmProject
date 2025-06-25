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
builder.Services.AddSwaggerJwtAuth();

// Add CORS configuration BEFORE other services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddFarmInfrastructure(
    builder.Configuration.GetConnectionString("FarmContext")!,
    builder.Configuration.GetConnectionString("FarmIdentity")!
);

builder.Services.AddScoped<LoggingHelper>();

builder.Services.AddEventArchitecture();

builder.Services.AddValidation();

builder.Services.AddFarmServices();

builder.Services.AddAuth(builder.Configuration);

builder.Services.AddIdentityConfig();

builder.Services.AddIdentityValidation();

builder.Services.AddIdentityServices();


builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
);

var app = builder.Build();

// Configure the HTTP request pipeline.

// CORS must be FIRST middleware
app.UseCors("AllowReactApp");

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
