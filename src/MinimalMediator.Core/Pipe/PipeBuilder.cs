using Microsoft.Extensions.DependencyInjection;
using MinimalMediator.Abstractions.Context;
using MinimalMediator.Abstractions.Middleware;
using MinimalMediator.Abstractions.Pipeline;
using MinimalMediator.Core.Container;

namespace MinimalMediator.Core.Pipe;

internal class PipeBuilder(IMediatorDependencyContext dependencyContext) : IPipeBuilder
{
    public IPipe<TContext, TMessage> Build<TMiddleware, TContext, TMessage>()
        where TMiddleware : class, IMiddleware<TContext, TMessage>
        where TContext : class, IPipeContext<TMessage>
        where TMessage : class
    {
        var middlewares = dependencyContext.ActivationServices.GetServices<TMiddleware>()
            .ToList();

        IPipe<TContext, TMessage> next = FirstPipe<TContext, TMessage>.Empty;

        for (var i = middlewares.Count - 1; i >= 0; i--)
        {
            next = new PipeMiddleware<TContext, TMessage>(middlewares[i], next);
        }

        return next;
    }
}