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

public class BeforeMiddleware1(SharedService sharedService, LifeTimeService lifeTimeService)
    : IBeforePublishMiddleware<TestMessage>
{
    public Task InvokeAsync(PreProcessMiddlewareContext<TestMessage> context, IPipe<PreProcessMiddlewareContext<TestMessage>, TestMessage> next, CancellationToken cancellationToken)
    {
        sharedService.IncrementCount();
        sharedService.AddMessage("BeforeMiddleware1");
        sharedService.AddId(lifeTimeService.Id);
        
        return next.InvokeAsync(context, cancellationToken);
    }
}

public class BeforeMiddleware2(SharedService sharedService, LifeTimeService lifeTimeService)
    : IBeforePublishMiddleware<TestMessage>
{
    public Task InvokeAsync(PreProcessMiddlewareContext<TestMessage> context, IPipe<PreProcessMiddlewareContext<TestMessage>, TestMessage> next, CancellationToken cancellationToken)
    {
        sharedService.IncrementCount();
        sharedService.AddMessage("BeforeMiddleware2");
        sharedService.AddId(lifeTimeService.Id);
        
        return next.InvokeAsync(context, cancellationToken);
    }
}
    
public class AfterMiddleware1(SharedService sharedService, LifeTimeService lifeTimeService)
    : IAfterPublishMiddleware<TestMessage>
{
    public Task InvokeAsync(PostProcessMiddlewareContext<TestMessage> context, IPipe<PostProcessMiddlewareContext<TestMessage>, TestMessage> next, CancellationToken cancellationToken)
    {
        sharedService.IncrementCount();
        sharedService.AddMessage("AfterMiddleware1");
        sharedService.AddId(lifeTimeService.Id);
        
        return next.InvokeAsync(context, cancellationToken);
    }
}

public class AfterMiddleware2(SharedService sharedService, LifeTimeService lifeTimeService)
    : IAfterPublishMiddleware<TestMessage>
{
    public Task InvokeAsync(PostProcessMiddlewareContext<TestMessage> context, IPipe<PostProcessMiddlewareContext<TestMessage>, TestMessage> next, CancellationToken cancellationToken)
    {
        sharedService.IncrementCount();
        sharedService.AddMessage("AfterMiddleware2");
        sharedService.AddId(lifeTimeService.Id);

        return next.InvokeAsync(context, cancellationToken);
    }
}

public class ExceptionMiddleware(SharedService sharedService, LifeTimeService lifeTimeService)
    : IExceptionHandlerMiddleware<TestMessage>
{
    public Task InvokeAsync(ExceptionHandlerMiddlewareContext<TestMessage> context, IPipe<ExceptionHandlerMiddlewareContext<TestMessage>, TestMessage> next, CancellationToken cancellationToken)
    {
        sharedService.IncrementCount();
        sharedService.AddMessage("ExceptionMiddleware");
        sharedService.AddId(lifeTimeService.Id);

        return next.InvokeAsync(context, cancellationToken);
    }
}

public class Consumer1(SharedService sharedService, LifeTimeService lifeTimeService)
    : IConsumer<TestMessage>
{
    public Task HandleAsync(PublishMiddlewareContext<TestMessage> context, CancellationToken cancellationToken)
    {
        sharedService.IncrementCount();
        sharedService.AddMessage("Consumer1");
        sharedService.AddId(lifeTimeService.Id);
        
        return Task.CompletedTask;
    }
}

public class Consumer2(SharedService sharedService, LifeTimeService lifeTimeService)
    : IConsumer<TestMessage>
{
    public Task HandleAsync(PublishMiddlewareContext<TestMessage> context, CancellationToken cancellationToken)
    {
        sharedService.IncrementCount();
        sharedService.AddMessage("Consumer2");
        sharedService.AddId(lifeTimeService.Id);
        
        return Task.CompletedTask;
    }
}

public class Receiver1(SharedService sharedService, LifeTimeService lifeTimeService)
    : IReceiver<TestMessage, TestResponse>
{
    public Task<TestResponse?> ReceiveAsync(TestMessage message, CancellationToken cancellationToken)
    {
        sharedService.IncrementCount();
        sharedService.AddMessage("Receiver1");
        sharedService.AddId(lifeTimeService.Id);
        
        return Task.FromResult<TestResponse?>(null);
    }
}

public class Receiver2(SharedService sharedService, LifeTimeService lifeTimeService)
    : IReceiverStreamAsync<TestMessage, TestResponse>
{
    public Task<TestResponse?> ReceiveAsync(IAsyncEnumerable<TestMessage> stream, CancellationToken cancellationToken)
    {
        sharedService.IncrementCount();
        sharedService.AddMessage("Receiver2");
        sharedService.AddId(lifeTimeService.Id);
        
        return Task.FromResult<TestResponse?>(null);
    }
}

public class Receiver3(SharedService sharedService, LifeTimeService lifeTimeService)
    : IReceiverStreamChannel<TestMessage, TestResponse>
{
    public Task<TestResponse?> ReceiveAsync(ChannelReader<TestMessage> reader, CancellationToken cancellationToken)
    {
        sharedService.IncrementCount();
        sharedService.AddMessage("Receiver3");
        sharedService.AddId(lifeTimeService.Id);
        
        return Task.FromResult<TestResponse?>(null);
    }
}

public class Receiver4(SharedService sharedService, LifeTimeService lifeTimeService)
    : IReceiverConsumeStreamChannel<TestMessage, TestResponse>
{
    public Task<ChannelReader<TestResponse>> ReceiveAsync(TestMessage message, CancellationToken cancellationToken)
    {
        sharedService.IncrementCount();
        sharedService.AddMessage("Receiver4");
        sharedService.AddId(lifeTimeService.Id);
        
        return Task.FromResult<ChannelReader<TestResponse>>(null!);
    }
}

public class Receiver5(SharedService sharedService, LifeTimeService lifeTimeService)
    : IReceiverConsumeStreamAsync<TestMessage, TestResponse>
{
    public IAsyncEnumerable<TestResponse> ReceiveAsync(TestMessage message, CancellationToken cancellationToken)
    {
        sharedService.IncrementCount();
        sharedService.AddMessage("Receiver5");
        sharedService.AddId(lifeTimeService.Id);

        return null!;
    }
}