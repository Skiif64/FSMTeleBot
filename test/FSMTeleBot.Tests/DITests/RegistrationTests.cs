using FSMTeleBot.ChatState;
using FSMTeleBot.ChatState.Abstractions;
using FSMTeleBot.Handlers.Abstractions;
using FSMTeleBot.Internal.DependencyInjectionExtensions;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;

namespace FSMTeleBot.Tests.DITests;

public class RegistrationTests
{
    public class TestMessageHandler : IHandler<Message>
    {
        public Task HandleAsync(Message data, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }

    public class TestMessageHandler2 : IHandler<Message>
    {
        public Task HandleAsync(Message data, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }

    public class TestStateGroup1 : ChatStateGroup 
    {
        public static IChatState State1 { get; set; } = null!;
        public static IChatState State2 { get; set; } = null!;
    }
    public class TestStateGroup2 : ChatStateGroup
    {
        public static IChatState State1 { get; set; } = null!;
        public static IChatState State2 { get; set; } = null!;
    }

    public RegistrationTests()
    {


    }

    [Test]
    public void WhenHandlersRegistered_ThenCollectionShouldContainHandlers()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddHandlers(typeof(RegistrationTests).Assembly);

        CollectionAssert.IsNotEmpty(serviceCollection);

        Assert.That(serviceCollection[0].ServiceType, Is.EqualTo(typeof(IHandler<Message>)));
        Assert.That(serviceCollection[0].ImplementationType, Is.EqualTo(typeof(TestMessageHandler)));
        Assert.That(serviceCollection[1].ServiceType, Is.EqualTo(typeof(IHandler<Message>)));
        Assert.That(serviceCollection[1].ImplementationType, Is.EqualTo(typeof(TestMessageHandler2)));
    }

    [Test]
    public void WhenStateGroupInitialized_ThenStatesShouldBeInitialized()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddStateGroups(typeof(RegistrationTests).Assembly);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var states = serviceProvider.GetServices<IChatStateGroup>();

        CollectionAssert.IsNotEmpty(states);
        CollectionAssert.IsNotEmpty(states.First().States);
        CollectionAssert.AllItemsAreNotNull(states.First().States);        
    }

}
