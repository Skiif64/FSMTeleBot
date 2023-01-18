namespace FSMTeleBot.FSM.Abstractions;

public class StateGroupBase
{
    private IReadOnlyList<StateBase> _states;
    private int _index;
    public StateBase CurrentState { get; private set; }

    public StateGroupBase(List<StateBase> states)
    {
        _states = states.AsReadOnly();
        CurrentState = _states[_index];
    }   

    public StateBase Next()
    {
        return _states[++_index];
    }

    public StateBase Previous()
    {
        return _states[--_index];
    }    
}
