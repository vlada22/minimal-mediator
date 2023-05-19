using MinimalMediator.Abstractions.Pipeline;
using MinimalMediator.Core;
using MinimalMediator.Core.DependencyInjection;
using MinimalMediator.Core.Middleware;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class MinimalMediatorServiceExtensions
{
    public static IServiceCollection AddMinimalMediator(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        services.Add(ServiceDescriptor.Describe(typeof(IMediatorDependencyContext),
            lifetime == ServiceLifetime.Scoped
                ? typeof(MediatorDependencyScopedContext)
                : typeof(MediatorDependencyContext), lifetime));

        services.AddTransient(typeof(IPipeMiddlewareBuilder<>), typeof(PipeMiddlewareBuilder<>));
        services.AddTransient(typeof(IPipeMiddlewareBuilder<,>), typeof(PipeMiddlewareBuilder<,>));

        services.Add(ServiceDescriptor.Describe(typeof(IMediator), typeof(MediatorDefault), lifetime));

        return services;
    }
}