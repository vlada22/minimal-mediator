// ReSharper disable All
namespace Microsoft.Extensions.DependencyInjection;

public class MinimalMediatorAttribute : Attribute
{
    public MinimalMediatorAttribute(int order)
    {
        Order = order;
    }
    
    public int Order { get; }
}