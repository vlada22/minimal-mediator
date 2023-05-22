using System.Threading.Channels;

namespace MinimalMediator.Core.Messaging;

/// <summary>
/// Contract for a receiver that receives a stream of messages.
/// </summary>
/// <typeparam name="TMessage"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IReceiverStreamChannel<TMessage, TResponse>
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
}