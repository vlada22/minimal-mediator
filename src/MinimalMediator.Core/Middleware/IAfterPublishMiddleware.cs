using MinimalMediator.Abstractions.Middleware;
using MinimalMediator.Core.Context;

namespace MinimalMediator.Core.Middleware;

/// <summary>
/// Middleware that runs after a message is published.
/// </summary>
/// <typeparam name="TMessage"></typeparam>
public interface IAfterPublishMiddleware<TMessage> : IMiddleware<PostProcessMiddlewareContext<TMessage>, TMessage>
    where TMessage : class
{
}