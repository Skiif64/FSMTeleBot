namespace FSMTeleBot.FSM.Abstractions;

public class StateGroupBase
{
    private IReadOnlyList<StateBase> _states;

    public StateGroupBase(List<StateBase> states)
    {        
        _states = states.AsReadOnly();        
    }
}
