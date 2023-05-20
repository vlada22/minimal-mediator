namespace MinimalMediator.Core.Messaging;

public interface IReceiverStreamAsync<in TMessage, TResponse>
    where TMessage : class
    where TResponse : class
{
    Task<TResponse?> ReceiveAsync(IAsyncEnumerable<TMessage> stream, CancellationToken cancellationToken);
}