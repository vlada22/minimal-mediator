using System.Threading.Channels;

namespace MinimalMediator.Core.Messaging;

public interface IReceiverStream<TMessage, TResponse>
    where TMessage : class
    where TResponse : class
{
    Task<TResponse?> ReceiveAsync(ChannelReader<TMessage> reader, CancellationToken cancellationToken);
}