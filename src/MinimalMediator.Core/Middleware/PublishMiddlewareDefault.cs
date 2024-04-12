using MinimalMediator.Abstractions.Pipeline;
using MinimalMediator.Core.Context;
using MinimalMediator.Core.Messaging;

namespace MinimalMediator.Core.Middleware;

internal class PublishMiddlewareDefault<TMessage>(IEnumerable<IConsumer<TMessage>> consumers)
    : IPublishMiddleware<TMessage>
    where TMessage : class
{
    public Task InvokeAsync(PublishMiddlewareContext<TMessage> context, IPipe<PublishMiddlewareContext<TMessage>, TMessage> next, CancellationToken cancellationToken)
    {
        return Task.WhenAll(consumers.Select(c => c.HandleAsync(context, cancellationToken)));
    }
}