using MinimalMediator.Abstractions.Context;

namespace MinimalMediator.Core.Context;

public sealed class ExceptionHandlerMiddlewareContext<TMessage>(TMessage? message, Exception? exception)
    : IPipeContext<TMessage>
    where TMessage : class
{
    public Guid Id { get; } = Guid.NewGuid();
    public TMessage? Message { get; } = message;
    public Exception? Exception { get; set; } = exception;
}