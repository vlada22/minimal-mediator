using MinimalMediator.Abstractions.Pipeline;
using MinimalMediator.Core.Context;
using MinimalMediator.Core.Messaging;
using MinimalMediator.Core.Middleware;

namespace TestSampleAot;

public class Consumer1(ILogger<Consumer1> logger) : IConsumer<TestMessage>
{
    public Task HandleAsync(PublishMiddlewareContext<TestMessage> context, CancellationToken cancellationToken)
    {
        logger.LogInformation("Consumer1 {Value}", context.Message?.Value);
        return Task.CompletedTask;
    }
}

public class Consumer2(ILogger<Consumer2> logger) : IConsumer<TestMessage>
{
    public Task HandleAsync(PublishMiddlewareContext<TestMessage> context, CancellationToken cancellationToken)
    {
        logger.LogInformation("Consumer2 {Value}", context.Message?.Value);
        return Task.CompletedTask;
    }
}

[MinimalMediator(Order = 1)]
public class BeforeMiddleware1(ILogger<BeforeMiddleware1> logger, TransientService transientService)
    : IBeforePublishMiddleware<TestMessage>
{
    public Task InvokeAsync(PreProcessMiddlewareContext<TestMessage> context, IPipe<PreProcessMiddlewareContext<TestMessage>, TestMessage> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("TransientService: {TransientServiceId}", transientService.Id);
        return next.InvokeAsync(context, cancellationToken);
    }
}

[MinimalMediator(Order = 2)]
public class BeforeMiddleware2(ILogger<BeforeMiddleware2> logger, ScopedService scopedService)
    : IBeforePublishMiddleware<TestMessage>
{
    public Task InvokeAsync(PreProcessMiddlewareContext<TestMessage> context, IPipe<PreProcessMiddlewareContext<TestMessage>, TestMessage> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("ScopedService: {ScopedServiceId}", scopedService.Id);
        return next.InvokeAsync(context, cancellationToken);
    }
}

[MinimalMediator(Order = 1)]
public class AfterMiddleware1(ILogger<AfterMiddleware1> logger) : IAfterPublishMiddleware<TestMessage>
{
    public Task InvokeAsync(PostProcessMiddlewareContext<TestMessage> context, IPipe<PostProcessMiddlewareContext<TestMessage>, TestMessage> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("AfterMiddleware1");
        return next.InvokeAsync(context, cancellationToken);
    }
}

[MinimalMediator(Order = 2)]
public class AfterMiddleware2(ILogger<AfterMiddleware2> logger) : IAfterPublishMiddleware<TestMessage>
{
    public Task InvokeAsync(PostProcessMiddlewareContext<TestMessage> context, IPipe<PostProcessMiddlewareContext<TestMessage>, TestMessage> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("AfterMiddleware2");
        return next.InvokeAsync(context, cancellationToken);
    }
}

public class ExceptionMiddleware(ILogger<ExceptionMiddleware> logger) : IExceptionHandlerMiddleware<TestMessage>
{
    public Task InvokeAsync(ExceptionHandlerMiddlewareContext<TestMessage> context, IPipe<ExceptionHandlerMiddlewareContext<TestMessage>, TestMessage> next, CancellationToken cancellationToken)
    {
        logger.LogError(context.Exception, "ExceptionMiddleware");
        return next.InvokeAsync(context, cancellationToken);
    }
}