using FSMTeleBot.ChatState.Abstractions;

namespace FSMTeleBot.ChatState;

public class ChatState : IChatState
{
    public string Name { get; }
    public ChatState(string name)
    {
        Name = name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not ChatState other)
            return false;

        return Name.Equals(other.Name);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}
