using FSMTeleBot.FSM;
using Moq;

namespace FSMTeleBot.Tests.FSM;

public class FsmContextTests
{
    private readonly Mock<IStateStorage> _storageMock;
    private readonly IStateStorage _storage;
    private readonly FsmContext _context;

    public FsmContextTests()
    {
        _storageMock = new Mock<IStateStorage>();
        _storage = _storageMock.Object;
        _context = new FsmContext(1, 1, _storage);
    }

    [SetUp]
    public void SetupStorageMock()
    {
        _storageMock.Reset();         
        _storageMock
            .Setup( x => x.GetOrAddAsync(1, 1, It.IsAny<IState>(), default))
            .Returns(Task.FromResult(It.IsAny<IState>()));
        _storageMock
            .Setup(x => x.UpdateAsync(1, 1, It.IsAny<IState>(), default))
            .Returns(Task.CompletedTask);
    }

    [Test]
    public async Task When_SetStateGroup_ThenStorageShouldBeUpdated()
    {
        var stateGroupMock = new Mock<IStateGroup>();
        stateGroupMock.Setup(x => x[0])
            .Returns(It.IsAny<IState>());
        await _context.SetStateGroupAsync(stateGroupMock.Object, default);
        _storageMock.Verify(x => x.UpdateAsync(1, 1, It.IsAny<IState>(), default), Times.Once);
    }
}
