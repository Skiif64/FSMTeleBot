namespace FSMTeleBot.FSM;

public interface IStateGroup
{
    IList<IState> States { get; }
    IState this[int index] { get; }

    Task<IState> Next(FsmContext context, CancellationToken cancellationToken = default);
}
