using System.Threading.Channels;

namespace MinimalMediator.Abstractions.Pipeline;

/// <summary>
/// State machine for sending messages.
/// </summary>
/// <typeparam name="TMessage"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface ISendStateMachine<TMessage, TResponse>
    where TMessage : class
    where TResponse : class
{
    /// <summary>
    /// Process <see cref="IAsyncEnumerable{T}"/> stream asynchronously.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    IAsyncEnumerable<TResponse> ProcessStreamAsync(TMessage message, CancellationToken cancellationToken);

    /// <summary>
    /// Process <see cref="ChannelReader{T}"/> stream asynchronously.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ChannelReader<TResponse>> ProcessChannelStreamAsync(TMessage message, CancellationToken cancellationToken);

    /// <summary>
    /// Process a send state machine asynchronously.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResponse?> ProcessAsync(TMessage message, CancellationToken cancellationToken);
    
    /// <summary>
    /// Process a send state machine asynchronously.
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResponse?> ProcessAsync(ChannelReader<TMessage> reader, CancellationToken cancellationToken);
    
    /// <summary>
    /// Process a send state machine asynchronously.
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResponse?> ProcessAsync(IAsyncEnumerable<TMessage> reader, CancellationToken cancellationToken);
}