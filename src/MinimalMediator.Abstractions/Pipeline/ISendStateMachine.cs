using System.Threading.Channels;

namespace MinimalMediator.Abstractions.Pipeline;

public interface ISendStateMachine<TMessage, TResponse>
    where TMessage : class
    where TResponse : class
{
    Task<TResponse?> ProcessAsync(TMessage message, CancellationToken cancellationToken);
    Task<TResponse?> ProcessAsync(ChannelReader<TMessage> reader, CancellationToken cancellationToken);
    Task<TResponse?> ProcessAsync(IAsyncEnumerable<TMessage> reader, CancellationToken cancellationToken);
}