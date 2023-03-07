using FSMTeleBot.Abstractions;
using FSMTeleBot.ChatState.Abstractions;

namespace FSMTeleBot.ChatState;

public class ChatContext : IChatContext
{
    private readonly IChatStateStorage _storage;
    private readonly long _chatId;
    private readonly long _userId;    
    public IChatState CurrentState { get; private set; }

    internal ChatContext(long chatId, long userId, IChatStateStorage storage)
    {
        _chatId = chatId;
        _userId = userId;
        _storage = storage;
        CurrentState = storage.GetOrAddAsync(chatId, _userId, null).Result; //TODO: Change this
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
