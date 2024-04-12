using System.Threading.Channels;
using MinimalMediator.Abstractions;

namespace TestSampleAot;

public class Worker(ILogger<Worker> logger, IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        
        await Publish(mediator);
        await SendMessage(mediator);
        await SendStream(mediator);
        await ReceiveStream(mediator);
    }
    
    private async Task Publish(IMediator mediator)
    {
        await mediator.PublishAsync(new TestMessage("Hello World!"));
    }

    private async Task SendMessage(IMediator mediator)
    {
       var response = await mediator.SendAsync<TestMessage, TestResponse>(new TestMessage("Hello World!"));
         logger.LogInformation("Response: {Response}", response);
    }

    private async Task SendStream(IMediator mediator)
    {
        // IAsyncEnumerable
        async IAsyncEnumerable<TestMessage> StreamAsync()
        {
            for (var i = 0; i < 10; i++)
            {
                yield return new TestMessage($"Hello World! {i}");
                await Task.Delay(1);
            }
        }
        
        var response = await mediator.SendStreamAsync<TestMessage, TestResponse>(StreamAsync());
        
        logger.LogInformation("Response from IAsyncEnumerable: {Response}", response);
        
        
        // ChannelReader
        var channel = Channel.CreateUnbounded<TestMessage>();
        
        _ = Task.Run(async () =>
        {
            for (var i = 0; i < 10; i++)
            {
                await channel.Writer.WriteAsync(new TestMessage($"Hello World! {i}"));
                await Task.Delay(1);
            }
            
            channel.Writer.Complete();
        });
        
        response = await mediator.SendStreamAsync<TestMessage, TestResponse>(channel.Reader);
        
        logger.LogInformation("Response from ChannelReader: {Response}", response);
    }

    private async Task ReceiveStream(IMediator mediator)
    {
        // IAsyncEnumerable
        var asyncStream = mediator.ReceiveStreamAsync<TestMessage, TestResponse>(new TestMessage("Hello World!"));
        await foreach (var item in asyncStream)
        {
            logger.LogInformation("ReceiveStreamAsync message: {Message}", item);
        }
        
        // ChannelReader
        var channelStream = await mediator.ReceiveChannelStreamAsync<TestMessage,TestResponse>(new TestMessage("Hello World!"));
        while (await channelStream.WaitToReadAsync())
        {
            while (channelStream.TryRead(out var item))
            {
                logger.LogInformation("ReceiveChannelStreamAsync message: {Message}", item);
            }
        }
    }
}