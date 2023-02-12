using FSMTeleBot.ChatState.Abstractions;
using Moq;

namespace FSMTeleBot.Tests.FSM;

public class TestStateGroupTests
{    
    private readonly IChatStateGroup _stateGroup;

    public TestStateGroupTests()
    {       
        _stateGroup = new FakeStateGroup();
    }

    [Test]
    public void WhenCreated_Then3StatesShouldContain()
    {
        Assert.IsNotNull(_stateGroup.States);
        Assert.That(_stateGroup.States.Count, Is.EqualTo(3));
        CollectionAssert.AllItemsAreInstancesOfType(_stateGroup.States, typeof(IChatState));        
    }

    [Test]
    public void WhenCreated_Then3StateShouldBeInitialized()
    {
        Assert.IsNotNull(FakeStateGroup.State1);
        Assert.IsNotNull(FakeStateGroup.State2);
        Assert.IsNotNull(FakeStateGroup.State3);
    }
    [Test]
    public async Task WhenNext_Then2StateShouldBe()
    {
        var contextMock = new Mock<IChatContext>();
        contextMock.Setup(x => x.SetStateAsync(It.IsAny<IChatState>(), default))
            .Returns(Task.CompletedTask);

        var result = await _stateGroup.Next(contextMock.Object, default);
        Assert.IsNotNull(result);
        Assert.That(result == _stateGroup[1]);
        contextMock.Verify(x => x.SetStateAsync(It.IsAny<IChatState>(), default), Times.Once);
    }
}
