using Microsoft.Extensions.DependencyInjection;
using MinimalMediator.Abstractions.Context;
using MinimalMediator.Abstractions.Middleware;
using MinimalMediator.Abstractions.Pipeline;
using MinimalMediator.Core.DependencyInjection;

namespace MinimalMediator.Core.Middleware;

public class PipeMiddlewareBuilder<TContext> : IPipeMiddlewareBuilder<TContext>
    where TContext : class, IPipeContext
{
    private readonly IMediatorDependencyContext _dependencyContext;

    public PipeMiddlewareBuilder(IMediatorDependencyContext dependencyContext)
    {
        _dependencyContext = dependencyContext;
    }

    public IPipe<TContext> Build<TMiddleware>()
        where TMiddleware : class, IMediatorMiddleware<TContext>
    {
        var middlewares = _dependencyContext.ActivationServices.GetServices<IMediatorMiddleware<TContext>>()
            .OfType<TMiddleware>().ToList();

        IPipe<TContext> next = FinalPipe<TContext>.Empty;

        for (var i = middlewares.Count - 1; i >= 0; i--)
        {
            next = new PipeMiddleware<TContext>(middlewares[i], next);
        }

        return next;
    }
}

public class PipeMiddlewareBuilder<TContext, TResult> : IPipeMiddlewareBuilder<TContext, TResult>
    where TContext : class, IPipeContext
    where TResult : class
{
    private readonly IMediatorDependencyContext _dependencyContext;

    public PipeMiddlewareBuilder(IMediatorDependencyContext dependencyContext)
    {
        _dependencyContext = dependencyContext;
    }

    public IPipe<TContext, TResult> Build<TMiddleware>()
        where TMiddleware : class, IMediatorMiddleware<TContext, TResult>
    {
        var middlewares = _dependencyContext.ActivationServices.GetServices<IMediatorMiddleware<TContext, TResult>>()
            .OfType<TMiddleware>().ToList();

        IPipe<TContext, TResult> next = FinalPipe<TContext, TResult>.Empty;

        for (var i = middlewares.Count - 1; i >= 0; i--)
        {
            next = new PipeMiddleware<TContext, TResult>(middlewares[i], next);
        }

        return next;
    }
}