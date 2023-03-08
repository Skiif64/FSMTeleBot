using FSMTeleBot.Abstractions;
using FSMTeleBot.States;
using FSMTeleBot.States.Abstractions;
using Moq;

namespace FSMTeleBot.Tests.FSM;

public class FsmContextTests
{
    private readonly Mock<IChatStateStorage> _storageMock;
    private readonly IChatStateStorage _storage;    

    public FsmContextTests()
    {
        _storageMock = new Mock<IChatStateStorage>();
        _storage = _storageMock.Object;        
    }

    [SetUp]
    public void SetupStorageMock()
    {
        _storageMock.Reset();         
        _storageMock
            .Setup( x => x.GetOrInit(1, 1, default))
            .Returns(Task.FromResult(It.IsAny<IChatState>()));
        _storageMock
            .Setup(x => x.UpdateAsync(1, 1, It.IsAny<IChatState>(), default))
            .Returns(Task.CompletedTask);
    }

    [Test]
    public async Task WhenSetState_ThenStorageShouldBeUpdated()
    {
        var context = new ChatContext(1, 1, _storage, It.IsAny<IChatState>());
        var stateGroupMock = new Mock<IChatState>();        
        await context.SetStateAsync(stateGroupMock.Object, default);
        _storageMock.Verify(x => x.UpdateAsync(1, 1, It.IsAny<IChatState>(), default), Times.Once);
    }
}
