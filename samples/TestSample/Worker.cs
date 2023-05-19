using MinimalMediator.Core;

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
        
        await mediator.Test(new TestContext("Test"), stoppingToken);
        
        await mediator.Test(new TestContext("Test1"), stoppingToken);
        
        await mediator.Test(new TestContext("Test2"), stoppingToken);
    }
}