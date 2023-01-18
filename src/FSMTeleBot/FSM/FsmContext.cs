using FSMTeleBot.FSM.Abstractions;

namespace FSMTeleBot.FSM;

public class FsmContext
{
    private readonly IStateStorage _stateStorage;
    private StateGroupBase _currentState;
    public StateBase CurrentState { get; private set; } = null!;

    public FsmContext(IStateStorage stateStorage, StateGroupBase currentState)
    {
        _stateStorage = stateStorage;
        _currentState = currentState;
    }

    public void SetState(StateGroupBase group)
    {

    }

    public void Next()
    {

    }

    public void Previous()
    {

    }

    public void Finish()
    {

    }

    public void SetValue(object value)
    {

    }

    public object[] GetValues()
    {
        throw new NotImplementedException();
    }
}
