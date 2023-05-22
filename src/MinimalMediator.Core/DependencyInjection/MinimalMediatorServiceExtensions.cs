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
    public static IMediatorBuilder AddMinimalMediator(this IServiceCollection services,
        Action<IMediatorBuilder>? configure = default,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        services.Add(ServiceDescriptor.Describe(typeof(IMediatorDependencyContext),
            lifetime == ServiceLifetime.Scoped
                ? typeof(MediatorDependencyScopedContext)
                : typeof(MediatorDependencyContext),
            lifetime == ServiceLifetime.Scoped ? ServiceLifetime.Scoped : ServiceLifetime.Singleton));
        
        services.AddTransient(typeof(IPipeBuilder), typeof(PipeBuilder));
        services.AddTransient(typeof(IPublishMiddleware<>), typeof(PublishMiddlewareDefault<>));
        services.AddTransient(typeof(IPublishStateMachine<>), typeof(PublishStateMachineDefault<>));
        services.AddTransient(typeof(ISendStateMachine<,>), typeof(SendStateMachineDefault<,>));

        services.Add(ServiceDescriptor.Describe(typeof(IMediator), typeof(MediatorDefault), lifetime));

        var builder = new MediatorBuilder(services);
        configure?.Invoke(builder);
        return builder;
    }
}