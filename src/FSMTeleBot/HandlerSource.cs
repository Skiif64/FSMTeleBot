namespace FSMTeleBot;

public class HandlerSource
{
    public Type HandlerType { get; }
    public Func<object[]?, object?> Source { get; }

    public HandlerSource(Type type)
    {
        HandlerType = type;
        Source = p => Activator.CreateInstance(type, p);
    }
}
