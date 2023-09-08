using Telegram.Bot.Types.Enums;

namespace FSMTeleBot.Abstractions;

public interface IChatMemberService
{
    Task<ChatMemberStatus> GetStatusAsync(long chatId, long userId, CancellationToken cancellationToken = default);
}
