using MinimalMediator.Abstractions.Context;

namespace MinimalMediator.Core.Context;

public class PostProcessMiddlewareContext<TMessage> : IPipeContext<TMessage>
    where TMessage : class
{
    public PostProcessMiddlewareContext(TMessage? message)
    {
        Id = Guid.NewGuid();
        Message = message;
    }

    public Guid Id { get; }
    public TMessage? Message { get; }
}