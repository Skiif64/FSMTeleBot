namespace FSMTeleBot.FSM.Abstractions;

public class StateBase
{
    public string Prefix { get; }
    public object? Value { get; set; }

    public StateBase(string prefix)
    {
        Prefix = prefix;
    }
}
