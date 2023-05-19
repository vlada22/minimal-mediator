using MinimalMediator.Abstractions.Context;
using MinimalMediator.Abstractions.Middleware;
using MinimalMediator.Abstractions.Pipeline;

namespace MinimalMediator.Core.Middleware;

public class PipeMiddleware<TContext> : IPipe<TContext>
    where TContext : class, IPipeContext
{
    private readonly IMediatorMiddleware<TContext> _mediatorMiddleware;
    private readonly IPipe<TContext> _next;

    public PipeMiddleware(IMediatorMiddleware<TContext> mediatorMiddleware, IPipe<TContext> next)
    {
        _mediatorMiddleware = mediatorMiddleware;
        _next = next;
    }

    public Task InvokeAsync(TContext context, CancellationToken cancellationToken)
    {
        return _mediatorMiddleware.InvokeAsync(context, _next, cancellationToken);
    }
}

public class PipeMiddleware<TContext, TResult> : IPipe<TContext, TResult>
    where TContext : class, IPipeContext
    where TResult : class
{
    private readonly IMediatorMiddleware<TContext, TResult> _mediatorMiddleware;
    private readonly IPipe<TContext, TResult> _next;

    public PipeMiddleware(IMediatorMiddleware<TContext, TResult> mediatorMiddleware, IPipe<TContext, TResult> next)
    {
        _mediatorMiddleware = mediatorMiddleware;
        _next = next;
    }

    public Task<TResult?> InvokeAsync(TContext context, CancellationToken cancellationToken)
    {
        return _mediatorMiddleware.InvokeAsync(context, _next, cancellationToken);
    }
}