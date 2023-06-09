using System.Collections.Immutable;
using System.Text;

namespace MinimalMediator.SourceGenerators;

public static class SourceGeneratorParser
{
    private const string Header = @"
// <auto-generated>
    //this file is generated by the MinimalMediator.SourceGenerators package.

    //Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>";

    public static string GenerateSource(ImmutableArray<GeneratedService?> classes)
    {
        var source = new StringBuilder();
        source.AppendLine(Header);

        source.Append(@"
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class MinimalMediatorSourceGeneratorExtensions
{
    public static IDependencyMediatorBuilder UseSourceGenerator(this IDependencyMediatorBuilder builder)
    {
");
        foreach (var service in classes.ToList().OrderBy(x => x?.Order ?? 0))
        {
            if (service is null)
            {
                continue;
            }
            
            source.AppendLine(Constants.TransientServices.Contains(service.MetadataName)
                ? $"            builder.Services.AddTransient(typeof({service.Interface}), typeof({service.Implementation}));"
                : $"            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient(typeof({service.Interface}), typeof({service.Implementation})));");
        }
        
        source.Append(@"
        return builder;
    }
}
");

        return source.ToString();
    }
}