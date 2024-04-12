using MinimalMediator.Abstractions;
using MinimalMediator.Abstractions.Pipeline;
using MinimalMediator.Core;
using MinimalMediator.Core.Container;
using MinimalMediator.Core.Middleware;
using MinimalMediator.Core.Pipe;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class MinimalMediatorServiceExtensions
{
    public static IDependencyMediatorBuilder AddMinimalMediator(this IServiceCollection services,
        Action<IDependencyMediatorBuilder>? configure = default,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        services.Add(ServiceDescriptor.Describe(typeof(IMediatorDependencyContext),
            lifetime == ServiceLifetime.Scoped
                ? typeof(MediatorDependencyScopedContext)
                : typeof(MediatorDependencyContext), 
            lifetime));
        
        services.AddTransient(typeof(IPipeBuilder), typeof(PipeBuilder));
        services.AddTransient(typeof(IPublishMiddleware<>), typeof(PublishMiddlewareDefault<>));
        services.AddTransient(typeof(IPublishStateMachine<>), typeof(PublishStateMachineDefault<>));
        services.AddTransient(typeof(ISendStateMachine<,>), typeof(SendStateMachineDefault<,>));

        services.Add(ServiceDescriptor.Describe(typeof(IMediator), typeof(MediatorDefault), lifetime));

        var builder = new DependencyMediatorBuilder(services);
        configure?.Invoke(builder);
        return builder;
    }
}