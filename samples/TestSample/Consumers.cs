using MinimalMediator.Abstractions.Context;
using MinimalMediator.Core.Context;
using MinimalMediator.Core.Messaging;

namespace TestSample;

public class Consumer1 : IConsumer<TestContext>
{
    private readonly ILogger<Consumer1> _logger;

    public Consumer1(ILogger<Consumer1> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(PublishMiddlewareContext<TestContext> context, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Consumer1: {Message}", context.Message);
        return Task.CompletedTask;
    }
}

public class Consumer2 : IConsumer<TestContext>
{
    private readonly ILogger<Consumer2> _logger;

    public Consumer2(ILogger<Consumer2> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(PublishMiddlewareContext<TestContext> context, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Consumer2: {Message}", context.Message);
        return Task.CompletedTask;
    }
}