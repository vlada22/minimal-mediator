// ReSharper disable All
namespace Microsoft.Extensions.DependencyInjection;

public static class MinimalMediatorReflectionExtensions
{
    public static IMediatorBuilder UseReflection(this IMediatorBuilder builder, params Type[] types)
    {
        TypeResolverReflection.Resolve(builder.Services, types);

        return builder;
    }
}