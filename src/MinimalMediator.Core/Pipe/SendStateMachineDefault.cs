using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using MinimalMediator.Abstractions.Pipeline;
using MinimalMediator.Core.Container;
using MinimalMediator.Core.Messaging;

namespace MinimalMediator.Core.Pipe;

public class SendStateMachineDefault<TMessage, TResponse> : ISendStateMachine<TMessage, TResponse>
    where TMessage : class
    where TResponse : class
{
    private readonly IMediatorDependencyContext _dependencyContext;

    public SendStateMachineDefault(IMediatorDependencyContext dependencyContext)
    {
        _dependencyContext = dependencyContext;
    }

    public IAsyncEnumerable<TResponse> ProcessStreamAsync(TMessage message, CancellationToken cancellationToken)
    {
        var consumer = _dependencyContext.ActivationServices.GetRequiredService<IReceiverConsumeStreamAsync<TMessage, TResponse>>();
        
        return consumer.ReceiveAsync(message, cancellationToken);
    }

    public Task<ChannelReader<TResponse>> ProcessChannelStreamAsync(TMessage message, CancellationToken cancellationToken)
    {
        var consumer = _dependencyContext.ActivationServices.GetRequiredService<IReceiverConsumeStreamChannel<TMessage, TResponse>>();
        
        return consumer.ReceiveAsync(message, cancellationToken);
    }

    public Task<TResponse?> ProcessAsync(TMessage message, CancellationToken cancellationToken)
    {
        var consumer = _dependencyContext.ActivationServices.GetRequiredService<IReceiver<TMessage, TResponse>>();

        return consumer.ReceiveAsync(message, cancellationToken);
    }

    public Task<TResponse?> ProcessAsync(ChannelReader<TMessage> reader, CancellationToken cancellationToken)
    {
        var consumer = _dependencyContext.ActivationServices.GetRequiredService<IReceiverStreamChannel<TMessage, TResponse>>();
        
        return consumer.ReceiveAsync(reader, cancellationToken);
    }

    public Task<TResponse?> ProcessAsync(IAsyncEnumerable<TMessage> reader, CancellationToken cancellationToken)
    {
        var consumer = _dependencyContext.ActivationServices.GetRequiredService<IReceiverStreamAsync<TMessage, TResponse>>();
        
        return consumer.ReceiveAsync(reader, cancellationToken);
    }
}