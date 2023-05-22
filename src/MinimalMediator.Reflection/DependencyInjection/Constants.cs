using MinimalMediator.Core.Messaging;
using MinimalMediator.Core.Middleware;
// ReSharper disable All

namespace Microsoft.Extensions.DependencyInjection;

public class Constants
{
    public static IReadOnlyList<Type> MediatorInterfaceTypes => new[]
    {
        typeof(IConsumer<>),
        typeof(IReceiver<,>),
        typeof(IReceiverStreamAsync<,>),
        typeof(IReceiverStreamChannel<,>),
        typeof(IReceiverConsumeStreamAsync<,>),
        typeof(IReceiverConsumeStreamChannel<,>),
        typeof(IAfterPublishMiddleware<>),
        typeof(IBeforePublishMiddleware<>),
        typeof(IExceptionHandlerMiddleware<>)
    };
}