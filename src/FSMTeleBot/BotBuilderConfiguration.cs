using FSMTeleBot.Abstractions;
using Telegram.Bot;

namespace FSMTeleBot;

public class BotBuilderConfiguration
{
    public ITelegramBotClient? TelegramBotClient { get; private set; }
    public IChatStateStorage? StateStorage { get; private set; }
    public IChatMemberService? MemberService { get; private set; }
}
