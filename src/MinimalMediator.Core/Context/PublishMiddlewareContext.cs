using MinimalMediator.Abstractions.Context;

namespace MinimalMediator.Core.Context;

public class PublishMiddlewareContext<TMessage> : IPipeContext<TMessage>
    where TMessage : class
{
    public PublishMiddlewareContext(TMessage? message)
    {
        Id = Guid.NewGuid();
        Message = message;
    }
    
    public Guid Id { get; }
    public TMessage? Message { get; }
}