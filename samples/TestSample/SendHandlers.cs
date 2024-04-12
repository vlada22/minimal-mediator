using System.Threading.Channels;
using MinimalMediator.Core.Messaging;

namespace TestSample;

public class ReceiverTest(TransientService transientService, ILogger<ReceiverTest> logger)
    : IReceiver<TestMessage, TestResponse>
{
    public Task<TestResponse?> ReceiveAsync(TestMessage message, CancellationToken cancellationToken)
    {
        logger.LogInformation("TransientService: {TransientServiceId}", transientService.Id);
        
        return Task.FromResult<TestResponse?>(new TestResponse(message.Value));
    }
}

public class ReceiverStream(ILogger<ReceiverStream> logger) : IReceiverStreamAsync<TestMessage, TestResponse>
{
    public async Task<TestResponse?> ReceiveAsync(IAsyncEnumerable<TestMessage> stream, CancellationToken cancellationToken)
    {
        var i = 0;
        await foreach (var item in stream.WithCancellation(cancellationToken))
        {
            logger.LogInformation("ReceiverStream message: {Message}", item);
            i++;
        }

        return new TestResponse($"Received total of {i} messages.");
    }
}

public class ReceiverStreamChannel(ILogger<ReceiverStreamChannel> logger)
    : IReceiverStreamChannel<TestMessage, TestResponse>
{
    public async Task<TestResponse?> ReceiveAsync(ChannelReader<TestMessage> reader, CancellationToken cancellationToken)
    {
        var i = 0;
        while (await reader.WaitToReadAsync(cancellationToken))
        {
            while (reader.TryRead(out var item))
            {
                logger.LogInformation("ReceiverStreamChannel message: {Message}", item);
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