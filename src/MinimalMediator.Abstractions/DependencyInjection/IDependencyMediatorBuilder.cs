// ReSharper disable All
namespace Microsoft.Extensions.DependencyInjection;

public interface IDependencyMediatorBuilder
{
    IServiceCollection Services { get; }

    IDependencyMediatorBuilder AddMiddleware(Type middlewareInterface, Type middlewareImplementation);
    IDependencyMediatorBuilder AddConsumer(Type consumerInterface, Type consumerImplementation);
    IDependencyMediatorBuilder AddReceiver(Type receiverInterface, Type receiverImplementation);
}