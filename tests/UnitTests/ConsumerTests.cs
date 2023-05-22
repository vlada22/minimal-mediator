using Microsoft.Extensions.DependencyInjection;
using MinimalMediator.Abstractions;

namespace UnitTests;

public class ConsumerTests
{
    [Fact]
    public async Task ShouldInvokeConsumerTransient()
    {
        var services = new ServiceCollection();
        services.AddMinimalMediator(c=>c.UseReflection(typeof(ConsumerTest)));
        services.AddSingleton<SharedService>();
        services.AddTransient<LifeTimeService>();
        var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();
        var sharedService = provider.GetRequiredService<SharedService>();

        await mediator.PublishAsync(new ConsumerMessage("Transient"), CancellationToken.None);
        
        Assert.Equal(4, sharedService.Count);
        // transient service is created for each middleware
        Assert.Equal(4, sharedService.Ids.Distinct().Count());
        // check execution order
        Assert.True(sharedService.Messages.SequenceEqual(GetExpectedMiddlewareOrder()));
    }
    
    [Fact]
    public async Task ShouldInvokeConsumerScoped()
    {
        var services = new ServiceCollection();
        services.AddMinimalMediator(c=>c.UseReflection(typeof(ConsumerTest)), ServiceLifetime.Scoped);
        services.AddSingleton<SharedService>();
        services.AddScoped<LifeTimeService>();
        var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();
        var sharedService = provider.GetRequiredService<SharedService>();

        await mediator.PublishAsync(new ConsumerMessage("Scoped"), CancellationToken.None);
        
        Assert.Equal(4, sharedService.Count);
        // scoped service, same instance for each middleware
        Assert.Single(sharedService.Ids.Distinct());
        // check execution order
        Assert.True(sharedService.Messages.SequenceEqual(GetExpectedMiddlewareOrder()));
    }

    private static IReadOnlyList<string> GetExpectedMiddlewareOrder()
    {
        return new List<string>
        {
            "ConsumerBeforeMiddleware2",
            "ConsumerBeforeMiddleware1",
            "ConsumerTest",
            "ConsumerAfterMiddleware"
        };
    }
}