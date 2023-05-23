namespace MinimalMediator.Core.Messaging;

public interface IReceiverConsumeStreamAsync<in TMessage, out TResult>
    where TMessage : class
    where TResult : class
{
    /// <summary>
    /// Handles stream of <see cref="IAsyncEnumerable{T}"/> messages.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    IAsyncEnumerable<TResult> ReceiveAsync(TMessage message, CancellationToken cancellationToken);
}