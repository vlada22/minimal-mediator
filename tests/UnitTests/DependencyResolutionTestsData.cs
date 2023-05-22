using System.Threading.Channels;
using MinimalMediator.Abstractions.Pipeline;
using MinimalMediator.Core.Context;
using MinimalMediator.Core.Messaging;
using MinimalMediator.Core.Middleware;

namespace UnitTests;

public record TestMessage(string Value);
    
public record TestResponse(string Value);

public class LifeTimeService
{
    public string Id { get; } = Guid.NewGuid().ToString();
}

public class SharedService
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly List<string> _messages = new();
    private readonly List<string> _ids = new();
    private readonly List<object> _handlerData = new();
    private int _count;
    
    public IReadOnlyList<string> Messages => _messages;
    public IReadOnlyList<string> Ids => _ids;
    public IReadOnlyList<object> HandlerData => _handlerData;
    public int Count => _count;
    
    public void AddHandlerData(object data)
    {
        _semaphore.Wait();
        
        _handlerData.Add(data);
        
        _semaphore.Release();
    }
    
    public void AddId(string id)
    {
        _semaphore.Wait();
        
        _ids.Add(id);
        
        _semaphore.Release();
    }
    
    public void AddMessage(string message)
    {
        _semaphore.Wait();
        
        _messages.Add(message);
        
        _semaphore.Release();
    }
    
    public void IncrementCount()
    {
        Interlocked.Increment(ref _count);
    }
}

public class BeforeMiddleware1 : IBeforePublishMiddleware<TestMessage>
{
    private readonly SharedService _sharedService;
    private readonly LifeTimeService _lifeTimeService;

    public BeforeMiddleware1(SharedService sharedService, LifeTimeService lifeTimeService)
    {
        _sharedService = sharedService;
        _lifeTimeService = lifeTimeService;
    }

    public Task InvokeAsync(PreProcessMiddlewareContext<TestMessage> context, IPipe<PreProcessMiddlewareContext<TestMessage>, TestMessage> next, CancellationToken cancellationToken)
    {
        _sharedService.IncrementCount();
        _sharedService.AddMessage("BeforeMiddleware1");
        _sharedService.AddId(_lifeTimeService.Id);
        
        return next.InvokeAsync(context, cancellationToken);
    }
}

public class BeforeMiddleware2 : IBeforePublishMiddleware<TestMessage>
{
    private readonly SharedService _sharedService;
    private readonly LifeTimeService _lifeTimeService;

    public BeforeMiddleware2(SharedService sharedService, LifeTimeService lifeTimeService)
    {
        _sharedService = sharedService;
        _lifeTimeService = lifeTimeService;
    }
    
    public Task InvokeAsync(PreProcessMiddlewareContext<TestMessage> context, IPipe<PreProcessMiddlewareContext<TestMessage>, TestMessage> next, CancellationToken cancellationToken)
    {
        _sharedService.IncrementCount();
        _sharedService.AddMessage("BeforeMiddleware2");
        _sharedService.AddId(_lifeTimeService.Id);
        
        return next.InvokeAsync(context, cancellationToken);
    }
}
    
public class AfterMiddleware1 : IAfterPublishMiddleware<TestMessage>
{
    private readonly SharedService _sharedService;
    private readonly LifeTimeService _lifeTimeService;

    public AfterMiddleware1(SharedService sharedService, LifeTimeService lifeTimeService)
    {
        _sharedService = sharedService;
        _lifeTimeService = lifeTimeService;
    }
    
    public Task InvokeAsync(PostProcessMiddlewareContext<TestMessage> context, IPipe<PostProcessMiddlewareContext<TestMessage>, TestMessage> next, CancellationToken cancellationToken)
    {
        _sharedService.IncrementCount();
        _sharedService.AddMessage("AfterMiddleware1");
        _sharedService.AddId(_lifeTimeService.Id);
        
        return next.InvokeAsync(context, cancellationToken);
    }
}

public class AfterMiddleware2 : IAfterPublishMiddleware<TestMessage>
{
    private readonly SharedService _sharedService;
    private readonly LifeTimeService _lifeTimeService;

    public AfterMiddleware2(SharedService sharedService, LifeTimeService lifeTimeService)
    {
        _sharedService = sharedService;
        _lifeTimeService = lifeTimeService;
    }
    
    public Task InvokeAsync(PostProcessMiddlewareContext<TestMessage> context, IPipe<PostProcessMiddlewareContext<TestMessage>, TestMessage> next, CancellationToken cancellationToken)
    {
        _sharedService.IncrementCount();
        _sharedService.AddMessage("AfterMiddleware2");
        _sharedService.AddId(_lifeTimeService.Id);

        return next.InvokeAsync(context, cancellationToken);
    }
}

public class ExceptionMiddleware : IExceptionHandlerMiddleware<TestMessage>
{
    private readonly SharedService _sharedService;
    private readonly LifeTimeService _lifeTimeService;

    public ExceptionMiddleware(SharedService sharedService, LifeTimeService lifeTimeService)
    {
        _sharedService = sharedService;
        _lifeTimeService = lifeTimeService;
    }
    
    public Task InvokeAsync(ExceptionHandlerMiddlewareContext<TestMessage> context, IPipe<ExceptionHandlerMiddlewareContext<TestMessage>, TestMessage> next, CancellationToken cancellationToken)
    {
        _sharedService.IncrementCount();
        _sharedService.AddMessage("ExceptionMiddleware");
        _sharedService.AddId(_lifeTimeService.Id);

        return next.InvokeAsync(context, cancellationToken);
    }
}

public class Consumer1 : IConsumer<TestMessage>
{
    private readonly SharedService _sharedService;
    private readonly LifeTimeService _lifeTimeService;

    public Consumer1(SharedService sharedService, LifeTimeService lifeTimeService)
    {
        _sharedService = sharedService;
        _lifeTimeService = lifeTimeService;
    }
    
    public Task HandleAsync(PublishMiddlewareContext<TestMessage> context, CancellationToken cancellationToken)
    {
        _sharedService.IncrementCount();
        _sharedService.AddMessage("Consumer1");
        _sharedService.AddId(_lifeTimeService.Id);
        
        return Task.CompletedTask;
    }
}

public class Consumer2 : IConsumer<TestMessage>
{
    private readonly SharedService _sharedService;
    private readonly LifeTimeService _lifeTimeService;

    public Consumer2(SharedService sharedService, LifeTimeService lifeTimeService)
    {
        _sharedService = sharedService;
        _lifeTimeService = lifeTimeService;
    }
    
    public Task HandleAsync(PublishMiddlewareContext<TestMessage> context, CancellationToken cancellationToken)
    {
        _sharedService.IncrementCount();
        _sharedService.AddMessage("Consumer2");
        _sharedService.AddId(_lifeTimeService.Id);
        
        return Task.CompletedTask;
    }
}

public class Receiver1 : IReceiver<TestMessage, TestResponse>
{
    private readonly SharedService _sharedService;
    private readonly LifeTimeService _lifeTimeService;

    public Receiver1(SharedService sharedService, LifeTimeService lifeTimeService)
    {
        _sharedService = sharedService;
        _lifeTimeService = lifeTimeService;
    }
    
    public Task<TestResponse?> ReceiveAsync(TestMessage message, CancellationToken cancellationToken)
    {
        _sharedService.IncrementCount();
        _sharedService.AddMessage("Receiver1");
        _sharedService.AddId(_lifeTimeService.Id);
        
        return Task.FromResult<TestResponse?>(null);
    }
}

public class Receiver2 : IReceiverStreamAsync<TestMessage, TestResponse>
{
    private readonly SharedService _sharedService;
    private readonly LifeTimeService _lifeTimeService;

    public Receiver2(SharedService sharedService, LifeTimeService lifeTimeService)
    {
        _sharedService = sharedService;
        _lifeTimeService = lifeTimeService;
    }
    
    public Task<TestResponse?> ReceiveAsync(IAsyncEnumerable<TestMessage> stream, CancellationToken cancellationToken)
    {
        _sharedService.IncrementCount();
        _sharedService.AddMessage("Receiver2");
        _sharedService.AddId(_lifeTimeService.Id);
        
        return Task.FromResult<TestResponse?>(null);
    }
}

public class Receiver3 : IReceiverStreamChannel<TestMessage, TestResponse>
{
    private readonly SharedService _sharedService;
    private readonly LifeTimeService _lifeTimeService;

    public Receiver3(SharedService sharedService, LifeTimeService lifeTimeService)
    {
        _sharedService = sharedService;
        _lifeTimeService = lifeTimeService;
    }
    
    public Task<TestResponse?> ReceiveAsync(ChannelReader<TestMessage> reader, CancellationToken cancellationToken)
    {
        _sharedService.IncrementCount();
        _sharedService.AddMessage("Receiver3");
        _sharedService.AddId(_lifeTimeService.Id);
        
        return Task.FromResult<TestResponse?>(null);
    }
}

public class Receiver4 : IReceiverConsumeStreamChannel<TestMessage, TestResponse>
{
    private readonly SharedService _sharedService;
    private readonly LifeTimeService _lifeTimeService;

    public Receiver4(SharedService sharedService, LifeTimeService lifeTimeService)
    {
        _sharedService = sharedService;
        _lifeTimeService = lifeTimeService;
    }
    
    public Task<ChannelReader<TestResponse>> ReceiveAsync(TestMessage message, CancellationToken cancellationToken)
    {
        _sharedService.IncrementCount();
        _sharedService.AddMessage("Receiver4");
        _sharedService.AddId(_lifeTimeService.Id);
        
        return Task.FromResult<ChannelReader<TestResponse>>(null!);
    }
}

public class Receiver5 : IReceiverConsumeStreamAsync<TestMessage, TestResponse>
{
    private readonly SharedService _sharedService;
    private readonly LifeTimeService _lifeTimeService;

    public Receiver5(SharedService sharedService, LifeTimeService lifeTimeService)
    {
        _sharedService = sharedService;
        _lifeTimeService = lifeTimeService;
    }
    
    public IAsyncEnumerable<TestResponse> ReceiveAsync(TestMessage message, CancellationToken cancellationToken)
    {
        _sharedService.IncrementCount();
        _sharedService.AddMessage("Receiver5");
        _sharedService.AddId(_lifeTimeService.Id);

        return null!;
    }
}