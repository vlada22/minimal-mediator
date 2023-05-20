using MinimalMediator.Abstractions.Middleware;
using MinimalMediator.Core.Context;

namespace MinimalMediator.Core.Middleware;

public interface IExceptionHandlerMiddleware<TMessage> : IMiddleware<ExceptionHandlerMiddlewareContext<TMessage>, TMessage>
    where TMessage : class
{
}