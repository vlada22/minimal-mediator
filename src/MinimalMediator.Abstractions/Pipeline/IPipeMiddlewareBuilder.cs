using MinimalMediator.Abstractions.Context;
using MinimalMediator.Abstractions.Middleware;

namespace MinimalMediator.Abstractions.Pipeline;

public interface IPipeMiddlewareBuilder<TContext> 
    where TContext : class, IPipeContext
{
    IPipe<TContext> Build<TMiddleware>() 
        where TMiddleware : class, IMediatorMiddleware<TContext>;
}

public interface IPipeMiddlewareBuilder<TContext, TResult>
    where TContext : class, IPipeContext
    where TResult : class
{
    IPipe<TContext, TResult> Build<TMiddleware>()
        where TMiddleware : class, IMediatorMiddleware<TContext, TResult>;
}