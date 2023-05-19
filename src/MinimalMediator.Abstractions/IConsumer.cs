using MinimalMediator.Abstractions.Context;

namespace MinimalMediator.Abstractions;

public interface IConsumer<in TContext>
    where TContext : class, IPipeContext
{
    Task ConsumeAsync(TContext context, CancellationToken cancellationToken);
}