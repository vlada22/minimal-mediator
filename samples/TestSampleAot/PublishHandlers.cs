using MinimalMediator.Abstractions.Pipeline;
using MinimalMediator.Core.Context;
using MinimalMediator.Core.Messaging;
using MinimalMediator.Core.Middleware;

namespace TestSampleAot;

public class Consumer1 : IConsumer<TestMessage>
{
    private readonly ILogger<Consumer1> _logger;

    public Consumer1(ILogger<Consumer1> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(PublishMiddlewareContext<TestMessage> context, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Consumer1 {Value}", context.Message?.Value);
        return Task.CompletedTask;
    }
}

public class Consumer2 : IConsumer<TestMessage>
{
    private readonly ILogger<Consumer2> _logger;

    public Consumer2(ILogger<Consumer2> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(PublishMiddlewareContext<TestMessage> context, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Consumer2 {Value}", context.Message?.Value);
        return Task.CompletedTask;
    }
}

[MinimalMediator(Order = 1)]
public class BeforeMiddleware1 : IBeforePublishMiddleware<TestMessage>
{
    private readonly ILogger<BeforeMiddleware1> _logger;
    private readonly TransientService _transientService;

    public BeforeMiddleware1(ILogger<BeforeMiddleware1> logger, TransientService transientService)
    {
        _logger = logger;
        _transientService = transientService;
    }

    public Task InvokeAsync(PreProcessMiddlewareContext<TestMessage> context, IPipe<PreProcessMiddlewareContext<TestMessage>, TestMessage> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("TransientService: {TransientServiceId}", _transientService.Id);
        return next.InvokeAsync(context, cancellationToken);
    }
}

[MinimalMediator(Order = 2)]
public class BeforeMiddleware2 : IBeforePublishMiddleware<TestMessage>
{
    private readonly ILogger<BeforeMiddleware2> _logger;
    private readonly ScopedService _scopedService;

    public BeforeMiddleware2(ILogger<BeforeMiddleware2> logger, ScopedService scopedService)
    {
        _logger = logger;
        _scopedService = scopedService;
    }

    public Task InvokeAsync(PreProcessMiddlewareContext<TestMessage> context, IPipe<PreProcessMiddlewareContext<TestMessage>, TestMessage> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("ScopedService: {ScopedServiceId}", _scopedService.Id);
        return next.InvokeAsync(context, cancellationToken);
    }
}

[MinimalMediator(Order = 1)]
public class AfterMiddleware1 : IAfterPublishMiddleware<TestMessage>
{
    private readonly ILogger<AfterMiddleware1> _logger;

    public AfterMiddleware1(ILogger<AfterMiddleware1> logger)
    {
        _logger = logger;
    }

    public Task InvokeAsync(PostProcessMiddlewareContext<TestMessage> context, IPipe<PostProcessMiddlewareContext<TestMessage>, TestMessage> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("AfterMiddleware1");
        return next.InvokeAsync(context, cancellationToken);
    }
}

[MinimalMediator(Order = 2)]
public class AfterMiddleware2 : IAfterPublishMiddleware<TestMessage>
{
    private readonly ILogger<AfterMiddleware2> _logger;

    public AfterMiddleware2(ILogger<AfterMiddleware2> logger)
    {
        _logger = logger;
    }

    public Task InvokeAsync(PostProcessMiddlewareContext<TestMessage> context, IPipe<PostProcessMiddlewareContext<TestMessage>, TestMessage> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("AfterMiddleware2");
        return next.InvokeAsync(context, cancellationToken);
    }
}

public class ExceptionMiddleware : IExceptionHandlerMiddleware<TestMessage>
{
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
    }

    public Task InvokeAsync(ExceptionHandlerMiddlewareContext<TestMessage> context, IPipe<ExceptionHandlerMiddlewareContext<TestMessage>, TestMessage> next, CancellationToken cancellationToken)
    {
        _logger.LogError(context.Exception, "ExceptionMiddleware");
        return next.InvokeAsync(context, cancellationToken);
    }
}