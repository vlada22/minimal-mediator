using MinimalMediator.Abstractions.Pipeline;
using MinimalMediator.Core.Context;
using MinimalMediator.Core.Messaging;

namespace MinimalMediator.Core.Middleware;

internal class PublishMiddlewareDefault<TMessage> : IPublishMiddleware<TMessage>
    where TMessage : class
{
    private readonly IEnumerable<IConsumer<TMessage>> _consumers;

    public PublishMiddlewareDefault(IEnumerable<IConsumer<TMessage>> consumers)
    {
        _consumers = consumers;
    }

    public Task InvokeAsync(PublishMiddlewareContext<TMessage> context, IPipe<PublishMiddlewareContext<TMessage>, TMessage> next, CancellationToken cancellationToken)
    {
        return Task.WhenAll(_consumers.Select(c => c.HandleAsync(context, cancellationToken)));
    }
}