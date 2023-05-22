using System.Threading.Channels;
using MinimalMediator.Core.Messaging;

namespace UnitTests;

public record ReceiverMessage(string Message);
public record ReceiverResponse(string Message);

public class ReceiverTest : IReceiver<ReceiverMessage, ReceiverResponse>
{
    private readonly SharedService _sharedService;
    private readonly LifeTimeService _lifeTimeService;

    public ReceiverTest(SharedService sharedService, LifeTimeService lifeTimeService)
    {
        _sharedService = sharedService;
        _lifeTimeService = lifeTimeService;
    }
    
    public Task<ReceiverResponse?> ReceiveAsync(ReceiverMessage message, CancellationToken cancellationToken)
    {
        _sharedService.IncrementCount();
        _sharedService.AddMessage("ReceiverTest");
        _sharedService.AddId(_lifeTimeService.Id);
        _sharedService.AddHandlerData(message);
        
        return Task.FromResult<ReceiverResponse?>(new ReceiverResponse(message.Message));
    }
}

public class ReceiverStream : IReceiverStreamAsync<ReceiverMessage, ReceiverResponse>
{
    private readonly SharedService _sharedService;
    private readonly LifeTimeService _lifeTimeService;

    public ReceiverStream(SharedService sharedService, LifeTimeService lifeTimeService)
    {
        _sharedService = sharedService;
        _lifeTimeService = lifeTimeService;
    }
    
    public async Task<ReceiverResponse?> ReceiveAsync(IAsyncEnumerable<ReceiverMessage> stream, CancellationToken cancellationToken)
    {
        _sharedService.IncrementCount();
        _sharedService.AddMessage("ReceiverStream");
        _sharedService.AddId(_lifeTimeService.Id);

        await foreach (var item in stream.WithCancellation(cancellationToken))
        {
            _sharedService.AddHandlerData(item);
        }

        return default;
    }
}

public class ReceiverStreamChannel : IReceiverStreamChannel<ReceiverMessage, ReceiverResponse>
{
    private readonly SharedService _sharedService;
    private readonly LifeTimeService _lifeTimeService;

    public ReceiverStreamChannel(SharedService sharedService, LifeTimeService lifeTimeService)
    {
        _sharedService = sharedService;
        _lifeTimeService = lifeTimeService;
    }
    
    public async Task<ReceiverResponse?> ReceiveAsync(ChannelReader<ReceiverMessage> reader, CancellationToken cancellationToken)
    {
        _sharedService.IncrementCount();
        _sharedService.AddMessage("ReceiverStreamChannel");
        _sharedService.AddId(_lifeTimeService.Id);
        
        while (await reader.WaitToReadAsync(cancellationToken))
        {
            while (reader.TryRead(out var item))
            {
                _sharedService.AddHandlerData(item);
            }
        }

        return default;
    }
}

public class ReceiverConsumeStream : IReceiverConsumeStreamAsync<ReceiverMessage, ReceiverResponse>
{
    private readonly SharedService _sharedService;
    private readonly LifeTimeService _lifeTimeService;

    public ReceiverConsumeStream(SharedService sharedService, LifeTimeService lifeTimeService)
    {
        _sharedService = sharedService;
        _lifeTimeService = lifeTimeService;
    }

    public IAsyncEnumerable<ReceiverResponse> ReceiveAsync(ReceiverMessage message,
        CancellationToken cancellationToken)
    {
        _sharedService.IncrementCount();
        _sharedService.AddMessage("ReceiverConsumeStream");
        _sharedService.AddId(_lifeTimeService.Id);
        _sharedService.AddHandlerData(message);

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

public class ReceiverConsumeChannel : IReceiverConsumeStreamChannel<ReceiverMessage, ReceiverResponse>
{
    private readonly SharedService _sharedService;
    private readonly LifeTimeService _lifeTimeService;

    public ReceiverConsumeChannel(SharedService sharedService, LifeTimeService lifeTimeService)
    {
        _sharedService = sharedService;
        _lifeTimeService = lifeTimeService;
    }
    
    public Task<ChannelReader<ReceiverResponse>> ReceiveAsync(ReceiverMessage message, CancellationToken cancellationToken)
    {
        _sharedService.IncrementCount();
        _sharedService.AddMessage("ReceiverConsumeChannel");
        _sharedService.AddId(_lifeTimeService.Id);
        _sharedService.AddHandlerData(message);

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