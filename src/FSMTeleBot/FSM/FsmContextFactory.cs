namespace FSMTeleBot.FSM;

public class FsmContextFactory : IFsmContextFactory
{
    private readonly IStateStorage _storage;

    public FsmContextFactory(IStateStorage storage)
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
