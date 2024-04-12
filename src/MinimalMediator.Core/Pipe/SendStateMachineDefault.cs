using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using MinimalMediator.Abstractions.Pipeline;
using MinimalMediator.Core.Container;
using MinimalMediator.Core.Messaging;

namespace MinimalMediator.Core.Pipe;

internal class SendStateMachineDefault<TMessage, TResponse>(IMediatorDependencyContext dependencyContext)
    : ISendStateMachine<TMessage, TResponse>
    where TMessage : class
    where TResponse : class
{
    public IAsyncEnumerable<TResponse> ProcessStreamAsync(TMessage message, CancellationToken cancellationToken)
    {
        var consumer = dependencyContext.ActivationServices.GetRequiredService<IReceiverConsumeStreamAsync<TMessage, TResponse>>();
        
        return consumer.ReceiveAsync(message, cancellationToken);
    }

    public Task<ChannelReader<TResponse>> ProcessChannelStreamAsync(TMessage message, CancellationToken cancellationToken)
    {
        var consumer = dependencyContext.ActivationServices.GetRequiredService<IReceiverConsumeStreamChannel<TMessage, TResponse>>();
        
        return consumer.ReceiveAsync(message, cancellationToken);
    }

    public Task<TResponse?> ProcessAsync(TMessage message, CancellationToken cancellationToken)
    {
        var consumer = dependencyContext.ActivationServices.GetRequiredService<IReceiver<TMessage, TResponse>>();

        return consumer.ReceiveAsync(message, cancellationToken);
    }

    public Task<TResponse?> ProcessAsync(ChannelReader<TMessage> reader, CancellationToken cancellationToken)
    {
        var consumer = dependencyContext.ActivationServices.GetRequiredService<IReceiverStreamChannel<TMessage, TResponse>>();
        
        return consumer.ReceiveAsync(reader, cancellationToken);
    }

    public Task<TResponse?> ProcessAsync(IAsyncEnumerable<TMessage> reader, CancellationToken cancellationToken)
    {
        var consumer = dependencyContext.ActivationServices.GetRequiredService<IReceiverStreamAsync<TMessage, TResponse>>();
        
        return consumer.ReceiveAsync(reader, cancellationToken);
    }
}