using System.Collections.Immutable;

namespace FSMTeleBot.FSM;

public interface IStateGroup
{
    ImmutableArray<IState> States { get; }
    IState this[int index] { get; }

    Task<IState> Next(IFsmContext context, CancellationToken cancellationToken = default);
}
