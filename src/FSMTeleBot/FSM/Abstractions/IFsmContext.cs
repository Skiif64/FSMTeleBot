
namespace FSMTeleBot.FSM
{
    public interface IFsmContext
    {
        IState? CurrentState { get; }

        Task FinishStateAsync(CancellationToken cancellationToken = default);
        Task SetStateAsync(IState state, CancellationToken cancellationToken = default);
        Task SetStateGroupAsync(IStateGroup stateGroup, CancellationToken cancellationToken = default);
    }
}