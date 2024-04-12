using MinimalMediator.Abstractions.Pipeline;
using MinimalMediator.Core.Context;

namespace MinimalMediator.Core.Pipe;

internal class PublishStateMachineDefault<TMessage>(IPipeBuilder builder) : IPublishStateMachine<TMessage>
    where TMessage : class
{
    public async Task ProcessAsync(TMessage message, CancellationToken cancellationToken)
    {
        var pipeBefore = builder.BuildBeforePublishMiddleware<TMessage>();
        var pipeAfter = builder.BuildAfterPublishMiddleware<TMessage>();
        var pipePublish = builder.BuildPublishMiddleware<TMessage>();
        var pipeException = builder.BuildExceptionHandlerMiddleware<TMessage>();

        try
        {
            await pipeBefore.InvokeAsync(new PreProcessMiddlewareContext<TMessage>(message), cancellationToken);
            await pipePublish.InvokeAsync(new PublishMiddlewareContext<TMessage>(message), cancellationToken);
            await pipeAfter.InvokeAsync(new PostProcessMiddlewareContext<TMessage>(message), cancellationToken);
        }
        catch (Exception ex)
        {
            // If there is no exception handler, rethrow the exception since the default behavior is to throw
            if (pipeException == FirstPipe<ExceptionHandlerMiddlewareContext<TMessage>, TMessage>.Empty)
            {
                throw;
            }
            
            // Otherwise, invoke the exception handler
            await pipeException.InvokeAsync(new ExceptionHandlerMiddlewareContext<TMessage>(message, ex), cancellationToken);
        }
    }
}