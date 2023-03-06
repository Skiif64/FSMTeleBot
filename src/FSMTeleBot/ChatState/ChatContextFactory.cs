using FSMTeleBot.Abstractions;
using FSMTeleBot.ChatState.Abstractions;

namespace FSMTeleBot.ChatState;

public class ChatContextFactory : IChatContextFactory
{
    private readonly IChatStateStorage _storage;

    public ChatContextFactory(IChatStateStorage storage)
    {
        _storage = storage;
    }

    public async Task<ChatContext> GetContextAsync(long chatId, long userId, CancellationToken cancellationToken = default)
    {
        var currentState = await _storage.GetOrAddAsync(chatId, userId, null, cancellationToken);// TODO: remove null
        var context = new ChatContext(chatId, userId, _storage);
        return context;
    }
}
