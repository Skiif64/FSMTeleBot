using FSMTeleBot.Abstractions;
using FSMTeleBot.Internal.UpdateDescriptors;
using FSMTeleBot.States;
using FSMTeleBot.States.Abstractions;

namespace FSMTeleBot.Internal;

internal class ChatContextFactory : IChatContextFactory //Move to internal folder
{
    private readonly IChatStateStorage _storage;
    private readonly IEnumerable<IUpdateDescriptor> _descriptors;
    public ChatContextFactory(IChatStateStorage storage, IEnumerable<IUpdateDescriptor> descriptors)
    {
        _storage = storage;
        _descriptors = descriptors;
    }

    public async Task<IChatContext> GetContextAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
    {
        if (message is null)
            throw new ArgumentNullException(nameof(message));

        var descriptor = _descriptors.FirstOrDefault(d => d.Type == typeof(TMessage));
        if (descriptor is null)
            throw new Exception(nameof(descriptor)); //TODO: normal exception

        var chatId = descriptor.GetChatId(message);
        var userId = descriptor.GetUserId(message);
        var currentState = await _storage.GetOrInitAsync(chatId, userId, cancellationToken);
        var context = new ChatContext(chatId, userId, _storage, currentState);
        return context;
    }
}
