using MinimalMediator.Abstractions.Context;
using MinimalMediator.Abstractions.Pipeline;

namespace MinimalMediator.Abstractions.Middleware;

/// <summary>
/// Contract for a middleware.
/// </summary>
/// <typeparam name="TContext"></typeparam>
/// <typeparam name="TMessage"></typeparam>
public interface IMiddleware<TContext, TMessage>
    where TContext : class, IPipeContext<TMessage>
    where TMessage : class
{
    /// <summary>
    /// Handles the middleware.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task InvokeAsync(TContext context, IPipe<TContext, TMessage> next, CancellationToken cancellationToken);
}