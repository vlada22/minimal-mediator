using MinimalMediator.Abstractions.Middleware;
using MinimalMediator.Core.Context;

namespace MinimalMediator.Core.Middleware;

/// <summary>
/// Middleware that executes when an exception is thrown.
/// </summary>
/// <typeparam name="TMessage"></typeparam>
public interface IExceptionHandlerMiddleware<TMessage> : IMiddleware<ExceptionHandlerMiddlewareContext<TMessage>, TMessage>
    where TMessage : class
{
}