using Microsoft.Extensions.DependencyInjection;

namespace MinimalMediator.Core.DependencyInjection;

public interface IMediatorDependencyContext : IAsyncDisposable
{
    IServiceProvider ActivationServices { get; }
}

public sealed class MediatorDependencyContext : IMediatorDependencyContext
{
    private static ValueTask Empty => new();
    
    public MediatorDependencyContext(IServiceProvider serviceProvider)
    {
        ActivationServices = serviceProvider;
    }

    public IServiceProvider ActivationServices { get; }

    public ValueTask DisposeAsync()
    {
#if NETSTANDARD2_0
        return Empty;
#else
        return ValueTask.CompletedTask;
#endif
    }
}

public sealed class MediatorDependencyScopedContext : IMediatorDependencyContext
{
    private readonly AsyncServiceScope _scope;

    public MediatorDependencyScopedContext(IServiceProvider serviceProvider)
    {
        _scope = serviceProvider.CreateAsyncScope();
    }

    public IServiceProvider ActivationServices => _scope.ServiceProvider;

    public ValueTask DisposeAsync()
    {
        return _scope.DisposeAsync();
    }
}