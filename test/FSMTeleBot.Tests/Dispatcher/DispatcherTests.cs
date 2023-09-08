using FSMTeleBot.Filters;
using FSMTeleBot.Handlers.Abstractions;
using FSMTeleBot.Internal.Dispatcher;
using Moq;
using System.ComponentModel;
using Telegram.Bot.Types;

namespace FSMTeleBot.Tests.Dispatcher;

public class DispatcherTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IBotDispatcher _dispatcher;
    private readonly Mock<IHandler<Message, IHandlerContext<Message>>> _startHandlerMock;
    private readonly Mock<IHandler<Message, IHandlerContext<Message>>> _cancelHandlerMock;
    private readonly Mock<IHandler<Message, IHandlerContext<Message>>> _emptyHandlerMock;

    private readonly Mock<IHandlerContextFactory<Message>> _contextFactoryMock;

    public DispatcherTests()
    {
        _startHandlerMock = new Mock<IHandler<Message, IHandlerContext<Message>>>();
        TypeDescriptor.AddAttributes(_startHandlerMock.Object, new MessageFilterAttribute { Contains = "start" });

        _cancelHandlerMock = new Mock<IHandler<Message, IHandlerContext<Message>>>();
        TypeDescriptor.AddAttributes(_cancelHandlerMock.Object, new MessageFilterAttribute { Contains = "cancel" });

        _emptyHandlerMock = new Mock<IHandler<Message, IHandlerContext<Message>>>();
        TypeDescriptor.AddAttributes(_emptyHandlerMock.Object, new MessageFilterAttribute());

        _contextFactoryMock = new Mock<IHandlerContextFactory<Message>>();
        
        _serviceProvider = new ServiceProviderBuilder()
            .Add(new[] 
            { 
                _startHandlerMock.Object,
                _cancelHandlerMock.Object,
                _emptyHandlerMock.Object 
            },
            typeof(IEnumerable<IHandler<Message, IHandlerContext<Message>>>))
            .Add(_contextFactoryMock.Object)
            .ServiceProvider;

        _dispatcher = new BotDispatcher(_serviceProvider);
    }

    [SetUp]
    public void Setup()
    {
        _startHandlerMock.Reset();
        _startHandlerMock
            .Setup(x => x.HandleAsync(It.IsAny<IHandlerContext<Message>>(), default))
            .Returns(Task.CompletedTask);
        _cancelHandlerMock.Reset();
        _cancelHandlerMock
            .Setup(x => x.HandleAsync(It.IsAny<IHandlerContext<Message>>(), default))
            .Returns(Task.CompletedTask);
        _emptyHandlerMock.Reset();
        _emptyHandlerMock
           .Setup(x => x.HandleAsync(It.IsAny<IHandlerContext<Message>>(), default))
           .Returns(Task.CompletedTask);
        _contextFactoryMock.Reset();
        _contextFactoryMock.Setup(x => x.CreateAsync(It.IsAny<Message>(), default))
            .Returns(Task.FromResult(new Mock<IHandlerContext<Message>>().Object));
    }

    [Test]
    public void WhenSend_StartMessage_ThenHandleStartHandler()
    {        
        var message = MessageBuilder.Create()
            .WithText("start")
            .Message;

        Assert.DoesNotThrowAsync(async () => await _dispatcher.SendAsync(message));
        _startHandlerMock.Verify(x => x.HandleAsync(It.IsAny<IHandlerContext<Message>>(), default), Times.Once);
        _cancelHandlerMock.Verify(x => x.HandleAsync(It.IsAny<IHandlerContext<Message>>(), default), Times.Never);
        _emptyHandlerMock.Verify(x => x.HandleAsync(It.IsAny<IHandlerContext<Message>>(), default), Times.Never);
    }

    [Test]
    public void WhenSend_CancelMessage_ThenHandleCancelHandler()
    {
        var message = MessageBuilder.Create()
            .WithText("cancel")
            .Message;

        Assert.DoesNotThrowAsync(async () => await _dispatcher.SendAsync(message));
        _startHandlerMock.Verify(x => x.HandleAsync(It.IsAny<IHandlerContext<Message>>(), default), Times.Never);
        _cancelHandlerMock.Verify(x => x.HandleAsync(It.IsAny<IHandlerContext<Message>>(), default), Times.Once);
        _emptyHandlerMock.Verify(x => x.HandleAsync(It.IsAny<IHandlerContext<Message>>(), default), Times.Never);
    }
    [Test]
    public void WhenSend_NoneMessage_ThenHandleEmptyHandler()
    {        
        var message = MessageBuilder.Create()
            .WithText("none")
            .Message;

        Assert.DoesNotThrowAsync(async () => await _dispatcher.SendAsync(message));
        _startHandlerMock.Verify(x => x.HandleAsync(It.IsAny<IHandlerContext<Message>>(), default), Times.Never);
        _cancelHandlerMock.Verify(x => x.HandleAsync(It.IsAny<IHandlerContext<Message>>(), default), Times.Never);
        _emptyHandlerMock.Verify(x => x.HandleAsync(It.IsAny<IHandlerContext<Message>>(), default), Times.Once);
    }
}
