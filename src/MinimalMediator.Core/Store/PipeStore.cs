using System.Collections.Concurrent;
using MinimalMediator.Abstractions.Context;
using MinimalMediator.Abstractions.Pipeline;

namespace MinimalMediator.Core.Store;

public class PipeStore<TContext>
    where TContext : class, IPipeContext
{
    private readonly ConcurrentDictionary<Type, Lazy<IPipe<TContext>>> _pipes = new();

    public bool TryAddPipe(Type type, IPipe<TContext> pipe)
    {
        return _pipes.TryAdd(type, new Lazy<IPipe<TContext>>(() => pipe));
    }

    public bool TryGetPipe(Type type, out IPipe<TContext>? pipe)
    {
        if (_pipes.TryGetValue(type, out var lazyPipe))
        {
            pipe = lazyPipe.Value;
            return true;
        }

        pipe = default;
        return false;
    }
}