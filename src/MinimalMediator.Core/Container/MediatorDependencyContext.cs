using Microsoft.Extensions.DependencyInjection;

namespace MinimalMediator.Core.Container;

internal interface IMediatorDependencyContext : IAsyncDisposable
{
    IServiceProvider ActivationServices { get; }
}

internal sealed class MediatorDependencyContext(IServiceProvider serviceProvider) : IMediatorDependencyContext
{
    private static ValueTask Empty => new();

    public IServiceProvider ActivationServices { get; } = serviceProvider;

    public ValueTask DisposeAsync()
    {
#if NETSTANDARD2_0
        return Empty;
#else
        return ValueTask.CompletedTask;
#endif
    }
}

internal sealed class MediatorDependencyScopedContext(IServiceProvider serviceProvider) : IMediatorDependencyContext
{
    private readonly AsyncServiceScope _scope = serviceProvider.CreateAsyncScope();

    public IServiceProvider ActivationServices => _scope.ServiceProvider;

    public ValueTask DisposeAsync()
    {
        return _scope.DisposeAsync();
    }
}