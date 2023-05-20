using Microsoft.Extensions.DependencyInjection;
using MinimalMediator.Abstractions.Context;
using MinimalMediator.Abstractions.Middleware;
using MinimalMediator.Abstractions.Pipeline;
using MinimalMediator.Core.DependencyInjection;

namespace MinimalMediator.Core.Pipe;

public class PipeBuilder : IPipeBuilder
{
    private readonly IMediatorDependencyContext _dependencyContext;

    public PipeBuilder(IMediatorDependencyContext dependencyContext)
    {
        _dependencyContext = dependencyContext;
    }

    public IPipe<TContext, TMessage> Build<TMiddleware, TContext, TMessage>()
        where TMiddleware : class, IMiddleware<TContext, TMessage>
        where TContext : class, IPipeContext<TMessage>
        where TMessage : class
    {
        var middlewares = _dependencyContext.ActivationServices.GetServices<TMiddleware>()
            .ToList();

        IPipe<TContext, TMessage> next = FirstPipe<TContext, TMessage>.Empty;

        for (var i = middlewares.Count - 1; i >= 0; i--)
        {
            next = new PipeMiddleware<TContext, TMessage>(middlewares[i], next);
        }

        return next;
    }
}