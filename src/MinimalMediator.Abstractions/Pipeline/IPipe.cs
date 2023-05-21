using MinimalMediator.Abstractions.Context;

namespace MinimalMediator.Abstractions.Pipeline;

/// <summary>
/// Contract for a pipe.
/// </summary>
/// <typeparam name="TContext"></typeparam>
/// <typeparam name="TMessage"></typeparam>
public interface IPipe<in TContext, TMessage> 
    where TContext : class, IPipeContext<TMessage>
    where TMessage : class
{
    /// <summary>
    /// Handles the pipe.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task InvokeAsync(TContext context, CancellationToken cancellationToken);
}