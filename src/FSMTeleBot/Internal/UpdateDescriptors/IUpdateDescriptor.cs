using Telegram.Bot.Types.Enums;

namespace FSMTeleBot.Internal.UpdateDescriptors;

internal interface IUpdateDescriptor
{
    Type Type { get; }
    long GetChatId(object message);
    long GetUserId(object message);
}
