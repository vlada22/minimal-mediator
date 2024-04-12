using Microsoft.Extensions.DependencyInjection;
using MinimalMediator.Abstractions.Pipeline;
using MinimalMediator.Core.Context;
using MinimalMediator.Core.Messaging;
using MinimalMediator.Core.Middleware;

namespace UnitTests;

public record ConsumerMessage(string Message);

public class ConsumerAfterMiddleware(SharedService sharedService, LifeTimeService lifeTimeService)
    : IAfterPublishMiddleware<ConsumerMessage>
{
    public Task InvokeAsync(PostProcessMiddlewareContext<ConsumerMessage> context, IPipe<PostProcessMiddlewareContext<ConsumerMessage>, ConsumerMessage> next, CancellationToken cancellationToken)
    {
        sharedService.IncrementCount();
        sharedService.AddMessage("ConsumerAfterMiddleware");
        sharedService.AddId(lifeTimeService.Id);
        
        return next.InvokeAsync(context, cancellationToken);
    }
}

[MinimalMediator(Order = 2)]
public class ConsumerBeforeMiddleware1(SharedService sharedService, LifeTimeService lifeTimeService)
    : IBeforePublishMiddleware<ConsumerMessage>
{
    public Task InvokeAsync(PreProcessMiddlewareContext<ConsumerMessage> context, IPipe<PreProcessMiddlewareContext<ConsumerMessage>, ConsumerMessage> next, CancellationToken cancellationToken)
    {
        sharedService.IncrementCount();
        sharedService.AddMessage("ConsumerBeforeMiddleware1");
        sharedService.AddId(lifeTimeService.Id);
        
        return next.InvokeAsync(context, cancellationToken);
    }
}

[MinimalMediator(Order = 1)]
public class ConsumerBeforeMiddleware2(SharedService sharedService, LifeTimeService lifeTimeService)
    : IBeforePublishMiddleware<ConsumerMessage>
{
    public Task InvokeAsync(PreProcessMiddlewareContext<ConsumerMessage> context, IPipe<PreProcessMiddlewareContext<ConsumerMessage>, ConsumerMessage> next, CancellationToken cancellationToken)
    {
        sharedService.IncrementCount();
        sharedService.AddMessage("ConsumerBeforeMiddleware2");
        sharedService.AddId(lifeTimeService.Id);
        
        return next.InvokeAsync(context, cancellationToken);
    }
}

public class ConsumerTest(SharedService sharedService, LifeTimeService lifeTimeService)
    : IConsumer<ConsumerMessage>
{
    public Task HandleAsync(PublishMiddlewareContext<ConsumerMessage> context, CancellationToken cancellationToken)
    {
        sharedService.IncrementCount();
        sharedService.AddMessage("ConsumerTest");
        sharedService.AddId(lifeTimeService.Id);
        
        return Task.CompletedTask;
    }
}