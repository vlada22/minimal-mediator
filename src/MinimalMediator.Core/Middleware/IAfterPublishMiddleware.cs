using MinimalMediator.Abstractions.Middleware;
using MinimalMediator.Core.Context;

namespace MinimalMediator.Core.Middleware;

public interface IAfterPublishMiddleware<TMessage> : IMiddleware<PostProcessMiddlewareContext<TMessage>, TMessage>
    where TMessage : class
{
}