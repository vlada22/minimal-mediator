using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using MinimalMediator.Abstractions.Pipeline;
using MinimalMediator.Core.DependencyInjection;
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

    public Task<TResponse?> ProcessAsync(TMessage message, CancellationToken cancellationToken)
    {
        var consumer = _dependencyContext.ActivationServices.GetService<IReceiver<TMessage, TResponse>>();
        
        return consumer?.ReceiveAsync(message, cancellationToken) ?? Task.FromResult<TResponse?>(default);
    }

    public Task<TResponse?> ProcessAsync(ChannelReader<TMessage> reader, CancellationToken cancellationToken)
    {
        var consumer = _dependencyContext.ActivationServices.GetService<IReceiverStream<TMessage, TResponse>>();
        
        return consumer?.ReceiveAsync(reader, cancellationToken) ?? Task.FromResult<TResponse?>(default);
    }

    public Task<TResponse?> ProcessAsync(IAsyncEnumerable<TMessage> reader, CancellationToken cancellationToken)
    {
        var consumer = _dependencyContext.ActivationServices.GetService<IReceiverStreamAsync<TMessage, TResponse>>();
        
        return consumer?.ReceiveAsync(reader, cancellationToken) ?? Task.FromResult<TResponse?>(default);
    }
}