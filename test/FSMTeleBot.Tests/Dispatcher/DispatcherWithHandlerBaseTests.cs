using FSMTeleBot.Filters;
using FSMTeleBot.Handlers.Abstractions;
using FSMTeleBot.Internal.Dispatcher;
using FSMTeleBot.States.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace FSMTeleBot.Tests.Dispatcher;
internal class DispatcherWithHandlerBaseTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IBotDispatcher _dispatcher;
    private readonly Mock<HandlerBase<Message>> _handlerMock;    

    public DispatcherWithHandlerBaseTests()
    {
        _handlerMock = new Mock<HandlerBase<Message>>(new object[] {null!});
        TypeDescriptor.AddAttributes(_handlerMock.Object, new MessageFilterAttribute());

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock
            .Setup(x => x.GetService(typeof(IEnumerable<IHandler<Message>>)))
        .Returns(new[] { _handlerMock.Object });

        var descriptorMock = new Mock<IUpdateDescriptor>();
        descriptorMock.Setup(x => x.GetUserId(It.IsAny<Message>()))
            .Returns(1);
        descriptorMock.Setup(x => x.GetChatId(It.IsAny<Message>()))
            .Returns(1);
        serviceProviderMock.Setup(x => x.GetService(typeof(IEnumerable<IUpdateDescriptor>)))
            .Returns(new[] { descriptorMock.Object });

        var contextMock = new Mock<IChatContext>();
        var contextFactoryMock = new Mock<IChatContextFactory>();
        contextFactoryMock.Setup(x => x.GetContextAsync(It.IsAny<Message>(), default))
            .Returns(Task.FromResult(contextMock.Object));
        serviceProviderMock.Setup(x => x.GetService(typeof(IChatContextFactory)))
            .Returns(contextFactoryMock.Object);

        _serviceProvider = serviceProviderMock.Object;
        _dispatcher = new BotDispatcher(_serviceProvider);
    }

    [SetUp]
    public void Setup()
    {
        _handlerMock.Reset();
        _handlerMock
            .Setup(x => x.HandleAsync(It.IsAny<Message>(), default))
            .Returns(Task.CompletedTask);        
    }

    [Test]
    public void WhenSend_ThenHandlerShouldBeInvoked()
    {
        var message = new Message
        {
            Text = "",
            Chat = new Chat
            {
                Id = 1
            },
            From = new User
            {
                Id = 1
            }
        };

        Assert.DoesNotThrowAsync(async () => await _dispatcher.SendAsync(message));
        _handlerMock.Verify(x => x.HandleAsync(It.IsAny<Message>(), default), Times.Once);        
    }
}
