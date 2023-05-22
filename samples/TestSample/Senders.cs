using System.Threading.Channels;
using MinimalMediator.Core.Messaging;

namespace TestSample;

public class Sender1 : IReceiver<TestContext, TestResponse>
{
    private readonly ILogger<Sender1> _logger;

    public Sender1(ILogger<Sender1> logger)
    {
        _logger = logger;
    }

    public Task<TestResponse?> ReceiveAsync(TestContext message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sender1: {Message}", message);
        return Task.FromResult<TestResponse?>(new TestResponse("Hello World!"));
    }
}

public class Sender2 : IReceiverStreamChannel<TestContext, TestResponse>
{
    private readonly ILogger<Sender2> _logger;

    public Sender2(ILogger<Sender2> logger)
    {
        _logger = logger;
    }

    public async Task<TestResponse?> ReceiveAsync(ChannelReader<TestContext> reader, CancellationToken cancellationToken)
    {
        while (await reader.WaitToReadAsync(cancellationToken))
        {
            var item = await reader.ReadAsync(cancellationToken);
            _logger.LogInformation("Sender2: {Message}", item);
        }
        return new TestResponse("Hello World!");
    }

    public Task<ChannelReader<TestResponse>> ReceiveAsync(TestContext message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

public class Sender3 : IReceiverStreamAsync<TestContext, TestResponse>
{
    private readonly ILogger<Sender3> _logger;

    public Sender3(ILogger<Sender3> logger)
    {
        _logger = logger;
    }

    public async Task<TestResponse?> ReceiveAsync(IAsyncEnumerable<TestContext> reader, CancellationToken cancellationToken)
    {
        await foreach (var item in reader.WithCancellation(cancellationToken))
        {
            _logger.LogInformation("Sender3: {Message}", item);
        }
        return new TestResponse("Hello World!");
    }

    public Task<IAsyncEnumerable<TestResponse>> ReceiveAsync(TestContext message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}