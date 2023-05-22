using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using MinimalMediator.Abstractions;

namespace UnitTests;

public class ReceiverTests
{
    [Fact]
    public async Task ShouldReceiveMessage()
    {
        var services = new ServiceCollection();
        services.AddMinimalMediator(c => c.UseReflection(typeof(ConsumerTest)));
        services.AddSingleton<SharedService>();
        services.AddTransient<LifeTimeService>();
        var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();

        var response =
            await mediator.SendAsync<ReceiverMessage, ReceiverResponse>(new ReceiverMessage("test message"),
                CancellationToken.None);

        Assert.NotNull(response);
        Assert.Equal("test message", response.Message);
    }

    [Fact]
    public async Task ShouldSendStreamAsync()
    {
        var services = new ServiceCollection();
        services.AddMinimalMediator(c => c.UseReflection(typeof(ConsumerTest)));
        services.AddSingleton<SharedService>();
        services.AddTransient<LifeTimeService>();
        var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();
        var sharedService = provider.GetRequiredService<SharedService>();

        async IAsyncEnumerable<ReceiverMessage> Stream()
        {
            for (var i = 0; i < 5; i++)
            {
                yield return new ReceiverMessage(i.ToString());
                await Task.Delay(1);
            }
        }

        await mediator.SendStreamAsync<ReceiverMessage, ReceiverResponse>(Stream(), CancellationToken.None);

        Assert.True(sharedService.HandlerData.Cast<ReceiverMessage>().Select(x => x.Message)
            .SequenceEqual(GetExpectedStreamMessages()));
    }

    [Fact]
    public async Task ShouldSendStreamChannel()
    {
        var services = new ServiceCollection();
        services.AddMinimalMediator(c => c.UseReflection(typeof(ConsumerTest)));
        services.AddSingleton<SharedService>();
        services.AddTransient<LifeTimeService>();
        var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();
        var sharedService = provider.GetRequiredService<SharedService>();

        var channel = Channel.CreateUnbounded<ReceiverMessage>();

        _ = Task.Run(async () =>
            {
                for (var i = 0; i < 5; i++)
                {
                    await channel.Writer.WriteAsync(new ReceiverMessage(i.ToString()));
                    await Task.Delay(1);
                }

                channel.Writer.Complete();
            }
        );

        await mediator.SendStreamAsync<ReceiverMessage, ReceiverResponse>(channel.Reader,
            CancellationToken.None);

        Assert.True(sharedService.HandlerData.Cast<ReceiverMessage>().Select(x => x.Message)
            .SequenceEqual(GetExpectedStreamMessages()));
    }

    [Fact]
    public async Task ShouldConsumeStream()
    {
        var services = new ServiceCollection();
        services.AddMinimalMediator(c => c.UseReflection(typeof(ConsumerTest)));
        services.AddSingleton<SharedService>();
        services.AddTransient<LifeTimeService>();
        var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();

        var stream = mediator.ReceiveStreamAsync<ReceiverMessage, ReceiverResponse>(new ReceiverMessage("test"), CancellationToken.None);
        var result = new List<string>();
        await foreach (var item in stream)
        {
            result.Add(item.Message);
        }
        
        Assert.True(result.SequenceEqual(GetExpectedStreamMessages()));
    }

    [Fact]
    public async Task ShouldConsumeStreamChannel()
    {
        var services = new ServiceCollection();
        services.AddMinimalMediator(c=>c.UseReflection(typeof(ConsumerTest)));
        services.AddSingleton<SharedService>();
        services.AddTransient<LifeTimeService>();
        var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();

        var reader =
            await mediator.ReceiveChannelStreamAsync<ReceiverMessage, ReceiverResponse>(new ReceiverMessage("test"),
                CancellationToken.None);

        var result = new List<string>();
        while (await reader.WaitToReadAsync())
        {
            reader.TryRead(out var item);
            result.Add(item?.Message ?? "");
        }
        
        Assert.True(result.SequenceEqual(GetExpectedStreamMessages()));
    }

    private static IReadOnlyList<string> GetExpectedStreamMessages()
    {
        return new List<string>
        {
            "0",
            "1",
            "2",
            "3",
            "4",
        };
    }
}