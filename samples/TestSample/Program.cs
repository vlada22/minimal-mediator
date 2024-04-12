using Microsoft.Extensions.DependencyInjection.Extensions;
using MinimalMediator.Core.Messaging;
using MinimalMediator.Core.Middleware;
using TestSample;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<TransientService>();
builder.Services.AddScoped<ScopedService>();

// Register Mediator with scoped lifetime, since we are going to call it from a BackgroundService (Worker) which is registered as a singleton.
builder.Services.AddMinimalMediator(config => config.UseSourceGenerator(), ServiceLifetime.Scoped);

// Uncomment the line below to use reflection instead of source generator
//builder.Services.AddMinimalMediator(config => config.UseReflection(typeof(Program)), ServiceLifetime.Scoped);

builder.Services.AddHostedService<Worker>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();