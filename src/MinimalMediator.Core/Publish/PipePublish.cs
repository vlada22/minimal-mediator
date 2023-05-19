using MinimalMediator.Abstractions;
using MinimalMediator.Abstractions.Pipeline;
using MinimalMediator.Core.Context;

namespace MinimalMediator.Core.Publish;

public class PipePublish<TMessage> : IPipe<PublishContext<TMessage>>
    where TMessage : class
{
    private readonly List<IConsumer<PublishContext<TMessage>>> _consumers;
    
    public PipePublish(IEnumerable<IConsumer<PublishContext<TMessage>>> consumers)
    {
        _consumers = consumers.ToList();
    }

    public Task InvokeAsync(PublishContext<TMessage> context, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}