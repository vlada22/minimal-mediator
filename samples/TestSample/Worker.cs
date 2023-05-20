using System.Threading.Channels;
using MinimalMediator.Abstractions;

namespace TestSample;

public class Worker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<Worker> _logger;

    public Worker(IServiceProvider serviceProvider, ILogger<Worker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        //await mediator.PublishAsync(new TestContext("Test"), stoppingToken);

        //var data = await mediator.SendAsync<TestContext, TestResponse>(new TestContext("test"), stoppingToken);
        //_logger.LogInformation("Data: {Data}", data);

        // var channel = Channel.CreateUnbounded<TestContext>();
        // _ = Task.Factory.StartNew(async () =>
        // {
        //     for (int i = 0; i < 5; i++)
        //     {
        //         await channel.Writer.WriteAsync(new TestContext($"test {i}"), stoppingToken);
        //     }
        // }, stoppingToken);

        //var data2 = await mediator.SendStreamAsync<TestContext, TestResponse>(channel.Reader, stoppingToken);
        //_logger.LogInformation("Data: {Data}", data2);
        
        async IAsyncEnumerable<TestContext> StreamData()
        {
            for (var i = 0; i < 5; i++)
            {
                var data = new TestContext($"async stream {i}");
                await Task.Delay(1, stoppingToken);
                yield return data;
            }
        }
        
        var data3 = await mediator.SendStreamAsync<TestContext, TestResponse>(StreamData(), stoppingToken);
        _logger.LogInformation("Data: {Data}", data3);
    }
}