// ReSharper disable All
namespace Microsoft.Extensions.DependencyInjection;

public interface IMediatorBuilder
{
    IServiceCollection Services { get; }

    IMediatorBuilder AddMiddleware(Type middlewareInterface, Type middlewareImplementation);
    IMediatorBuilder AddConsumer(Type consumerInterface, Type consumerImplementation);
    IMediatorBuilder AddReceiver(Type receiverInterface, Type receiverImplementation);
}