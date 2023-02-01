namespace FSMTeleBot.FSM;

public class FsmContext : IFsmContext
{
    private readonly IStateStorage _storage;
    private readonly long _chatId;
    private readonly long _userId;
    private IStateGroup? _stateGroup; //TODO: Lazy load?    
    public IState? CurrentState { get; private set; }

    internal FsmContext(long chatId, long userId, IStateStorage storage)
    {
        _chatId = chatId;
        _userId = userId;
        _storage = storage;
    }

    public async Task SetStateGroupAsync(IStateGroup stateGroup, CancellationToken cancellationToken = default)
    {
        _stateGroup = stateGroup;
        CurrentState = stateGroup[0];
        await _storage
            .UpdateAsync(_chatId, _userId, CurrentState, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task SetStateAsync(IState state, CancellationToken cancellationToken = default)
    {
        CurrentState = state;
        await _storage
            .UpdateAsync(_chatId, _userId, CurrentState, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task FinishStateAsync(CancellationToken cancellationToken = default)
    {
        _stateGroup = null;
        CurrentState = null;
        await _storage
            .UpdateAsync(_chatId, _userId, null, cancellationToken)
            .ConfigureAwait(false);
    }
}
