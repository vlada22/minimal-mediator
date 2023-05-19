using MinimalMediator.Abstractions.Context;

namespace MinimalMediator.Abstractions.Pipeline;

public interface IPipe<in TContext> 
    where TContext : class, IPipeContext
{
    Task InvokeAsync(TContext context, CancellationToken cancellationToken);
}

public interface IPipe<in TContext, TResult> 
    where TContext : class, IPipeContext 
    where TResult : class
{
    Task<TResult?> InvokeAsync(TContext context, CancellationToken cancellationToken);
}