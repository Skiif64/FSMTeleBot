using Telegram.Bot.Types.Enums;

namespace FSMTeleBot.Internal.UpdateDescriptors;

internal interface IUpdateDescriptor<TMessage>
{
    long GetChatId(TMessage message);
    long GetUserId(TMessage message);
}
