using FSMTeleBot.Abstractions;
using FSMTeleBot.ChatState.Abstractions;

namespace FSMTeleBot.ChatState;

public class ChatContext : IChatContext
{
    private readonly IChatStateStorage _storage;
    private readonly long _chatId;
    private readonly long _userId;
    private IChatStateGroup? _stateGroup; //TODO: Lazy load?    
    public IChatState? CurrentState { get; private set; }

    internal ChatContext(long chatId, long userId, IChatStateStorage storage)
    {
        _chatId = chatId;
        _userId = userId;
        _storage = storage;
    }

    public async Task SetStateGroupAsync(IChatStateGroup stateGroup, CancellationToken cancellationToken = default)
    {
        _stateGroup = stateGroup;
        CurrentState = stateGroup[0];
        await _storage
            .UpdateAsync(_chatId, _userId, CurrentState, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task SetStateAsync(IChatState state, CancellationToken cancellationToken = default)
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
