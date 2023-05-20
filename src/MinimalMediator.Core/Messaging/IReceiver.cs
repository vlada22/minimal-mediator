namespace MinimalMediator.Core.Messaging;

public interface IReceiver<in TMessage, TResult>
    where TMessage : class
    where TResult : class
{
    Task<TResult?> ReceiveAsync(TMessage message, CancellationToken cancellationToken);
}