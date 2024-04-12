using MinimalMediator.Abstractions.Context;
using MinimalMediator.Abstractions.Pipeline;

namespace MinimalMediator.Core.Pipe;

internal class FirstPipe<TContext, TMessage> : IPipe<TContext, TMessage>
    where TContext : class, IPipeContext<TMessage>
    where TMessage : class
{
    public Task InvokeAsync(TContext context, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public static FirstPipe<TContext, TMessage> Empty { get; } = new();
}