using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace MinimalMediator.SourceGenerators;

[Generator]
public class MinimalMediatorGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var mediatorDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (n, _) => n is ClassDeclarationSyntax,
                transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
            .Where(static m => m is not null);

        var mediatorCompilationClasses = context.CompilationProvider.Combine(mediatorDeclarations.Collect());

        context.RegisterSourceOutput(mediatorCompilationClasses, 
            static (spc, source) => Execute(source.Right, spc));
    }

    private static void Execute(ImmutableArray<GeneratedService?> classes, SourceProductionContext spc)
    {
        spc.AddSource("MinimalMediatorSourceGeneratorExtensions.g.cs", 
            SourceText.From(SourceGeneratorParser.GenerateSource(classes), Encoding.UTF8));
    }

    // all classes that implement one of the Constants.MediatorTypes
    private static GeneratedService? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
    {
        if (context.Node is not ClassDeclarationSyntax cls)
        {
            return default;
        }
        
        if(context.SemanticModel.GetDeclaredSymbol(cls) is not { } classSymbol)
        {
            return default;
        }
        
        var interfaceType = classSymbol.AllInterfaces.FirstOrDefault(i => Constants.MediatorTypes.Contains(i.MetadataName));
        
        if (interfaceType is null)
        {
            return default;
        }

        var order = 0;
        foreach (var attributeData in classSymbol.GetAttributes())
        {
            if (attributeData.AttributeClass?.ToDisplayString() != Constants.EndpointAttributeFullName)
            {
                continue;
            }

            foreach (var namedArgument in attributeData.NamedArguments)
            {
                switch (namedArgument.Key)
                {
                    case "Order"
                        when namedArgument.Value.Value?.ToString() is { } orderValue
                             && int.TryParse(orderValue, out var orderInt):
                        order = orderInt;
                        continue;
                }
            }
        }

        return new GeneratedService
        {
            MetadataName = interfaceType.MetadataName,
            Interface = interfaceType.ToDisplayString(),
            Implementation = classSymbol.ToDisplayString(),
            Order = order
        };
    }
}