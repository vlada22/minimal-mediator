using MinimalMediator.Abstractions.Middleware;
using MinimalMediator.Core.Context;

namespace MinimalMediator.Core.Middleware;

public interface IBeforePublishMiddleware<TMessage> : IMiddleware<PreProcessMiddlewareContext<TMessage>, TMessage>
    where TMessage : class
{
}