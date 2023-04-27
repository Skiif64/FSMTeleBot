using FSMTeleBot.States.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FSMTeleBot.Handlers.Contexts;
public class MessageContext : HandlerContext<Message>
{
    public IChatState ChatState { get; }
    public MessageContext(Message data, ITelegramBotClient client, IChatState chatState) : base(data, client)
    {
        ChatState = chatState;
    }
}
