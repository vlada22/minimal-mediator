using System.Threading.Channels;
using MinimalMediator.Core.Messaging;

namespace TestSampleAot;

public class ReceiverTest : IReceiver<TestMessage, TestResponse>
{
    private readonly TransientService _transientService;
    private readonly ILogger<ReceiverTest> _logger;

    public ReceiverTest(TransientService transientService, ILogger<ReceiverTest> logger)
    {
        _transientService = transientService;
        _logger = logger;
    }

    public Task<TestResponse?> ReceiveAsync(TestMessage message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("TransientService: {TransientServiceId}", _transientService.Id);
        
        return Task.FromResult<TestResponse?>(new TestResponse(message.Value));
    }
}

public class ReceiverStream : IReceiverStreamAsync<TestMessage, TestResponse>
{
    private readonly ILogger<ReceiverStream> _logger;

    public ReceiverStream(ILogger<ReceiverStream> logger)
    {
        _logger = logger;
    }

    public async Task<TestResponse?> ReceiveAsync(IAsyncEnumerable<TestMessage> stream, CancellationToken cancellationToken)
    {
        var i = 0;
        await foreach (var item in stream.WithCancellation(cancellationToken))
        {
            _logger.LogInformation("ReceiverStream message: {Message}", item);
            i++;
        }

        return new TestResponse($"Received total of {i} messages.");
    }
}

public class ReceiverStreamChannel : IReceiverStreamChannel<TestMessage, TestResponse>
{
    private readonly ILogger<ReceiverStreamChannel> _logger;

    public ReceiverStreamChannel(ILogger<ReceiverStreamChannel> logger)
    {
        _logger = logger;
    }

    public async Task<TestResponse?> ReceiveAsync(ChannelReader<TestMessage> reader, CancellationToken cancellationToken)
    {
        var i = 0;
        while (await reader.WaitToReadAsync(cancellationToken))
        {
            while (reader.TryRead(out var item))
            {
                _logger.LogInformation("ReceiverStreamChannel message: {Message}", item);
                i++;
            }
        }

        return new TestResponse($"Received total of {i} messages.");
    }
}

public class ReceiverConsumeStream : IReceiverConsumeStreamAsync<TestMessage, TestResponse>
{
    public IAsyncEnumerable<TestResponse> ReceiveAsync(TestMessage message,
        CancellationToken cancellationToken)
    {
        async IAsyncEnumerable<TestResponse> Stream()
        {
            for (var i = 0; i < 5; i++)
            {
                yield return new TestResponse(i.ToString());
                await Task.Delay(1, cancellationToken);
            }
        }
        
        return Stream();
    }
}

public class ReceiverConsumeChannel : IReceiverConsumeStreamChannel<TestMessage, TestResponse>
{
    public Task<ChannelReader<TestResponse>> ReceiveAsync(TestMessage message, CancellationToken cancellationToken)
    {
        var channel = Channel.CreateUnbounded<TestResponse>();

        _ = Task.Run(async () =>
        {
            for (var i = 0; i < 5; i++)
            {
                await channel.Writer.WriteAsync(new TestResponse(i.ToString()), cancellationToken);
                await Task.Delay(100, cancellationToken);
            }

            channel.Writer.Complete();
        }, cancellationToken);

        return Task.FromResult(channel.Reader);
    }
}