using Microsoft.Extensions.DependencyInjection;
using MinimalMediator.Abstractions.Context;
using MinimalMediator.Abstractions.Middleware;
using MinimalMediator.Abstractions.Pipeline;
using MinimalMediator.Core.DependencyInjection;

namespace MinimalMediator.Core;

public class MinimalMediator : IMediator
{
    private readonly IMediatorDependencyContext _dependencyContext;

    public MinimalMediator(IMediatorDependencyContext dependencyContext)
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

    public ValueTask DisposeAsync()
    {
        return new ValueTask();
    }
}