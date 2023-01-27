using FSMTeleBot.Handlers.Abstractions;
using FSMTeleBot.Internal.Mediator;
using FSMTeleBot.Tests.Fakes;
using FSMTeleBot.Tests.TestHandlers;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Telegram.Bot.Types;

namespace FSMTeleBot.Tests.Mediator;

public class MediatorTests
{    
    private readonly IServiceProvider _serviceProvider;
    private readonly IBotMediator _mediator;

    public MediatorTests()
    {       
        _serviceProvider = SetupServiceProvider();
        _mediator = new BotMediator(_serviceProvider);
    }

    private IServiceProvider SetupServiceProvider()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<IHandler<Message>, StartMessageHandler>();
        serviceCollection.AddTransient<IHandler<Message>, CancelMessageHandler>();
        return serviceCollection.BuildServiceProvider();
    }

    [TestCase("start")]
    [TestCase("cancel")]
    public void WhenMessage_ThenHandleCorrectHandler(string text)
    {
        var message = new Message
        {
            Text = text
        };

        Assert.DoesNotThrowAsync(async () => await _mediator.Send(message));        
    }
}
