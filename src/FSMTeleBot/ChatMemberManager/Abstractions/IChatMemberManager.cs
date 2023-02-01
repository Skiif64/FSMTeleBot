using Telegram.Bot.Types.Enums;

namespace FSMTeleBot.ChatMemberManager.Abstractions;

public interface IChatMemberManager
{
    Task<ChatMemberStatus> GetChatMemberStatus(long chatId, long userId, CancellationToken cancellationToken = default);
}
