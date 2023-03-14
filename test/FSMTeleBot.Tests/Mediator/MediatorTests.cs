using FSMTeleBot.Filters;
using FSMTeleBot.Handlers.Abstractions;
using FSMTeleBot.Internal.Mediator;
using Moq;
using System.ComponentModel;
using Telegram.Bot.Types;

namespace FSMTeleBot.Tests.Mediator;

public class MediatorTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IBotMediator _mediator;
    private readonly Mock<IHandler<Message>> _startHandlerMock;
    private readonly Mock<IHandler<Message>> _cancelHandlerMock;
    private readonly Mock<IHandler<Message>> _emptyHandlerMock;
    
    public MediatorTests()
    {
        _startHandlerMock = new Mock<IHandler<Message>>();
        TypeDescriptor.AddAttributes(_startHandlerMock.Object, new MessageFilterAttribute { Contains = "start" });

        _cancelHandlerMock = new Mock<IHandler<Message>>();
        TypeDescriptor.AddAttributes(_cancelHandlerMock.Object, new MessageFilterAttribute { Contains = "cancel" });

        _emptyHandlerMock = new Mock<IHandler<Message>>();
        TypeDescriptor.AddAttributes(_emptyHandlerMock.Object, new MessageFilterAttribute());

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock
            .Setup(x => x.GetService(typeof(IEnumerable<IHandler<Message>>)))
        .Returns(new[] { _startHandlerMock.Object, _cancelHandlerMock.Object, _emptyHandlerMock.Object });

        _serviceProvider = serviceProviderMock.Object;
        _mediator = new BotMediator(_serviceProvider);
    }

    [SetUp]
    public void Setup()
    {
        _startHandlerMock.Reset();
        _startHandlerMock
            .Setup(x => x.HandleAsync(It.IsAny<Message>(), default))
            .Returns(Task.CompletedTask);
        _cancelHandlerMock.Reset();
        _cancelHandlerMock
            .Setup(x => x.HandleAsync(It.IsAny<Message>(), default))
            .Returns(Task.CompletedTask);
        _emptyHandlerMock.Reset();
        _emptyHandlerMock
           .Setup(x => x.HandleAsync(It.IsAny<Message>(), default))
           .Returns(Task.CompletedTask);
    }    

    [Test]
    public void WhenSend_StartMessage_ThenHandleStartHandler()
    {
        var message = new Message
        {
            Text = "start",
            Chat = new Chat
            {
                Id = 1
            },
            From = new User
            {
                Id = 1
            }
        };

        Assert.DoesNotThrowAsync(async () => await _mediator.SendAsync(message));
        _startHandlerMock.Verify(x => x.HandleAsync(It.IsAny<Message>(), default), Times.Once);
        _cancelHandlerMock.Verify(x => x.HandleAsync(It.IsAny<Message>(), default), Times.Never);
        _emptyHandlerMock.Verify(x => x.HandleAsync(It.IsAny<Message>(), default), Times.Never);
    }

    [Test]
    public void WhenSend_CancelMessage_ThenHandleCancelHandler()
    {
        var message = new Message
        {
            Text = "cancel",
            Chat = new Chat
            {
                Id = 1
            },
            From = new User
            {
                Id = 1
            }
        };

        Assert.DoesNotThrowAsync(async () => await _mediator.SendAsync(message));
        _startHandlerMock.Verify(x => x.HandleAsync(It.IsAny<Message>(), default), Times.Never);
        _cancelHandlerMock.Verify(x => x.HandleAsync(It.IsAny<Message>(), default), Times.Once);
        _emptyHandlerMock.Verify(x => x.HandleAsync(It.IsAny<Message>(), default), Times.Never);
    }
    [Test]
    public void WhenSend_NoneMessage_ThenHandleEmptyHandler()
    {
        var message = new Message
        {
            Text = "none",
            Chat = new Chat
            {
                Id = 1
            },
            From = new User
            {
                Id = 1
            }
        };

        Assert.DoesNotThrowAsync(async () => await _mediator.SendAsync(message));
        _startHandlerMock.Verify(x => x.HandleAsync(It.IsAny<Message>(), default), Times.Never);
        _cancelHandlerMock.Verify(x => x.HandleAsync(It.IsAny<Message>(), default), Times.Never);
        _emptyHandlerMock.Verify(x => x.HandleAsync(It.IsAny<Message>(), default), Times.Once);
    }
}
