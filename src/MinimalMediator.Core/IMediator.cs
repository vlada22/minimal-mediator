using MinimalMediator.Abstractions.Context;

namespace MinimalMediator.Core;

public interface IMediator : IAsyncDisposable
{
    Task Test<TContext>(TContext context, CancellationToken cancellationToken)
        where TContext : class, IPipeContext;
}