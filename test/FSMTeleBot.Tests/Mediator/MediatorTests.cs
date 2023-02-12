using FSMTeleBot.Filters;
using FSMTeleBot.Handlers.Abstractions;
using FSMTeleBot.Internal.Mediator;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Collections.Generic;
using System.ComponentModel;
using Telegram.Bot.Types;

namespace FSMTeleBot.Tests.Mediator;

public class MediatorTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IBotMediator _mediator;
    private readonly Mock<IHandler<Message>> _startHandlerMock;
    private readonly Mock<IHandler<Message>> _cancelHandlerMock;

    public MediatorTests()
    {
        _startHandlerMock = new Mock<IHandler<Message>>();
        TypeDescriptor.AddAttributes(_startHandlerMock.Object, new MessageFilterAttribute { Contains = "start" });

        _cancelHandlerMock = new Mock<IHandler<Message>>();
        TypeDescriptor.AddAttributes(_cancelHandlerMock.Object, new MessageFilterAttribute { Contains = "cancel" });

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock
            .Setup(x => x.GetService(typeof(IEnumerable<IHandler<Message>>)))
        .Returns(new[] { _startHandlerMock.Object, _cancelHandlerMock.Object });

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
    }    

    [Test]
    public void When_SendStartMessage_Then_HandleCorrectHandler()
    {
        var message = new Message
        {
            Text = "start"
        };

        Assert.DoesNotThrowAsync(async () => await _mediator.Send(message));
        _startHandlerMock.Verify(x => x.HandleAsync(It.IsAny<Message>(), default), Times.Once);
        _cancelHandlerMock.Verify(x => x.HandleAsync(It.IsAny<Message>(), default), Times.Never);
    }

    [Test]
    public void When_SendNoneMessage_Then_DoNothing()
    {
        var message = new Message
        {
            Text = "none"
        };

        Assert.DoesNotThrowAsync(async () => await _mediator.Send(message));
        _startHandlerMock.Verify(x => x.HandleAsync(It.IsAny<Message>(), default), Times.Never);
        _cancelHandlerMock.Verify(x => x.HandleAsync(It.IsAny<Message>(), default), Times.Never);
    }
}
