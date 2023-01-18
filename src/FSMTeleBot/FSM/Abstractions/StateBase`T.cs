namespace FSMTeleBot.FSM.Abstractions;

public class StateBase<T>
{
    public string StateName { get; } = string.Empty;
    public T? Value { get; private set; }
}
