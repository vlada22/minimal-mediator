using MinimalMediator.Abstractions.Middleware;
using MinimalMediator.Core.Context;

namespace MinimalMediator.Core.Middleware;

/// <summary>
/// Middleware that runs before a message is published.
/// </summary>
/// <typeparam name="TMessage"></typeparam>
public interface IBeforePublishMiddleware<TMessage> : IMiddleware<PreProcessMiddlewareContext<TMessage>, TMessage>
    where TMessage : class
{
}