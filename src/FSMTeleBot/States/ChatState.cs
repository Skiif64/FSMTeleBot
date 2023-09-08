using FSMTeleBot.States.Abstractions;

namespace FSMTeleBot.States;

public readonly struct ChatState : IChatState
{
    public string Name { get; }
    public ChatState()
    {
        Name = string.Empty;
    }
    public ChatState(string name)
    {
        Name = name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not ChatState other)
        {
            if (obj is not string name)
                return false;

            return Name.Equals(name);
        }

        return Name.Equals(other.Name);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    public override string ToString()
    {
        return Name;
    }

    public static bool operator ==(ChatState left, ChatState right) => left.Equals(right);
    public static bool operator !=(ChatState left, ChatState right) => !left.Equals(right);
}
