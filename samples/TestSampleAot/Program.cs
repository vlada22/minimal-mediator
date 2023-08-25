using MinimalMediator.Core.Messaging;
using MinimalMediator.Core.Middleware;
using TestSampleAot;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddTransient<TransientService>();
builder.Services.AddScoped<ScopedService>();

// Register Mediator with scoped lifetime, since we are going to call it from a BackgroundService (Worker) which is registered as a singleton.

// use source generated DI
//builder.Services.AddMinimalMediator(config=>config.UseSourceGenerator(), ServiceLifetime.Scoped);

// or manually register
builder.Services.AddMinimalMediator(config =>
{
    config.AddConsumer(typeof(IConsumer<TestMessage>), typeof(Consumer1));
    config.AddConsumer(typeof(IConsumer<TestMessage>), typeof(Consumer2));
    config.AddMiddleware(typeof(IBeforePublishMiddleware<TestMessage>), typeof(BeforeMiddleware1));
    config.AddMiddleware(typeof(IBeforePublishMiddleware<TestMessage>), typeof(BeforeMiddleware2));
    config.AddMiddleware(typeof(IAfterPublishMiddleware<TestMessage>), typeof(AfterMiddleware1));
    config.AddMiddleware(typeof(IAfterPublishMiddleware<TestMessage>), typeof(AfterMiddleware2));
    config.AddMiddleware(typeof(IExceptionHandlerMiddleware<TestMessage>), typeof(ExceptionMiddleware));
    config.AddReceiver(typeof(IReceiver<TestMessage, TestResponse>), typeof(ReceiverTest));
    config.AddReceiver(typeof(IReceiverStreamAsync<TestMessage, TestResponse>), typeof(ReceiverStream));
    config.AddReceiver(typeof(IReceiverStreamChannel<TestMessage, TestResponse>), typeof(ReceiverStreamChannel));
    config.AddReceiver(typeof(IReceiverConsumeStreamAsync<TestMessage, TestResponse>), typeof(ReceiverConsumeStream));
    config.AddReceiver(typeof(IReceiverConsumeStreamChannel<TestMessage, TestResponse>), typeof(ReceiverConsumeChannel));
});

builder.Services.AddHostedService<Worker>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();