namespace FSMTeleBot.FSM;

public class BotFSM : IBotFSM
{
    private readonly IStateStorage _storage;

    public BotFSM(IStateStorage storage)
    {
        _storage = storage;
    }

    public async Task<FsmContext> GetContextAsync(long chatId, long userId, CancellationToken cancellationToken = default)
    {
        var currentState = await _storage.GetOrAddAsync(chatId, userId, null, cancellationToken);// TODO: remove null
        var context = new FsmContext(chatId, userId, _storage);
        return context;
    }
}
