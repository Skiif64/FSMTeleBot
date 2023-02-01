using Telegram.Bot.Types.Enums;

namespace FSMTeleBot.Abstractions;

public interface IChatMemberService
{
    Task<ChatMemberStatus> GetStatus(long chatId, long userId, CancellationToken cancellationToken = default);
}
