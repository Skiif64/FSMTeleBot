using FSMTeleBot.Abstractions;
using FSMTeleBot.States.Abstractions;

namespace FSMTeleBot.States;

public class ChatContext : IChatContext
{
    private readonly IChatStateStorage _storage;
    public long UserId { get; }
    public long ChatId { get; }
    public IChatState CurrentState { get; private set; }

    internal ChatContext(long chatId, long userId, IChatStateStorage storage, IChatState currentState)
    {
        ChatId = chatId;
        UserId = userId;
        _storage = storage;
        CurrentState = currentState;
    }

    public async Task SetStateAsync(IChatState state, CancellationToken cancellationToken = default)
    {
        CurrentState = state;
        await _storage
            .UpdateAsync(ChatId, UserId, CurrentState, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task FinishStateAsync(CancellationToken cancellationToken = default)
    {
        CurrentState = null; //TODO: Change this
        await _storage
            .UpdateAsync(ChatId, UserId, null, cancellationToken)
            .ConfigureAwait(false);
    }
}
