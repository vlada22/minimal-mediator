using MinimalMediator.Abstractions.Context;

namespace MinimalMediator.Abstractions.Middleware;

public interface IPostProcessMiddleware<TContext> : IMediatorMiddleware<TContext>
    where TContext : class, IPipeContext
{
}

public interface IPostProcessMiddleware<TContext, TResult> : IMediatorMiddleware<TContext, TResult>
    where TContext : class, IPipeContext
    where TResult : class
{
}