using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using MinimalMediator.Abstractions;
using MinimalMediator.Abstractions.Context;
using MinimalMediator.Abstractions.Middleware;
using MinimalMediator.Abstractions.Pipeline;
using MinimalMediator.Core.Context;
using MinimalMediator.Core.DependencyInjection;
using MinimalMediator.Core.Store;

namespace MinimalMediator.Core;

public sealed class MediatorDefault : IMediator
{
    private readonly IMediatorDependencyContext _dependencyContext;
    private readonly ConcurrentDictionary<Type, object> _publishers = new();

    public MediatorDefault(IMediatorDependencyContext dependencyContext)
    {
        _dependencyContext = dependencyContext;
    }

    public async Task Test<TContext>(TContext context, CancellationToken cancellationToken)
        where TContext : class, IPipeContext
    {
        var pipeBuilder = _dependencyContext.ActivationServices.GetRequiredService<IPipeMiddlewareBuilder<TContext>>();
        
        var pipePre = pipeBuilder.Build<IPreprocessMiddleware<TContext>>();

        await pipePre.InvokeAsync(context, cancellationToken);
        
        var pipePost = pipeBuilder.Build<IPostProcessMiddleware<TContext>>();
        
        await pipePost.InvokeAsync(context, cancellationToken);
    }

    public Task PublishAsync<T>(T context, CancellationToken cancellationToken) where T : class
    {
        var publisher = (IPipe<PublishContext<T>>) _publishers.GetOrAdd(typeof(T), _dependencyContext.ActivationServices.GetRequiredService<IPipe<PublishContext<T>>>());

        return publisher.InvokeAsync(context, cancellationToken);
    }

    public ValueTask DisposeAsync()
    {
        return new ValueTask();
    }
}