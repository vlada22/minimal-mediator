using MinimalMediator.Abstractions.Context;
using MinimalMediator.Abstractions.Pipeline;

namespace MinimalMediator.Abstractions.Middleware;

public interface IMediatorMiddleware<TContext> : IDisposable
    where TContext : class, IPipeContext
{
    Task InvokeAsync(TContext context, IPipe<TContext> next, CancellationToken cancellationToken);
}

public interface IMediatorMiddleware<TContext, TResult> : IDisposable
    where TContext : class, IPipeContext 
    where TResult : class
{
    Task<TResult?> InvokeAsync(TContext context, IPipe<TContext, TResult> next, CancellationToken cancellationToken);
}