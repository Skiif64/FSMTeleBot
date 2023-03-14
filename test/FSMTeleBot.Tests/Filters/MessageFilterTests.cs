using FSMTeleBot.Filters;
using FSMTeleBot.Handlers.Abstractions;
using FSMTeleBot.States;
using FSMTeleBot.States.Abstractions;
using FSMTeleBot.Tests.FSM;
using Moq;
using Telegram.Bot.Types;

namespace FSMTeleBot.Tests.Filters;
public class MessageFilterTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IChatState _chatState1;
    private readonly IChatState _chatState2;
    private readonly Mock<IChatContextFactory<Message>> _factoryMock;
    private readonly Mock<IChatContext> _chatContextMock;

    private readonly MessageFilterAttribute _filterWithContains;
    private readonly MessageFilterAttribute _filterWithContainsCommand;
    private readonly MessageFilterAttribute _filterWithRegexp;
    private readonly MessageFilterAttribute _filterWithChatStateRequired;
    private readonly MessageFilterAttribute _filterWithoutArgs;
    public MessageFilterTests()
    {
        _factoryMock = new Mock<IChatContextFactory<Message>>();
        _chatContextMock = new Mock<IChatContext>();
        _chatState1 = new ChatState("Fake1");
        _chatState2 = new ChatState("Fake2");
        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock
            .Setup(x => x.GetService(typeof(IChatContext)))
            .Returns(_chatContextMock.Object);
        serviceProviderMock
            .Setup(x => x.GetService(typeof(IChatContextFactory<Message>)))
            .Returns(_factoryMock.Object);
        _factoryMock.Setup(x => x.GetContextAsync(It.IsAny<Message>(), default))
            .Returns(Task.FromResult(_chatContextMock.Object));
            
        _serviceProvider = serviceProviderMock.Object;
        
        _filterWithContains = new MessageFilterAttribute { Contains = "message" };
        _filterWithContainsCommand = new MessageFilterAttribute { ContainsCommand = "start" };
        _filterWithRegexp = new MessageFilterAttribute { Regexp = @"^[0-9]+" };
        _filterWithChatStateRequired = new MessageFilterAttribute { RequiredState = _chatState1.Name };
        _filterWithoutArgs = new MessageFilterAttribute();
    }

    [SetUp]
    public void Setup()
    {

    }

    public void WhenFilterWithoutArgsIsMatch_TextMessage_ThenShouldReturnTrue()
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

        var result = _filterWithoutArgs.IsMatch(message, _serviceProvider);

        Assert.IsTrue(result);
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
