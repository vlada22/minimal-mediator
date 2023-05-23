// ReSharper disable All
namespace Microsoft.Extensions.DependencyInjection;

public static class MinimalMediatorReflectionExtensions
{
    public static IDependencyMediatorBuilder UseReflection(this IDependencyMediatorBuilder builder, params Type[] types)
    {
        TypeResolverReflection.Resolve(builder.Services, types);

        return builder;
    }
}