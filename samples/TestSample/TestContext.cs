using MinimalMediator.Abstractions.Context;

namespace TestSample;

public record TestContext(string Name) : IPipeContext
{
    public Guid Id { get; } = Guid.NewGuid();
}