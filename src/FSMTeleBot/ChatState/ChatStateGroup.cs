using FSMTeleBot.ChatState.Abstractions;
using FSMTeleBot.Internal;
using System.Reflection;

namespace FSMTeleBot.ChatState;

public abstract class ChatStateGroup : IChatStateGroup
{
    private int _currentStateIndex = 0;
    public IReadOnlyList<IChatState> States { get; private set; }
    public IChatState this[int index] => States[index];

    public ChatStateGroup()
    {

    }

    internal void InitState(ChatStateGroup child)
    {
        var childType = child.GetType();
        var properties = childType           
           .GetProperties(BindingFlags.Public | BindingFlags.Static)
           .Where(p => p.PropertyType.IsAssignableTo(typeof(IChatState)))
           .ForEach(p => p.SetValue(child, new ChatState(childType.Name + "/" + p.Name)));
        States = properties
            .Select(p => p.GetValue(child))
            .OfType<IChatState>()
           .ToList();
    }

    public async Task<IChatState> Next(IChatContext context, CancellationToken cancellationToken = default)
    {
        if (_currentStateIndex >= States.Count)
            throw new InvalidOperationException("Out of range");//TODO: Finish state?
        await context.SetStateAsync(States[++_currentStateIndex], cancellationToken);
        return States[_currentStateIndex];
    }
}
