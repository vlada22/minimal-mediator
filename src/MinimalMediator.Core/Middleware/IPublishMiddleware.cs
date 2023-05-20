using MinimalMediator.Abstractions.Middleware;
using MinimalMediator.Core.Context;

namespace MinimalMediator.Core.Middleware;

public interface IPublishMiddleware<TMessage> : IMiddleware<PublishMiddlewareContext<TMessage>, TMessage>
    where TMessage : class
{
    
}