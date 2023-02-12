using FSMTeleBot.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace FSMTeleBot.Services;

public class ChatMemberService : IChatMemberService
{
    private readonly ITelegramBotClient _client;

    public ChatMemberService(ITelegramBotClient client)
    {
        _client = client;
    }

    //TODO: Shorten name?
    public virtual async Task<ChatMemberStatus> GetStatus(long chatId, long userId, CancellationToken cancellationToken = default)
    {
        var member = await _client.GetChatMemberAsync(chatId, userId, cancellationToken);
        return member.Status;
    }
}
