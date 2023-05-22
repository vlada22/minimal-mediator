namespace MinimalMediator.SourceGenerators;

public static class Constants
{
    public const string EndpointAttributeFullName = "Microsoft.Extensions.DependencyInjection.MinimalMediatorAttribute";
    
    public static IReadOnlyList<string> MediatorTypes => new[]
    {
        "IReceiver`2",
        "IReceiverStreamAsync`2",
        "IReceiverStreamChannel`2",
        "IReceiverConsumeStreamAsync`2",
        "IReceiverConsumeStreamChannel`2",
        "IAfterPublishMiddleware`1",
        "IBeforePublishMiddleware`1",
        "IExceptionHandlerMiddleware`1",
        "IConsumer`1"
    };

    public static IReadOnlyList<string> TransientServices => new[]
    {
        "IReceiver`2",
        "IReceiverStreamAsync`2",
        "IReceiverStreamChannel`2",
        "IReceiverConsumeStreamAsync`2",
        "IReceiverConsumeStreamChannel`2",
    };
}