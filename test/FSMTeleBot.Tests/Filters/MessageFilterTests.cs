using FSMTeleBot.ChatState.Abstractions;
using FSMTeleBot.Filters;
using FSMTeleBot.Handlers.Abstractions;
using FSMTeleBot.Tests.FSM;
using Moq;
using Telegram.Bot.Types;

namespace FSMTeleBot.Tests.Filters;
public class FakeState1 : IChatState
{
    public string Name => "Fake1";
}

public class FakeState2 : IChatState
{
    public string Name => "Fake2";
}

public class MessageFilterTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IChatState _chatState1;
    private readonly IChatState _chatState2;
    private readonly Mock<IChatContext> _chatContextMock;

    private readonly MessageFilterAttribute _filterWithContains;
    private readonly MessageFilterAttribute _filterWithContainsCommand;
    private readonly MessageFilterAttribute _filterWithRegexp;
    private readonly MessageFilterAttribute _filterWithChatStateRequired;
    public MessageFilterTests()
    {
        _chatContextMock = new Mock<IChatContext>();
        _chatState1 = new FakeState1();
        _chatState2 = new FakeState2();
        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock
            .Setup(x => x.GetService(typeof(IChatContext)))
            .Returns(_chatContextMock.Object);
        _serviceProvider = serviceProviderMock.Object;
        
        _filterWithContains = new MessageFilterAttribute { Contains = "message" };
        _filterWithContainsCommand = new MessageFilterAttribute { ContainsCommand = "start" };
        _filterWithRegexp = new MessageFilterAttribute { Regexp = @"^[0-9]+" };
        _filterWithChatStateRequired = new MessageFilterAttribute { RequiredState = _chatState1 };
    }

    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public void WhenFilterWithContainsIsMatch_OnlyMessageText_ThenShouldReturnTrue()
    {
        var message = new Message
        {
            Text = "message",
            From = new User
            {
                Id = 1
            },
            Chat = new Chat
            {
                Id = 1
            }
        };

        var result = _filterWithContains.IsMatch(message, _serviceProvider);

        Assert.IsTrue(result);
        
    }

    [Test]
    public void WhenFilterWithContainsIsMatch_InvalidMessageText_ThenShouldReturnFalse()
    {
        var message = new Message
        {
            Text = "messsssage",
            From = new User
            {
                Id = 1
            },
            Chat = new Chat
            {
                Id = 1
            }
        };

        var result = _filterWithContains.IsMatch(message, _serviceProvider);

        Assert.That(result, Is.False);

    }

    [Test]
    public void WhenFilterWithContainsIsMatch_ContainsMessageText_ThenShouldReturnTrue()
    {
        var message = new Message
        {
            Text = "test-test message test-test-test",
            From = new User
            {
                Id = 1
            },
            Chat = new Chat
            {
                Id = 1
            }
        };

        var result = _filterWithContains.IsMatch(message, _serviceProvider);

        Assert.IsTrue(result);

    }

    [Test]
    public void WhenFilterWithContainsCommandIsMatch_OnlyStartCommand_ThenShouldReturnTrue()
    {
        var message = new Message
        {
            Text = "/start",
            From = new User
            {
                Id = 1
            },
            Chat = new Chat
            {
                Id = 1
            }
        };

        var result = _filterWithContainsCommand.IsMatch(message, _serviceProvider);

        Assert.IsTrue(result);

    }

    [Test]
    public void WhenFilterWithContainsCommandIsMatch_NotContainsCommand_ThenShouldReturnFalse()
    {
        var message = new Message
        {
            Text = "start",
            From = new User
            {
                Id = 1
            },
            Chat = new Chat
            {
                Id = 1
            }
        };

        var result = _filterWithContainsCommand.IsMatch(message, _serviceProvider);

        Assert.That(result, Is.False);

    }

    [Test]
    public void WhenFilterWithContainsCommandIsMatch_StartsStartCommand_ThenShouldReturnTrue()
    {
        var message = new Message
        {
            Text = "/start command arguments",
            From = new User
            {
                Id = 1
            },
            Chat = new Chat
            {
                Id = 1
            }
        };

        var result = _filterWithContainsCommand.IsMatch(message, _serviceProvider);

        Assert.IsTrue(result);

    }

    [Test]
    public void WhenFilterWithRegexpIsMatch_MessageWithValidRegex_ThenShouldReturnTrue()
    {
        var message = new Message
        {
            Text = "12345678901010 test-test",
            From = new User
            {
                Id = 1
            },
            Chat = new Chat
            {
                Id = 1
            }
        };

        var result = _filterWithRegexp.IsMatch(message, _serviceProvider);

        Assert.IsTrue(result);

    }

    [Test]
    public void WhenFilterWithRequiredStateIsMatch_RequiredState_ThenShouldReturnTrue()
    {
        var message = new Message
        {
            Text = "none",
            From = new User
            {
                Id = 1
            },
            Chat = new Chat
            {
                Id = 1
            }
        };

        _chatContextMock.Reset();
        _chatContextMock
            .SetupGet(x => x.CurrentState)
            .Returns(_chatState1);

        var result = _filterWithChatStateRequired.IsMatch(message, _serviceProvider);

        Assert.IsTrue(result);
        _chatContextMock.VerifyGet(x => x.CurrentState, Times.Once);
    }

    [Test]
    public void WhenFilterWithRequiredStateIsMatch_NotInRequiredState_ThenShouldReturnFalse()
    {
        var message = new Message
        {
            Text = "none",
            From = new User
            {
                Id = 1
            },
            Chat = new Chat
            {
                Id = 1
            }
        };

        _chatContextMock.Reset();
        _chatContextMock
            .SetupGet(x => x.CurrentState)
            .Returns(_chatState2);

        var result = _filterWithChatStateRequired.IsMatch(message, _serviceProvider);

        Assert.That(result, Is.False);
        _chatContextMock.VerifyGet(x => x.CurrentState, Times.Once);
    }
}
