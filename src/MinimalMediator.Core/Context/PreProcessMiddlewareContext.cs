using MinimalMediator.Abstractions.Context;

namespace MinimalMediator.Core.Context;

public class PreProcessMiddlewareContext<TMessage> : IPipeContext<TMessage>
    where TMessage : class
{
    public PreProcessMiddlewareContext(TMessage? message)
    {
        Id = Guid.NewGuid();
        Message = message;
    }
    
    public Guid Id { get; }
    public TMessage? Message { get; }
}