using MinimalMediator.Abstractions.Middleware;
using MinimalMediator.Core.Context;

namespace MinimalMediator.Core.Publish;

public interface IPublishMiddleware : IMediatorMiddleware<PublishContext>
{
}