using System.Threading.Channels;

namespace MinimalMediator.Abstractions;

public interface IMediator
{
    Task<TResponse?> SendStreamAsync<TMessage, TResponse>(IAsyncEnumerable<TMessage> message, CancellationToken cancellationToken)
        where TMessage : class
        where TResponse : class;
    
    Task<TResponse?> SendStreamAsync<TMessage, TResponse>(ChannelReader<TMessage> message, CancellationToken cancellationToken)
        where TMessage : class
        where TResponse : class;
    
    Task<TResponse?> SendAsync<TMessage, TResponse>(TMessage message, CancellationToken cancellationToken)
        where TMessage : class
        where TResponse : class;

    Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken)
        where TMessage : class;
}