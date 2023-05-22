using Microsoft.Extensions.DependencyInjection.Extensions;
using MinimalMediator.Core.Messaging;
using MinimalMediator.Core.Middleware;
using TestSample;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ServicesB>();
builder.Services.AddTransient<ServicesC>();
builder.Services.TryAddEnumerable(ServiceDescriptor.Transient(typeof(IAfterPublishMiddleware<TestContext>), typeof(ValidationMediatorMiddleware3)));
builder.Services.TryAddEnumerable(ServiceDescriptor.Transient(typeof(IBeforePublishMiddleware<TestContext>), typeof(ValidationMediatorMiddleware)));
builder.Services.TryAddEnumerable(ServiceDescriptor.Transient(typeof(IBeforePublishMiddleware<TestContext>), typeof(ValidationMediatorMiddleware1)));
builder.Services.TryAddEnumerable(ServiceDescriptor.Transient(typeof(IAfterPublishMiddleware<TestContext>), typeof(ValidationMediatorMiddleware2)));

builder.Services.TryAddEnumerable(ServiceDescriptor.Transient(typeof(IConsumer<TestContext>), typeof(Consumer1)));
builder.Services.TryAddEnumerable(ServiceDescriptor.Transient(typeof(IConsumer<TestContext>), typeof(Consumer2)));
builder.Services.AddTransient(typeof(IReceiver<TestContext, TestResponse>), typeof(Sender1));
builder.Services.AddTransient(typeof(IReceiverStreamChannel<TestContext, TestResponse>), typeof(Sender2));
builder.Services.AddTransient(typeof(IReceiverStreamAsync<TestContext, TestResponse>), typeof(Sender3));

builder.Services.AddMinimalMediator( c=>c.UseReflection(),ServiceLifetime.Scoped);

builder.Services.AddHostedService<Worker>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();