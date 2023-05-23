using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public sealed class DependencyMediatorBuilder : IDependencyMediatorBuilder
{
    public DependencyMediatorBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public IServiceCollection Services { get; }

    public IDependencyMediatorBuilder AddMiddleware(Type middlewareInterface, Type middlewareImplementation)
    {
        Services.TryAddEnumerable(ServiceDescriptor.Transient(middlewareInterface, middlewareImplementation));

        return this;
    }

    public IDependencyMediatorBuilder AddConsumer(Type consumerInterface, Type consumerImplementation)
    {
        Services.TryAddEnumerable(ServiceDescriptor.Transient(consumerInterface, consumerImplementation));

        return this;
    }

    public IDependencyMediatorBuilder AddReceiver(Type receiverInterface, Type receiverImplementation)
    {
        Services.TryAddTransient(receiverInterface, receiverImplementation);

        return this;
    }
}