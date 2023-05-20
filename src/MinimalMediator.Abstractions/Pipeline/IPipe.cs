using MinimalMediator.Abstractions.Context;

namespace MinimalMediator.Abstractions.Pipeline;

public interface IPipe<in TContext, TMessage> 
    where TContext : class, IPipeContext<TMessage>
    where TMessage : class
{
    Task InvokeAsync(TContext context, CancellationToken cancellationToken);
}