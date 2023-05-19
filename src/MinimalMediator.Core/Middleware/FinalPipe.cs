using MinimalMediator.Abstractions.Context;
using MinimalMediator.Abstractions.Pipeline;

namespace MinimalMediator.Core.Middleware;

public class FinalPipe<TContext> : IPipe<TContext> 
    where TContext : class, IPipeContext
{
    public Task InvokeAsync(TContext context, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    
    public static FinalPipe<TContext> Empty { get; } = new();
}

public class FinalPipe<TContext, TResult> : IPipe<TContext, TResult>
    where TContext : class, IPipeContext
    where TResult : class
{
    public Task<TResult?> InvokeAsync(TContext context, CancellationToken cancellationToken)
    {
        return Task.FromResult<TResult?>(default);
    }
    
    public static FinalPipe<TContext, TResult> Empty { get; } = new();
}