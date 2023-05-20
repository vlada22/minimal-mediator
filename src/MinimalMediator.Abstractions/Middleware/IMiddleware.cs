using MinimalMediator.Abstractions.Context;
using MinimalMediator.Abstractions.Pipeline;

namespace MinimalMediator.Abstractions.Middleware;

public interface IMiddleware<TContext, TMessage>
    where TContext : class, IPipeContext<TMessage>
    where TMessage : class
{
    Task InvokeAsync(TContext context, IPipe<TContext, TMessage> next, CancellationToken cancellationToken);
}