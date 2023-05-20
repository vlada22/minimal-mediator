namespace MinimalMediator.Abstractions.Context;

public interface IPipeContext<out TMessage>
    where TMessage : class
{
    Guid Id { get; }
    TMessage? Message { get; }
}