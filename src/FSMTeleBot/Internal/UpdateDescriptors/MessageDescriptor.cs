using Telegram.Bot.Types;

namespace FSMTeleBot.Internal.UpdateDescriptors;

internal class MessageDescriptor : IUpdateDescriptor<Message>
{
    public long GetChatId(Message message) => message.Chat.Id;

    public long GetUserId(Message message) => message.From?.Id ?? throw new Exception(nameof(message)); //TODO: normal exception
}
