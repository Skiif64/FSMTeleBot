using FSMTeleBot.Abstractions;
using FSMTeleBot.Internal.UpdateDescriptors;
using FSMTeleBot.States.Abstractions;

namespace FSMTeleBot.States;

internal class ChatContextFactory<TMessage> : IChatContextFactory //Move to internal folder
{
    private readonly IChatStateStorage _storage;
    private readonly IUpdateDescriptor<TMessage> _descriptor;
    private readonly TMessage _message;
    public ChatContextFactory(IChatStateStorage storage, TMessage message, IUpdateDescriptor<TMessage> descriptor)
    {
        _storage = storage;
        _message = message;
        _descriptor = descriptor;
    }

    public async Task<ChatContext> GetContextAsync(CancellationToken cancellationToken = default)
    {
        var chatId = _descriptor.GetChatId(_message);
        var userId = _descriptor.GetUserId(_message);
        var currentState = await _storage.GetOrInitAsync(chatId, userId, cancellationToken);
        var context = new ChatContext(chatId, userId, _storage, currentState);
        return context;
    }
}
