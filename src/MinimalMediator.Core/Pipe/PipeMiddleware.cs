using MinimalMediator.Abstractions.Context;
using MinimalMediator.Abstractions.Middleware;
using MinimalMediator.Abstractions.Pipeline;

namespace MinimalMediator.Core.Pipe;

public class PipeMiddleware<TContext, TMessage> : IPipe<TContext, TMessage>
    where TContext : class, IPipeContext<TMessage>
    where TMessage : class
{
    private readonly IMiddleware<TContext, TMessage> _middleware;
    private readonly IPipe<TContext, TMessage> _next;

    public PipeMiddleware(IMiddleware<TContext, TMessage> middleware, IPipe<TContext, TMessage> next)
    {
        _middleware = middleware;
        _next = next;
    }

    public Task InvokeAsync(TContext context, CancellationToken cancellationToken)
    {
        return _middleware.InvokeAsync(context, _next, cancellationToken);
    }
}