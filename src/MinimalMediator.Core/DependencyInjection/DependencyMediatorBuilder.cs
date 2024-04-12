using Microsoft.Extensions.DependencyInjection.Extensions;

#if NETSTANDARD2_0
using System.Diagnostics.CodeAnalysis.Polyfill;
#else
using System.Diagnostics.CodeAnalysis;
#endif

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public sealed class DependencyMediatorBuilder(IServiceCollection services) : IDependencyMediatorBuilder
{
    public IServiceCollection Services { get; } = services;

    public IDependencyMediatorBuilder AddMiddleware(Type middlewareInterface,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type middlewareImplementation)
    {
        Services.TryAddEnumerable(ServiceDescriptor.Transient(middlewareInterface, middlewareImplementation));

        return this;
    }

    public IDependencyMediatorBuilder AddConsumer(Type consumerInterface,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type consumerImplementation)
    {
        Services.TryAddEnumerable(ServiceDescriptor.Transient(consumerInterface, consumerImplementation));

        return this;
    }

    public IDependencyMediatorBuilder AddReceiver(Type receiverInterface,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type receiverImplementation)
    {
        Services.TryAddTransient(receiverInterface, receiverImplementation);

        return this;
    }
}