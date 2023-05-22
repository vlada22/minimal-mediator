// ReSharper disable All
namespace Microsoft.Extensions.DependencyInjection;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class MinimalMediatorAttribute : Attribute
{
    public int Order { get; set; }
}