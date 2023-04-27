using FSMTeleBot.States.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FSMTeleBot.Handlers.Contexts;
public class MessageContext : HandlerContext<Message>
{
    public IChatContext ChatState { get; }
    public MessageContext(Message data, ITelegramBotClient client, IChatContext chatState) : base(data, client)
    {
        ChatState = chatState;
    }
}
