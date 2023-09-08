using Telegram.Bot.Types.Enums;

namespace FSMTeleBot.Handlers.Abstractions;

public interface IUpdateDescriptor
{
    Type Type { get; }
    long GetChatId(object message);
    long GetUserId(object message);
}
