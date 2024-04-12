using System.Threading.Channels;
using MinimalMediator.Core.Messaging;

namespace UnitTests;

public record ReceiverMessage(string Message);
public record ReceiverResponse(string Message);

public class ReceiverTest(SharedService sharedService, LifeTimeService lifeTimeService)
    : IReceiver<ReceiverMessage, ReceiverResponse>
{
    public Task<ReceiverResponse?> ReceiveAsync(ReceiverMessage message, CancellationToken cancellationToken)
    {
        sharedService.IncrementCount();
        sharedService.AddMessage("ReceiverTest");
        sharedService.AddId(lifeTimeService.Id);
        sharedService.AddHandlerData(message);
        
        return Task.FromResult<ReceiverResponse?>(new ReceiverResponse(message.Message));
    }
}

public class ReceiverStream(SharedService sharedService, LifeTimeService lifeTimeService)
    : IReceiverStreamAsync<ReceiverMessage, ReceiverResponse>
{
    public async Task<ReceiverResponse?> ReceiveAsync(IAsyncEnumerable<ReceiverMessage> stream, CancellationToken cancellationToken)
    {
        sharedService.IncrementCount();
        sharedService.AddMessage("ReceiverStream");
        sharedService.AddId(lifeTimeService.Id);

        await foreach (var item in stream.WithCancellation(cancellationToken))
        {
            sharedService.AddHandlerData(item);
        }

        return default;
    }
}

public class ReceiverStreamChannel(SharedService sharedService, LifeTimeService lifeTimeService)
    : IReceiverStreamChannel<ReceiverMessage, ReceiverResponse>
{
    public async Task<ReceiverResponse?> ReceiveAsync(ChannelReader<ReceiverMessage> reader, CancellationToken cancellationToken)
    {
        sharedService.IncrementCount();
        sharedService.AddMessage("ReceiverStreamChannel");
        sharedService.AddId(lifeTimeService.Id);
        
        while (await reader.WaitToReadAsync(cancellationToken))
        {
            while (reader.TryRead(out var item))
            {
                sharedService.AddHandlerData(item);
            }
        }

        return default;
    }
}

public class ReceiverConsumeStream(SharedService sharedService, LifeTimeService lifeTimeService)
    : IReceiverConsumeStreamAsync<ReceiverMessage, ReceiverResponse>
{
    public IAsyncEnumerable<ReceiverResponse> ReceiveAsync(ReceiverMessage message,
        CancellationToken cancellationToken)
    {
        sharedService.IncrementCount();
        sharedService.AddMessage("ReceiverConsumeStream");
        sharedService.AddId(lifeTimeService.Id);
        sharedService.AddHandlerData(message);

        async IAsyncEnumerable<ReceiverResponse> Stream()
        {
            for (var i = 0; i < 5; i++)
            {
                yield return new ReceiverResponse(i.ToString());
                await Task.Delay(1, cancellationToken);
            }
        }
        
        return Stream();
    }
}

public class ReceiverConsumeChannel(SharedService sharedService, LifeTimeService lifeTimeService)
    : IReceiverConsumeStreamChannel<ReceiverMessage, ReceiverResponse>
{
    public Task<ChannelReader<ReceiverResponse>> ReceiveAsync(ReceiverMessage message, CancellationToken cancellationToken)
    {
        sharedService.IncrementCount();
        sharedService.AddMessage("ReceiverConsumeChannel");
        sharedService.AddId(lifeTimeService.Id);
        sharedService.AddHandlerData(message);

        var channel = Channel.CreateUnbounded<ReceiverResponse>();

        _ = Task.Run(async () =>
        {
            for (var i = 0; i < 5; i++)
            {
                await channel.Writer.WriteAsync(new ReceiverResponse(i.ToString()), cancellationToken);
                await Task.Delay(100, cancellationToken);
            }

            channel.Writer.Complete();
        }, cancellationToken);

        return Task.FromResult(channel.Reader);
    }
}