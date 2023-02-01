namespace FSMTeleBot.FSM;

public interface IStateStorage
{
    Task<IState> GetOrAddAsync(long chatId, long userId, IState toAddState, CancellationToken cancellationToken = default);
    Task UpdateAsync(long chatId, long userId, IState state, CancellationToken cancellationToken = default);
}
