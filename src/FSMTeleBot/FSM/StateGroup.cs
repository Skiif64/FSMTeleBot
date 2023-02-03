using System.Collections.Immutable;
using System.Reflection;

namespace FSMTeleBot.FSM;

public abstract class StateGroup : IStateGroup
{
    private int _currentStateIndex = 0;
    public ImmutableArray<IState> States { get; private set; }
    public IState this[int index] => States[index];

    public StateGroup()
    {
       
    }

    protected void InitState(StateGroup child)
    {
        var childType = child.GetType();
        States = childType           
           .GetProperties(BindingFlags.Public | BindingFlags.Static)
           .Where(p => p.PropertyType.IsAssignableTo(typeof(IState)))
           .Select(p => new ChatState(childType.Name +"/"+ p.Name))
           .OfType<IState>()
           .ToImmutableArray();
    }

    public async Task<IState> Next(IFsmContext context, CancellationToken cancellationToken = default)
    {
        if (_currentStateIndex >= States.Length)
            throw new InvalidOperationException("Out of range");//TODO: Finish state?
        await context.SetStateAsync(States[++_currentStateIndex], cancellationToken);
        return States[_currentStateIndex];
    }
}
