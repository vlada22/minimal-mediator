using Microsoft.Extensions.DependencyInjection.Extensions;
using MinimalMediator.Abstractions.Middleware;
using MinimalMediator.Core.DependencyInjection;
using TestSample;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ServicesB>();
builder.Services.AddTransient<ServicesC>();
builder.Services.TryAddEnumerable(ServiceDescriptor.Transient(typeof(IMediatorMiddleware<TestContext>), typeof(ValidationMediatorMiddleware)));
builder.Services.TryAddEnumerable(ServiceDescriptor.Transient(typeof(IMediatorMiddleware<TestContext>), typeof(ValidationMiddleware2)));
builder.Services.TryAddEnumerable(ServiceDescriptor.Transient(typeof(IMediatorMiddleware<TestContext>), typeof(ExceptionMediatorMiddleware)));
builder.Services.TryAddEnumerable(ServiceDescriptor.Transient(typeof(IMediatorMiddleware<TestContext>), typeof(ValidationMiddleware3)));

builder.Services.AddMinimalMediator(ServiceLifetime.Scoped);

builder.Services.AddHostedService<Worker>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();