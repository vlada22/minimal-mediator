using MinimalMediator.Abstractions.Context;
using MinimalMediator.Abstractions.Middleware;

namespace MinimalMediator.Abstractions.Pipeline;

/// <summary>
/// Builds a pipe.
/// </summary>
public interface IPipeBuilder
{
    /// <summary>
    /// Builds a pipe with the specified middleware.
    /// </summary>
    /// <typeparam name="TMiddleware"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TMessage"></typeparam>
    /// <returns></returns>
    public IPipe<TContext, TMessage> Build<TMiddleware, TContext, TMessage>()
        where TMiddleware : class, IMiddleware<TContext, TMessage>
        where TContext : class, IPipeContext<TMessage>
        where TMessage : class;
}