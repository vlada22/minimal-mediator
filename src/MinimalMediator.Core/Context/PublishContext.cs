using MinimalMediator.Abstractions.Context;

namespace MinimalMediator.Core.Context;

public class PublishContext : IPipeContext
{
    public Guid Id { get; } = Guid.NewGuid();
}