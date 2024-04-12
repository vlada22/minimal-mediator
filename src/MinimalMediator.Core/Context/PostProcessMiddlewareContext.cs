using MinimalMediator.Abstractions.Context;

namespace MinimalMediator.Core.Context;

public sealed class PostProcessMiddlewareContext<TMessage>(TMessage? message) : IPipeContext<TMessage>
    where TMessage : class
{
    public Guid Id { get; } = Guid.NewGuid();
    public TMessage? Message { get; } = message;
}