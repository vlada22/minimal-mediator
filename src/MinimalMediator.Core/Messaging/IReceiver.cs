namespace MinimalMediator.Core.Messaging;

/// <summary>
/// Contract for a receiver. Only one receiver can be registered for a message.
/// </summary>
/// <typeparam name="TMessage"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface IReceiver<in TMessage, TResult>
    where TMessage : class
    where TResult : class
{
    /// <summary>
    /// Handles the message.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResult?> ReceiveAsync(TMessage message, CancellationToken cancellationToken);
}