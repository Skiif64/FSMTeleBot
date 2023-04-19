using FSMTeleBot.Handlers.Abstractions;
using Telegram.Bot.Types;

namespace FSMTeleBot.Internal.UpdateDescriptors;

internal class MessageDescriptor : IUpdateDescriptor
{
    public Type Type => typeof(Message);

    public long GetChatId(Message message) => message.Chat.Id;

    public long GetChatId(object message)
        => GetChatId((Message)message);
    
    public long GetUserId(Message message) => message.From?.Id ?? throw new Exception(nameof(message)); //TODO: normal exception
    public long GetUserId(object message)
        => GetUserId((Message)message);   
}
