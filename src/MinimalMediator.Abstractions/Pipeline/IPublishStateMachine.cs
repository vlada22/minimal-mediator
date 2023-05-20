namespace MinimalMediator.Abstractions.Pipeline;

public interface IPublishStateMachine<in TMessage>
    where TMessage : class
{
    Task ProcessAsync(TMessage message, CancellationToken cancellationToken);
}