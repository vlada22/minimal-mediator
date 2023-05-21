using MinimalMediator.Abstractions.Middleware;
using MinimalMediator.Core.Context;

namespace MinimalMediator.Core.Middleware;

/// <summary>
/// Middleware that publishes a message.
/// </summary>
/// <typeparam name="TMessage"></typeparam>
public interface IPublishMiddleware<TMessage> : IMiddleware<PublishMiddlewareContext<TMessage>, TMessage>
    where TMessage : class
{
    
}