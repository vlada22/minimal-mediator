using MinimalMediator.Abstractions.Middleware;
using MinimalMediator.Core.Context;

namespace MinimalMediator.Core.Publish;

public interface IPublishMiddleware<TMessage> : IMediatorMiddleware<PublishContext<TMessage>>
    where TMessage : class
{
}