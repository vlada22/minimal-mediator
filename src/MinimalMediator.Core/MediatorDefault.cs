using System.Collections.Concurrent;
using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using MinimalMediator.Abstractions;
using MinimalMediator.Abstractions.Pipeline;
using MinimalMediator.Core.Container;

namespace MinimalMediator.Core;

public sealed class MediatorDefault : IMediator
{
    private readonly IMediatorDependencyContext _dependencyContext;
    private readonly ConcurrentDictionary<Type, object> _publishers = new();

    public MediatorDefault(IMediatorDependencyContext dependencyContext)
    {
        _dependencyContext = dependencyContext;
    }

    public IAsyncEnumerable<TResponse> ReceiveStreamAsync<TMessage, TResponse>(TMessage message, CancellationToken cancellationToken) where TMessage : class where TResponse : class
    {
        var stateMachine = _dependencyContext.ActivationServices
            .GetRequiredService<ISendStateMachine<TMessage, TResponse>>();

        return stateMachine.ProcessStreamAsync(message, cancellationToken);
    }

    public Task<ChannelReader<TResponse>> ReceiveChannelStreamAsync<TMessage, TResponse>(TMessage message, CancellationToken cancellationToken) where TMessage : class where TResponse : class
    {
        var stateMachine = _dependencyContext.ActivationServices
            .GetRequiredService<ISendStateMachine<TMessage, TResponse>>();

        return stateMachine.ProcessChannelStreamAsync(message, cancellationToken);
    }

    public Task<TResponse?> SendStreamAsync<TMessage, TResponse>(IAsyncEnumerable<TMessage> message, CancellationToken cancellationToken) where TMessage : class where TResponse : class
    {
        var stateMachine = _dependencyContext.ActivationServices
            .GetRequiredService<ISendStateMachine<TMessage, TResponse>>();

        return stateMachine.ProcessAsync(message, cancellationToken);
    }

    public Task<TResponse?> SendStreamAsync<TMessage, TResponse>(ChannelReader<TMessage> message, CancellationToken cancellationToken) where TMessage : class where TResponse : class
    {
        var stateMachine = _dependencyContext.ActivationServices
            .GetRequiredService<ISendStateMachine<TMessage, TResponse>>();

        return stateMachine.ProcessAsync(message, cancellationToken);
    }

    public Task<TResponse?> SendAsync<TMessage, TResponse>(TMessage message, CancellationToken cancellationToken) 
        where TMessage : class 
        where TResponse : class
    {
        var stateMachine = _dependencyContext.ActivationServices
            .GetRequiredService<ISendStateMachine<TMessage, TResponse>>();

        return stateMachine.ProcessAsync(message, cancellationToken);
    }

    public Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken) 
        where TMessage : class
    {
        var stateMachine = _dependencyContext.ActivationServices
            .GetRequiredService<IPublishStateMachine<TMessage>>();

        return stateMachine.ProcessAsync(message, cancellationToken);
    }
}