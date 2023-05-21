using System.Threading.Channels;

namespace MinimalMediator.Abstractions;

/// <summary>
/// Mediator contract.
/// </summary>
public interface IMediator
{
    /// <summary>
    /// Sends a stream of messages to a consumer.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    Task<TResponse?> SendStreamAsync<TMessage, TResponse>(IAsyncEnumerable<TMessage> message, CancellationToken cancellationToken)
        where TMessage : class
        where TResponse : class;
    
    /// <summary>
    /// Sends a stream of messages to a consumer.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    Task<TResponse?> SendStreamAsync<TMessage, TResponse>(ChannelReader<TMessage> message, CancellationToken cancellationToken)
        where TMessage : class
        where TResponse : class;
    
    /// <summary>
    /// Sends a message to a consumer.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    Task<TResponse?> SendAsync<TMessage, TResponse>(TMessage message, CancellationToken cancellationToken)
        where TMessage : class
        where TResponse : class;

    /// <summary>
    /// Publishes a message to consumers.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TMessage"></typeparam>
    /// <returns></returns>
    Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken)
        where TMessage : class;
}