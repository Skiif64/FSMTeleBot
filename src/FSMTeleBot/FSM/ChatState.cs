namespace FSMTeleBot.FSM;

public class ChatState : IState
{
    public string Name { get; }
    public ChatState(string name)
    {
        Name = name;
    }
}
