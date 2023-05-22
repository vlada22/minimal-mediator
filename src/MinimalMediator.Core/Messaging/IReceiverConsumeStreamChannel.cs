using System.Threading.Channels;

namespace MinimalMediator.Core.Messaging;

public interface IReceiverConsumeStreamChannel<in TMessage, TResult>
    where TMessage : class
    where TResult : class
{
    /// <summary>
    /// Handles stream of <see cref="Channel{T}"/> messages.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ChannelReader<TResult>> ReceiveAsync(TMessage message, CancellationToken cancellationToken);
}