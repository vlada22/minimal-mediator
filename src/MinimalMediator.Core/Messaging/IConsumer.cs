using MinimalMediator.Abstractions.Context;
using MinimalMediator.Core.Context;

namespace MinimalMediator.Core.Messaging;

public interface IConsumer<TMessage>
    where TMessage : class
{
    Task HandleAsync(PublishMiddlewareContext<TMessage> context, CancellationToken cancellationToken);
}