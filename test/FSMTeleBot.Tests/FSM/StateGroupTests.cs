﻿using FSMTeleBot.FSM;
using Moq;

namespace FSMTeleBot.Tests.FSM;

public class StateGroupTests
{    
    class TestStateGroup : StateGroup
    {
        public static IState State1 { get; }
        public static IState State2 { get; }
        public static IState State3 { get; }
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
    public async Task WhenNext_Then2StateShouldBe()
    {
        var contextMock = new Mock<IFsmContext>();
        contextMock.Setup(x => x.SetStateAsync(It.IsAny<IState>(), default))
            .Returns(Task.CompletedTask);

        var result = await _stateGroup.Next(contextMock.Object, default);
        Assert.IsNotNull(result);
        Assert.That(result == _stateGroup[1]);
    }
}