using FSMTeleBot.ChatState;
using FSMTeleBot.ChatState.Abstractions;

namespace FSMTeleBot.Tests.FSM;

public class FakeStateGroup : ChatStateGroup
{
    public static IChatState State1 { get; set; }
    public static IChatState State2 { get; set; }
    public static IChatState State3 { get; set; }
    public FakeStateGroup()
    {
        InitState(this);
    }

}
