namespace FSMTeleBot.FSM;

public interface IFsmContextFactory
{
    Task<FsmContext> GetContextAsync(long chatId, long userId, CancellationToken cancellationToken = default);
}
