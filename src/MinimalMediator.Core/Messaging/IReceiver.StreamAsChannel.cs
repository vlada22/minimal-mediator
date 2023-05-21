using System.Threading.Channels;

namespace MinimalMediator.Core.Messaging;

/// <summary>
/// Contract for a receiver that receives a stream of messages.
/// </summary>
/// <typeparam name="TMessage"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IReceiverStream<TMessage, TResponse>
    where TMessage : class
    where TResponse : class
{
    /// <summary>
    /// Handles the <see cref="Channel{T}"/> stream.
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResponse?> ReceiveAsync(ChannelReader<TMessage> reader, CancellationToken cancellationToken);
    
    /// <summary>
    /// Handles stream of <see cref="Channel{T}"/> messages.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ChannelReader<TResponse>> ReceiveAsync(TMessage message, CancellationToken cancellationToken);
}