using MinimalMediator.Abstractions.Context;

namespace MinimalMediator.Core.Context;

public class ExceptionHandlerMiddlewareContext<TMessage> : IPipeContext<TMessage>
    where TMessage : class
{
    public ExceptionHandlerMiddlewareContext(TMessage? message, Exception? exception)
    {
        Id = Guid.NewGuid();
        Message = message;
        Exception = exception;
    }
    
    public Guid Id { get; }
    public TMessage? Message { get; }
    public Exception? Exception { get; set; }
}