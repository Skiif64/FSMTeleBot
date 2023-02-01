namespace FSMTeleBot.FSM;

public interface IBotFSM
{
    Task<FsmContext> GetContextAsync(long chatId, long userId, CancellationToken cancellationToken = default);
}
