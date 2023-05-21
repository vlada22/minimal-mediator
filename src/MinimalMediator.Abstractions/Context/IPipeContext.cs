namespace MinimalMediator.Abstractions.Context;

/// <summary>
/// The context of a pipe.
/// </summary>
/// <typeparam name="TMessage"></typeparam>
public interface IPipeContext<out TMessage>
    where TMessage : class
{
    /// <summary>
    /// Id of the context.
    /// </summary>
    Guid Id { get; }
    
    /// <summary>
    /// Message of the context.
    /// </summary>
    TMessage? Message { get; }
}