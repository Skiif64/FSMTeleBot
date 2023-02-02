using FSMTeleBot.FSM;
using Moq;

namespace FSMTeleBot.Tests.FSM;

public class StateGroupTests
{    
    class TestStateGroup : StateGroup
    {
        public static IState State1 { get; set; }
        public static IState State2 { get; set; }
        public static IState State3 { get; set; }
        public TestStateGroup()
        {
            InitState(this);
        }
    }
    private readonly IStateGroup _stateGroup;

    public StateGroupTests()
    {       
        _stateGroup = new TestStateGroup();
    }

    [Test]
    public void WhenCreated_Then3StatesShouldContain()
    {
        Assert.IsNotNull(_stateGroup.States);
        Assert.That(_stateGroup.States.Count, Is.EqualTo(3));
        CollectionAssert.AllItemsAreInstancesOfType(_stateGroup.States, typeof(IState));        
    }

    [Test]
    public void WhenCreated_Then3StateShouldBeInitialized()
    {
        Assert.IsNotNull(TestStateGroup.State1);
        Assert.IsNotNull(TestStateGroup.State2);
        Assert.IsNotNull(TestStateGroup.State3);
    }
    [Test]
    public async Task WhenNext_Then2StateShouldBe()
    {
        var contextMock = new Mock<IFsmContext>();
        contextMock.Setup(x => x.SetStateAsync(It.IsAny<IState>(), default))
            .Returns(Task.CompletedTask);

        var result = await _stateGroup.Next(contextMock.Object, default);
        Assert.IsNotNull(result);
        Assert.That(result == _stateGroup[1]);
        contextMock.Verify(x => x.SetStateAsync(It.IsAny<IState>(), default), Times.Once);
    }
}
