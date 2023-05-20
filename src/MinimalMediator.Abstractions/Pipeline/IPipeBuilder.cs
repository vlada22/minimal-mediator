using MinimalMediator.Abstractions.Context;
using MinimalMediator.Abstractions.Middleware;

namespace MinimalMediator.Abstractions.Pipeline;

public interface IPipeBuilder
{
    public IPipe<TContext, TMessage> Build<TMiddleware, TContext, TMessage>()
        where TMiddleware : class, IMiddleware<TContext, TMessage>
        where TContext : class, IPipeContext<TMessage>
        where TMessage : class;
}