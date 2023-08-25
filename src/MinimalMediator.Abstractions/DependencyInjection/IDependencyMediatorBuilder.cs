#if NETSTANDARD2_0
using System.Diagnostics.CodeAnalysis.Polyfill;
#else
using System.Diagnostics.CodeAnalysis;
#endif

// ReSharper disable All
namespace Microsoft.Extensions.DependencyInjection;

public interface IDependencyMediatorBuilder
{
    IServiceCollection Services { get; }

    IDependencyMediatorBuilder AddMiddleware(Type middlewareInterface, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]Type middlewareImplementation);
    IDependencyMediatorBuilder AddConsumer(Type consumerInterface, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]Type consumerImplementation);
    IDependencyMediatorBuilder AddReceiver(Type receiverInterface, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]Type receiverImplementation);
}