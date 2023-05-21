namespace MinimalMediator.Abstractions.Pipeline;

/// <summary>
/// State machine for publishing a message.
/// </summary>
/// <typeparam name="TMessage"></typeparam>
public interface IPublishStateMachine<in TMessage>
    where TMessage : class
{
    /// <summary>
    /// Process a publish state machine asynchronously.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ProcessAsync(TMessage message, CancellationToken cancellationToken);
}