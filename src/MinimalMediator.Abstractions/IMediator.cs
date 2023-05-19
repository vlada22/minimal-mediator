using MinimalMediator.Abstractions.Context;

namespace MinimalMediator.Abstractions;

public interface IMediator
{
    Task Test<TContext>(TContext context, CancellationToken cancellationToken)
        where TContext : class, IPipeContext;

    Task PublishAsync<T>(T context, CancellationToken cancellationToken)
        where T : class;
}