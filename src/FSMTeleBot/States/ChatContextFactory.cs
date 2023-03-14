using FSMTeleBot.Abstractions;
using FSMTeleBot.Internal.UpdateDescriptors;
using FSMTeleBot.States.Abstractions;

namespace FSMTeleBot.States;

internal class ChatContextFactory<TMessage> : IChatContextFactory<TMessage> //Move to internal folder
{
    private readonly IChatStateStorage _storage;
    private readonly IUpdateDescriptor<TMessage> _descriptor;    
    public ChatContextFactory(IChatStateStorage storage, IUpdateDescriptor<TMessage> descriptor)
    {
        _storage = storage;        
        _descriptor = descriptor;
    }

    public async Task<IChatContext> GetContextAsync(TMessage message, CancellationToken cancellationToken = default)
    {
        var chatId = _descriptor.GetChatId(message);
        var userId = _descriptor.GetUserId(message);
        var currentState = await _storage.GetOrInitAsync(chatId, userId, cancellationToken);
        var context = new ChatContext(chatId, userId, _storage, currentState);
        return context;
    }
}
