using FSMTeleBot.ChatMemberManager.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace FSMTeleBot.ChatMemberManager;

public class ChatMemberManager : IChatMemberManager
{
    private readonly ITelegramBotClient _client;
    
    public ChatMemberManager(ITelegramBotClient client)
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
