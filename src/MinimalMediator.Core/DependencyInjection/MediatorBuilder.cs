using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public sealed class MediatorBuilder : IMediatorBuilder
{
    public MediatorBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public IServiceCollection Services { get; }

    public IMediatorBuilder AddMiddleware(Type middlewareInterface, Type middlewareImplementation)
    {
        Services.TryAddEnumerable(ServiceDescriptor.Transient(middlewareInterface, middlewareImplementation));

        return this;
    }

    public IMediatorBuilder AddConsumer(Type consumerInterface, Type consumerImplementation)
    {
        Services.TryAddEnumerable(ServiceDescriptor.Transient(consumerInterface, consumerImplementation));

        return this;
    }

    public IMediatorBuilder AddReceiver(Type receiverInterface, Type receiverImplementation)
    {
        Services.TryAddTransient(receiverInterface, receiverImplementation);

        return this;
    }
}