using FSMTeleBot.Abstractions;
using FSMTeleBot.States.Abstractions;

namespace FSMTeleBot.States;

public class ChatContext : IChatContext
{
    private readonly IChatStateStorage _storage;
    private readonly long _chatId;
    private readonly long _userId;
    public IChatState CurrentState { get; private set; }

    internal ChatContext(long chatId, long userId, IChatStateStorage storage, IChatState currentState)
    {
        _chatId = chatId;
        _userId = userId;
        _storage = storage;
        CurrentState = currentState;
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
        CurrentState = null; //TODO: Change this
        await _storage
            .UpdateAsync(_chatId, _userId, null, cancellationToken)
            .ConfigureAwait(false);
    }
}
