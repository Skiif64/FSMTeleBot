using FSMTeleBot.Abstractions;
using FSMTeleBot.States.Abstractions;

namespace FSMTeleBot.States;

public class ChatContextFactory : IChatContextFactory
{
    private readonly IChatStateStorage _storage;

    public ChatContextFactory(IChatStateStorage storage)
    {
        _storage = storage;
    }

    public async Task<ChatContext> GetContextAsync(long chatId, long userId, CancellationToken cancellationToken = default)
    {
        var currentState = await _storage.GetOrInit(chatId, userId, cancellationToken);
        var context = new ChatContext(chatId, userId, _storage, currentState);
        return context;
    }
}
