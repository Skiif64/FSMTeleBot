using FSMTeleBot.FSM;
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
}
