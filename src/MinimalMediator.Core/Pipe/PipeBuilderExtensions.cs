using MinimalMediator.Abstractions.Pipeline;
using MinimalMediator.Core.Context;
using MinimalMediator.Core.Middleware;

namespace MinimalMediator.Core.Pipe;

public static class PipeBuilderExtensions
{
    public static IPipe<PreProcessMiddlewareContext<TMessage>, TMessage> BuildBeforePublishMiddleware<TMessage>(this IPipeBuilder builder)
        where TMessage : class
    {
        return builder.Build<IBeforePublishMiddleware<TMessage>, PreProcessMiddlewareContext<TMessage>, TMessage>();
    }
    
    public static IPipe<PostProcessMiddlewareContext<TMessage>, TMessage> BuildAfterPublishMiddleware<TMessage>(this IPipeBuilder builder)
        where TMessage : class
    {
        return builder.Build<IAfterPublishMiddleware<TMessage>, PostProcessMiddlewareContext<TMessage>, TMessage>();
    }
    
    public static IPipe<PublishMiddlewareContext<TMessage>, TMessage> BuildPublishMiddleware<TMessage>(this IPipeBuilder builder)
        where TMessage : class
    {
        return builder.Build<IPublishMiddleware<TMessage>, PublishMiddlewareContext<TMessage>, TMessage>();
    }
    
    public static IPipe<ExceptionHandlerMiddlewareContext<TMessage>, TMessage> BuildExceptionHandlerMiddleware<TMessage>(this IPipeBuilder builder)
        where TMessage : class
    {
        return builder.Build<IExceptionHandlerMiddleware<TMessage>, ExceptionHandlerMiddlewareContext<TMessage>, TMessage>();
    }
}