namespace MinimalMediator.Core.Messaging;

/// <summary>
/// Contract for a receiver that receives a stream of messages.
/// </summary>
/// <typeparam name="TMessage"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IReceiverStreamAsync<in TMessage, TResponse>
    where TMessage : class
    where TResponse : class
{
    /// <summary>
    /// Handles the <see cref="IAsyncEnumerable{T}"/> stream.
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResponse?> ReceiveAsync(IAsyncEnumerable<TMessage> stream, CancellationToken cancellationToken);
    
    /// <summary>
    /// Handles stream of <see cref="IAsyncEnumerable{T}"/> messages.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IAsyncEnumerable<TResponse>> ReceiveAsync(TMessage message, CancellationToken cancellationToken);
}