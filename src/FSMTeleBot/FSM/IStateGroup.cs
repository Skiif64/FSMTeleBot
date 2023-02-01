namespace FSMTeleBot.FSM;

public interface IStateGroup
{   
    IState this[int index] { get; }

    Task<IState> Next(FsmContext context, CancellationToken cancellationToken = default);
}
