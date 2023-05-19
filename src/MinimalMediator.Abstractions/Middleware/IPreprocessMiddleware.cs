using MinimalMediator.Abstractions.Context;

namespace MinimalMediator.Abstractions.Middleware;

public interface IPreprocessMiddleware<TContext> : IMediatorMiddleware<TContext>
    where TContext : class, IPipeContext
{
}

public interface IPreprocessMiddleware<TContext, TResult> : IMediatorMiddleware<TContext, TResult>
    where TContext : class, IPipeContext
    where TResult : class
{
}