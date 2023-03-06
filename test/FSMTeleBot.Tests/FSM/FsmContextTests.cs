using FSMTeleBot.Abstractions;
using FSMTeleBot.ChatState;
using FSMTeleBot.ChatState.Abstractions;
using Moq;

namespace FSMTeleBot.Tests.FSM;

public class FsmContextTests
{
    private readonly Mock<IChatStateStorage> _storageMock;
    private readonly IChatStateStorage _storage;
    private readonly ChatContext _context;

    public FsmContextTests()
    {
        _storageMock = new Mock<IChatStateStorage>();
        _storage = _storageMock.Object;
        _context = new ChatContext(1, 1, _storage);
    }

    [SetUp]
    public void SetupStorageMock()
    {
        _storageMock.Reset();         
        _storageMock
            .Setup( x => x.GetOrAddAsync(1, 1, It.IsAny<IChatState>(), default))
            .Returns(Task.FromResult(It.IsAny<IChatState>()));
        _storageMock
            .Setup(x => x.UpdateAsync(1, 1, It.IsAny<IChatState>(), default))
            .Returns(Task.CompletedTask);
    }

    [Test]
    public async Task When_SetState_ThenStorageShouldBeUpdated()
    {
        var stateGroupMock = new Mock<IChatState>();        
        await _context.SetStateAsync(stateGroupMock.Object, default);
        _storageMock.Verify(x => x.UpdateAsync(1, 1, It.IsAny<IChatState>(), default), Times.Once);
    }
}
