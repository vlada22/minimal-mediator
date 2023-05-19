using Microsoft.Extensions.DependencyInjection;
using MinimalMediator.Abstractions.Pipeline;
using MinimalMediator.Core.Middleware;

namespace MinimalMediator.Core.DependencyInjection;

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

        services.Add(ServiceDescriptor.Describe(typeof(IMediator), typeof(MinimalMediator), lifetime));

        return services;
    }
}