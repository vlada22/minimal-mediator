# Minimal Mediator
![Github actions](https://github.com/vlada22/minimal-mediator/actions/workflows/build-release.yml/badge.svg)
[![Nuget feed](https://img.shields.io/nuget/v/MinimalMediator?label=MinimalMediator)](https://www.nuget.org/packages/MinimalMediator)

Minimal Mediator is a lightweight library for implementing the mediator pattern in .NET Minimal APIs.

Lightweight and fast, it is designed to be used in high-performance applications with no overhead.

Minimal Mediator runs in-process without any serialization or reflection.

Native AOT compilation is supported.

Support for publishing/sending messages.

Support for streaming requests and responses based on **System.Threading.Channels** and **IAsyncEnumerable**.

The implementation provides a simple middleware pipeline workflow for handling requests and responses that is easy to extend and customize. 

## Installation
Install the [MinimalMediator](https://www.nuget.org/packages/MinimalMediator) NuGet package.
```bash
dotnet add package MinimalMediator
```

## Initialization
Minimal Mediator supports `Microsoft.Extensions.DependencyInjection` and `Microsoft.Extensions.Hosting` for dependency injection and configuration.

There are three possible ways to initialize the mediator services:
- using **Source Generators** - automatically by scanning the assembly for handlers and generating the mediator code
```csharp
builder.Services.AddMinimalMediator(config => config.UseSourceGenerator());
```
- using **Reflection** - automatically by scanning the assembly for handlers and registering them in the service collection
```csharp
builder.Services.AddMinimalMediator(config => config.UseReflection(typeof(Program)));
```

- Manually by registering the mediator and handlers in the service collection
```csharp
builder.Services.AddMinimalMediator(config =>
{
    config.AddConsumer(typeof(IConsumer<TestMessage>), typeof(Consumer2));
    config.AddMiddleware(typeof(IAfterPublishMiddleware<TestMessage>), typeof(AfterMiddleware1));
    config.AddReceiver(typeof(IReceiver<TestMessage, TestResponse>), typeof(ReceiverTest));
});
```

## Usage
Minimal Mediator supports two types of invocations:
- **Publish/Subscribe** - broadcast a message to all consumers
- **Request/Response** - send a message and receive a response. This also supports streaming requests and responses.

### Publish/Subscribe
Minimal Mediator supports publishing messages to all consumers that implement the `IConsumer<TMessage>` interface.
```csharp
public interface IConsumer<TMessage>
{
    Task ConsumeAsync(TMessage message, CancellationToken cancellationToken);
}
```
The mediator will invoke all consumers in parallel.

### Middleware
Because the mediator implements middleware pipelines, it is possible to add middleware that will be invoked before and after the message is published.

`This behavior is only valid for the Publish/Subscribe invocation. The Request/Response invocation does not support middleware.`

There are three types of middleware:
- **IBeforePublishMiddleware** - invoked before the message is published
- **IAfterPublishMiddleware** - invoked after the message is published
- **IExceptionHandlerMiddleware** - invoked if an exception is thrown during the message publishing

Multiple middleware can be registered of a specific type/message and they will be invoked in the order they are registered.

To control the order of middleware, use the `Order` property of the `MinimalMediatorAttribute` class. The order is important only for middleware of the same type.

The presence of this attribute is not required. If it is not present, the middleware will be invoked in the order they are registered. 

```csharp
[MinimalMediator(Order = 2)]
public class AfterMiddleware2 : IAfterPublishMiddleware<TestMessage>
{
    ...
}
```

### Request/Response
Minimal Mediator supports sending and receiving messages and streams. The functionalities are provided by the following interfaces:
- **IReceiver<TMessage, TResponse>** - used for sending a message and receiving a response
- **IReceiverStreamAsync<TMessage, TResponse>** - used for sending a **IAsyncEnumerable<TMessage>** stream of messages and receiving a TResponse message
- **IReceiverStreamChannel<TMessage, TResponse>** - used for sending a **ChannelReader<TMessage>** stream of messages and receiving a TResponse message
- **IReceiverConsumeStreamAsync<TMessage, TResponse>** - used for sending a TMessage message and receiving a **IAsyncEnumerable<TResponse>** stream of messages
- **IReceiverConsumeStreamChannel<TMessage, TResponse>** - used for sending a TMessage message and receiving a **ChannelReader<TResponse>** stream of messages

`See the sample project and tests for more details.`

### Service Lifetime
The predefined lifetime of the **Publish/Subscribe** and **Request/Response** invocations is **Transient**. This behavior cannot be changed.

The default lifetime of the **IMediator** service is **Singleton**. There is also a **Scoped** lifetime available that can be enabled from the configuration.

```csharp
builder.Services.AddMinimalMediator(config => config.UseSourceGenerator(), ServiceLifetime.Scoped);
```

The mediator will be registered as **Scoped** but the invocations will still be **Transient**.

In addition, when using the **Scoped** lifetime, the mediator creates a new scope for each invocation.

## License
[MIT](https://choosealicense.com/licenses/mit/)