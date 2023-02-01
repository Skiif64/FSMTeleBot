using System.Reflection;

namespace FSMTeleBot.FSM;

public abstract class StateGroup : IStateGroup
{
    private int _currentStateIndex = 0;
    private readonly IList<IState> _states;
    public IState this[int index] => _states[index];

    public StateGroup(StateGroup child)
    {
        _states = child
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Static)
            .Where(p => p.PropertyType.IsAssignableTo(typeof(IState)))
            .Select(p => new ChatState(p.Name))
            .OfType<IState>()
            .ToList();  
    }

    internal StateGroup()
    {

    }

    public async Task<IState> Next(FsmContext context, CancellationToken cancellationToken = default)
    {
        if (_currentStateIndex >= _states.Count)
            throw new InvalidOperationException("Out of range");//TODO: Finish state?
        await context.SetStateAsync(_states[++_currentStateIndex], cancellationToken);
        return _states[_currentStateIndex];
    }
}
