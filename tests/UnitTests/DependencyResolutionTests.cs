using Microsoft.Extensions.DependencyInjection;
using MinimalMediator.Abstractions;
using MinimalMediator.Abstractions.Pipeline;
using MinimalMediator.Core.Container;
using MinimalMediator.Core.Messaging;
using MinimalMediator.Core.Middleware;

namespace UnitTests;

public class DependencyResolutionTests
{
    [Fact]
    public void ShouldResolveCoreDependencies()
    {
        var services = new ServiceCollection();
        services.AddSingleton<SharedService>();
        services.AddTransient<LifeTimeService>();
        services.AddMinimalMediator(config => config.UseReflection());
        var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();
        var diContext = provider.GetRequiredService<IMediatorDependencyContext>();
        var pipeBuilder = provider.GetRequiredService<IPipeBuilder>();
        var publishMiddleware = provider.GetRequiredService<IPublishMiddleware<TestMessage>>();
        var publishStateMachine = provider.GetRequiredService<IPublishStateMachine<TestMessage>>();
        var sendStateMachine = provider.GetRequiredService<ISendStateMachine<TestMessage, TestMessage>>();

        Assert.NotNull(mediator);
        Assert.NotNull(diContext);
        Assert.NotNull(pipeBuilder);
        Assert.NotNull(publishMiddleware);
        Assert.NotNull(publishStateMachine);
        Assert.NotNull(sendStateMachine);
    }

    [Fact]
    public void ShouldResolveCoreScopedDependencies()
    {
        var services = new ServiceCollection();
        services.AddSingleton<SharedService>();
        services.AddTransient<LifeTimeService>();
        services.AddMinimalMediator(config => config.UseReflection(), ServiceLifetime.Scoped);
        var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();
        var diContext = provider.GetRequiredService<IMediatorDependencyContext>();
        var pipeBuilder = provider.GetRequiredService<IPipeBuilder>();
        var publishMiddleware = provider.GetRequiredService<IPublishMiddleware<TestMessage>>();
        var publishStateMachine = provider.GetRequiredService<IPublishStateMachine<TestMessage>>();
        var sendStateMachine = provider.GetRequiredService<ISendStateMachine<TestMessage, TestMessage>>();

        Assert.NotNull(mediator);
        Assert.NotNull(diContext);
        Assert.NotNull(pipeBuilder);
        Assert.NotNull(publishMiddleware);
        Assert.NotNull(publishStateMachine);
        Assert.NotNull(sendStateMachine);
    }

    [Fact]
    public void ShouldResolveMiddlewareDependencies()
    {
        var services = new ServiceCollection();
        services.AddSingleton<SharedService>();
        services.AddTransient<LifeTimeService>();
        services.AddMinimalMediator(
            config => config.UseReflection(typeof(DependencyResolutionTests)));
        var provider = services.BuildServiceProvider();

        var afterMiddleware = provider.GetRequiredService<IEnumerable<IAfterPublishMiddleware<TestMessage>>>();
        var beforeMiddleware = provider.GetRequiredService<IEnumerable<IBeforePublishMiddleware<TestMessage>>>();
        var exceptionMiddleware = provider.GetRequiredService<IEnumerable<IExceptionHandlerMiddleware<TestMessage>>>();
        var consumer = provider.GetRequiredService<IEnumerable<IConsumer<TestMessage>>>();
        var receiver = provider.GetRequiredService<IEnumerable<IReceiver<TestMessage, TestResponse>>>();
        var receiver2 = provider.GetRequiredService<IEnumerable<IReceiverStreamAsync<TestMessage, TestResponse>>>();
        var receiver3 = provider.GetRequiredService<IEnumerable<IReceiverStreamChannel<TestMessage, TestResponse>>>();
        var receiver4 = provider.GetRequiredService<IEnumerable<IReceiverConsumeStreamAsync<TestMessage, TestResponse>>>();
        var receiver5 = provider.GetRequiredService<IEnumerable<IReceiverConsumeStreamChannel<TestMessage, TestResponse>>>();

        Assert.NotEmpty(afterMiddleware);
        Assert.NotEmpty(beforeMiddleware);
        Assert.NotEmpty(exceptionMiddleware);
        Assert.NotEmpty(consumer);
        Assert.NotEmpty(receiver);
        Assert.NotEmpty(receiver2);
        Assert.NotEmpty(receiver3);
        Assert.NotEmpty(receiver4);
        Assert.NotEmpty(receiver5);
    }
}