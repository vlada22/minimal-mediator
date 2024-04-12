using MinimalMediator.Abstractions.Context;
using MinimalMediator.Abstractions.Middleware;
using MinimalMediator.Abstractions.Pipeline;

namespace MinimalMediator.Core.Pipe;

internal class PipeMiddleware<TContext, TMessage>(
    IMiddleware<TContext, TMessage> middleware,
    IPipe<TContext, TMessage> next)
    : IPipe<TContext, TMessage>
    where TContext : class, IPipeContext<TMessage>
    where TMessage : class
{
    public Task InvokeAsync(TContext context, CancellationToken cancellationToken)
    {
        return middleware.InvokeAsync(context, next, cancellationToken);
    }
}