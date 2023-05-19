using MinimalMediator.Abstractions.Context;

namespace MinimalMediator.Core.Context;

public class PublishContext<T> : IPipeContext
    where T : class
{
    public Guid Id { get; } = Guid.NewGuid();
    public T? Message { get; set; }
}