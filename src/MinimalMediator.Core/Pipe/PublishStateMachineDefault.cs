using MinimalMediator.Abstractions.Pipeline;
using MinimalMediator.Core.Context;

namespace MinimalMediator.Core.Pipe;

public class PublishStateMachineDefault<TMessage> : IPublishStateMachine<TMessage>
    where TMessage : class
{
    private readonly IPipeBuilder _builder;

    public PublishStateMachineDefault(IPipeBuilder builder)
    {
        _builder = builder;
    }

    public async Task ProcessAsync(TMessage message, CancellationToken cancellationToken)
    {
        var pipeBefore = _builder.BuildBeforePublishMiddleware<TMessage>();
        var pipeAfter = _builder.BuildAfterPublishMiddleware<TMessage>();
        var pipePublish = _builder.BuildPublishMiddleware<TMessage>();
        var pipeException = _builder.BuildExceptionHandlerMiddleware<TMessage>();

        try
        {
            await pipeBefore.InvokeAsync(new PreProcessMiddlewareContext<TMessage>(message), cancellationToken);
            await pipePublish.InvokeAsync(new PublishMiddlewareContext<TMessage>(message), cancellationToken);
            await pipeAfter.InvokeAsync(new PostProcessMiddlewareContext<TMessage>(message), cancellationToken);
        }
        catch (OperationCanceledException)
        {
            // Ignore
        }
        catch (Exception ex)
        {
            await pipeException.InvokeAsync(new ExceptionHandlerMiddlewareContext<TMessage>(message, ex), cancellationToken);
        }
    }
}