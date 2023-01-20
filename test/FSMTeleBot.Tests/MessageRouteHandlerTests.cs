using FSMTeleBot.Handlers.Abstractions;
using FSMTeleBot.RouteHandlers;
using FSMTeleBot.Tests.TestHandlers;
using Moq;
using Telegram.Bot.Types;

namespace FSMTeleBot.Tests
{
    public class MessageRouteHandlerTests
    {         
        [SetUp]
        public void Setup()
        {

            
        }

        [Test]
        public async Task Route_MockHandler_ShouldBe_Invoked()
        {
            var message = new Message
            {
                Chat = new Chat
                {
                    Id = 12345
                },
                Text = "/start"
            };
            var handlerMock = new Mock<IHandler<Message>>();
            handlerMock.Setup(x => x.HandleAsync(It.Is<Message>(x => x == message), default))            
               .Returns(Task.CompletedTask);
                
           var mock = new Mock<IHandlersStorage>();
            mock.Setup(x => x.GetHandler<Message>(It.IsAny<Message>()))
                .Returns(handlerMock.Object);
                
            var messageRouteHandler = new MessageRouteHandler(mock.Object);

            await messageRouteHandler.RouteAsync(message);

            mock.Verify(x => x.GetHandler<Message>(It.IsAny<Message>()), Times.Once());
            handlerMock.Verify(x => x.HandleAsync(It.Is<Message>(x => x == message), default), Times.Once);
        }

        [Test]
        public async Task Route_StartHandler_ShouldBe_Invoked()
        {
            var message = new Message
            {
                Chat = new Chat
                {
                    Id = 12345
                },
                Text = "/start"
            };
            
            var mock = new Mock<IHandlersStorage>();
            mock.Setup(x => x.GetHandler(It.Is<Message>(x => x.Text == "/start")))
                .Returns(new StartMessageHandler());

            var messageRouteHandler = new MessageRouteHandler(mock.Object);

            await messageRouteHandler.RouteAsync(message);

            mock.Verify(x => x.GetHandler<Message>(It.IsAny<Message>()), Times.Once());            
        }
    }
}