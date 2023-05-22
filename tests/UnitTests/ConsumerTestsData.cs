using Microsoft.Extensions.DependencyInjection;
using MinimalMediator.Abstractions.Pipeline;
using MinimalMediator.Core.Context;
using MinimalMediator.Core.Messaging;
using MinimalMediator.Core.Middleware;

namespace UnitTests;

public record ConsumerMessage(string Message);

public class ConsumerAfterMiddleware : IAfterPublishMiddleware<ConsumerMessage>
{
    private readonly SharedService _sharedService;
    private readonly LifeTimeService _lifeTimeService;

    public ConsumerAfterMiddleware(SharedService sharedService, LifeTimeService lifeTimeService)
    {
        _sharedService = sharedService;
        _lifeTimeService = lifeTimeService;
    }

    public Task InvokeAsync(PostProcessMiddlewareContext<ConsumerMessage> context, IPipe<PostProcessMiddlewareContext<ConsumerMessage>, ConsumerMessage> next, CancellationToken cancellationToken)
    {
        _sharedService.IncrementCount();
        _sharedService.AddMessage("ConsumerAfterMiddleware");
        _sharedService.AddId(_lifeTimeService.Id);
        
        return next.InvokeAsync(context, cancellationToken);
    }
}

[MinimalMediator(Order = 2)]
public class ConsumerBeforeMiddleware1 : IBeforePublishMiddleware<ConsumerMessage>
{
    private readonly SharedService _sharedService;
    private readonly LifeTimeService _lifeTimeService;

    public ConsumerBeforeMiddleware1(SharedService sharedService, LifeTimeService lifeTimeService)
    {
        _sharedService = sharedService;
        _lifeTimeService = lifeTimeService;
    }
    
    public Task InvokeAsync(PreProcessMiddlewareContext<ConsumerMessage> context, IPipe<PreProcessMiddlewareContext<ConsumerMessage>, ConsumerMessage> next, CancellationToken cancellationToken)
    {
        _sharedService.IncrementCount();
        _sharedService.AddMessage("ConsumerBeforeMiddleware1");
        _sharedService.AddId(_lifeTimeService.Id);
        
        return next.InvokeAsync(context, cancellationToken);
    }
}

[MinimalMediator(Order = 1)]
public class ConsumerBeforeMiddleware2 : IBeforePublishMiddleware<ConsumerMessage>
{
    private readonly SharedService _sharedService;
    private readonly LifeTimeService _lifeTimeService;

    public ConsumerBeforeMiddleware2(SharedService sharedService, LifeTimeService lifeTimeService)
    {
        _sharedService = sharedService;
        _lifeTimeService = lifeTimeService;
    }
    
    public Task InvokeAsync(PreProcessMiddlewareContext<ConsumerMessage> context, IPipe<PreProcessMiddlewareContext<ConsumerMessage>, ConsumerMessage> next, CancellationToken cancellationToken)
    {
        _sharedService.IncrementCount();
        _sharedService.AddMessage("ConsumerBeforeMiddleware2");
        _sharedService.AddId(_lifeTimeService.Id);
        
        return next.InvokeAsync(context, cancellationToken);
    }
}

public class ConsumerTest : IConsumer<ConsumerMessage>
{
    private readonly SharedService _sharedService;
    private readonly LifeTimeService _lifeTimeService;

    public ConsumerTest(SharedService sharedService, LifeTimeService lifeTimeService)
    {
        _sharedService = sharedService;
        _lifeTimeService = lifeTimeService;
    }
    
    public Task HandleAsync(PublishMiddlewareContext<ConsumerMessage> context, CancellationToken cancellationToken)
    {
        _sharedService.IncrementCount();
        _sharedService.AddMessage("ConsumerTest");
        _sharedService.AddId(_lifeTimeService.Id);
        
        return Task.CompletedTask;
    }
}