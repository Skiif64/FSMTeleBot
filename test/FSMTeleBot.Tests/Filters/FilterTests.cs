using FSMTeleBot.Abstractions;
using FSMTeleBot.Filters;
using FSMTeleBot.Handlers.Abstractions;
using Moq;
using System.ComponentModel;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FSMTeleBot.Tests.Filters;

public class FilterTests
{
    private readonly IServiceProvider _serviceProvider;
    
    private readonly Mock<IChatMemberService> _chatMemberServiceMock;    
    private readonly FilterAttribute _filter;
    private readonly Message _message;
    public FilterTests()
    {
        var serviceProviderMock = new Mock<IServiceProvider>(); 
        _chatMemberServiceMock = new Mock<IChatMemberService>();        

        serviceProviderMock
             .Setup(x => x.GetService(typeof(IChatMemberService)))
             .Returns(_chatMemberServiceMock.Object);

        _serviceProvider = serviceProviderMock.Object;

        _filter = new MessageFilterAttribute { Allowed = ChatMemberStatus.Member };

        _message = new Message
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
    }
    

    [Test]
    public void WhenIsMatchInvoked_AllowedIsMember_ThenShouldReturnTrue()
    {
        _chatMemberServiceMock.Reset();
        _chatMemberServiceMock
            .Setup(x => x.GetStatus(It.IsAny<long>(), It.IsAny<long>(), default))
            .Returns(Task.FromResult(ChatMemberStatus.Member));

        var result = _filter.IsMatch(_message, _serviceProvider);

        Assert.IsTrue(result);
        _chatMemberServiceMock.Verify(x => x.GetStatus(It.IsAny<long>(), It.IsAny<long>(), default), Times.Once);
    }

    [Test]
    public void WhenIsMatchInvoked_AllowedIsAdministrator_ThenShouldReturnTrue()
    {
        _chatMemberServiceMock.Reset();
        _chatMemberServiceMock
            .Setup(x => x.GetStatus(It.IsAny<long>(), It.IsAny<long>(), default))
            .Returns(Task.FromResult(ChatMemberStatus.Administrator));

        var result = _filter.IsMatch(_message, _serviceProvider);

        Assert.IsTrue(result);
        _chatMemberServiceMock.Verify(x => x.GetStatus(It.IsAny<long>(), It.IsAny<long>(), default), Times.Once);
    }

    [Test]
    public void WhenIsMatchInvoked_AllowedIsKicked_ThenShouldReturnFalse()
    {
        _chatMemberServiceMock.Reset();
        _chatMemberServiceMock
            .Setup(x => x.GetStatus(It.IsAny<long>(), It.IsAny<long>(), default))
            .Returns(Task.FromResult(ChatMemberStatus.Kicked));

        var result = _filter.IsMatch(_message, _serviceProvider);

        Assert.IsFalse(result);
        _chatMemberServiceMock.Verify(x => x.GetStatus(It.IsAny<long>(), It.IsAny<long>(), default), Times.Once);
    }
}
