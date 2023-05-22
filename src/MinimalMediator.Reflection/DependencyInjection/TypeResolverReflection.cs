using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MinimalMediator.Core.Messaging;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class TypeResolverReflection
{
    public static void Resolve(IServiceCollection services, params Type[]? assemblyTypes)
    {
        var assembliesToScan = assemblyTypes?.Select(x => x.Assembly).ToArray() ?? AppDomain.CurrentDomain.GetAssemblies();

        foreach (var type in Constants.MediatorInterfaceTypes)
        {
            var types = assembliesToScan.SelectMany(x => x.GetTypes())
                .Where(x => x.IsClass && !x.IsAbstract &&
                            x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == type))
                .OrderBy(x => x.GetCustomAttribute<MinimalMediatorAttribute>()?.Order ?? 0)
                .ToArray();

            foreach (var t in types)
            {
                var interfaces = t.GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == type)
                    .ToArray();

                foreach (var i in interfaces)
                {
                    if (IsNotEnumerableService(type))
                    {
                        services.TryAddTransient(i, t);
                    }
                    else
                    {
                        services.TryAddEnumerable(ServiceDescriptor.Transient(i, t));
                    }
                }
            }
        }
    }

    private static bool IsNotEnumerableService(Type type) =>
        type == typeof(IReceiver<,>) ||
        type == typeof(IReceiverStreamAsync<,>) ||
        type == typeof(IReceiverStreamChannel<,>) ||
        type == typeof(IReceiverConsumeStreamAsync<,>) ||
        type == typeof(IReceiverConsumeStreamChannel<,>);
}