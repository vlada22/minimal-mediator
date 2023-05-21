using MinimalMediator.Abstractions.Context;
using MinimalMediator.Core.Context;

namespace MinimalMediator.Core.Messaging;

/// <summary>
/// Contract for a consumer. Multiple consumers can be registered for a message.
/// </summary>
/// <typeparam name="TMessage"></typeparam>
public interface IConsumer<TMessage>
    where TMessage : class
{
    /// <summary>
    /// Handles the message.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task HandleAsync(PublishMiddlewareContext<TMessage> context, CancellationToken cancellationToken);
}