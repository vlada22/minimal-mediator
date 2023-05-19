using MinimalMediator.Abstractions.Pipeline;
using MinimalMediator.Core.Context;

namespace MinimalMediator.Core.Publish;

public class PipePublish : IPipe<PublishContext>
{
    public Task InvokeAsync(PublishContext context, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}